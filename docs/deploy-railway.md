# Publicação no Railway

A aplicação está publicada no Railway no projeto `fearless-spirit`, ambiente `production`, a partir do [repositório GitHub](https://github.com/FelipeOliveiraDvP/sesi-industria) e da branch `main`.

| Serviço | Endereço/uso |
| --- | --- |
| Frontend (`@sesi/frontend`) | [https://sesifrontend-production.up.railway.app/](https://sesifrontend-production.up.railway.app/) |
| API (`sesi-api`) | [https://sesi-api-production.up.railway.app](https://sesi-api-production.up.railway.app) |
| Listagem de cursos | [GET /api/courses](https://sesi-api-production.up.railway.app/api/courses) |
| Detalhes do curso 1 | [GET /api/courses/1](https://sesi-api-production.up.railway.app/api/courses/1) |
| Banco (`postgres-17`) | PostgreSQL 17, acessível pela API na rede privada |

O frontend e a API estão publicados em domínios HTTPS separados. O navegador chama a API; a API conecta-se ao PostgreSQL. O repositório contém o código e as instruções para reproduzir a configuração.

## Arquitetura

```text
Navegador --> Frontend/Nginx (dominio HTTPS publico)
    |
    +------> API .NET (dominio HTTPS publico)
                 |
                 +--> PostgreSQL 17 (rede privada e armazenamento persistente)

GitHub Actions --> resultado CI --> Railway aguarda CI quando habilitado
```

O frontend é uma SPA estática independente. O navegador precisa alcançar publicamente frontend e API, pois é ele quem consulta `GET /api/courses` e `GET /api/courses/{id}`. Somente a API precisa alcançar o PostgreSQL; não habilite acesso público ao banco para este fluxo. As imagens dos cursos são servidas pelos assets do frontend.

Os Dockerfiles são construídos com a raiz do monorepo como contexto, para que a imagem frontend possa usar `pnpm-lock.yaml` e `pnpm-workspace.yaml`. Não defina `apps/frontend` ou `apps/backend` como Root Directory do serviço.

## 1. Configuração dos serviços

Os serviços existentes usam a raiz do monorepo (`/`) como contexto de build e a branch `main` como origem. Para recriar a implantação, associe os serviços de aplicação ao repositório acima, crie o PostgreSQL 17 e configure os domínios e variáveis descritos neste guia. O frontend e a API precisam de domínios públicos HTTPS; o banco não precisa de endpoint TCP público.

Nos serviços ativos, o caminho do Dockerfile é configurado pela variável `RAILWAY_DOCKERFILE_PATH`. Os arquivos `apps/backend/railway.json` e `apps/frontend/railway.json` são configurações legadas; não os selecione como configuração ativa ao recriar os serviços. Mantenha **Root Directory** em `/` para que o frontend tenha acesso aos manifests do workspace pnpm. A rota `/api/courses` é o healthcheck da API: enquanto banco, migrations ou seed impedirem a resposta HTTP, o serviço não estará pronto.

## 2. Configure as variáveis

Use variáveis do serviço, e não valores versionados em Dockerfiles, workflow ou `.env.example`. O exemplo abaixo usa `postgres-17` como nome do serviço de banco; se outro nome for escolhido, ajuste as referências para o nome exato no Railway.

### API — runtime

| Variável | Valor |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | `Host=${{postgres-17.PGHOST}};Port=${{postgres-17.PGPORT}};Database=${{postgres-17.PGDATABASE}};Username=${{postgres-17.PGUSER}};Password=${{postgres-17.PGPASSWORD}}` |
| `FRONTEND_URL` | `https://<dominio-publico-do-frontend>` |
| `Database__InitializeOnStartup` | `true` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

Configure a origem do frontend como origem HTTP/HTTPS sem caminho adicional, credenciais, query ou fragmento. `FRONTEND_URL` é usada para compor as URLs públicas das imagens; a API não faz uma consulta de disponibilidade ao frontend durante o startup. O CORS do catálogo é público, somente para GET e sem credenciais; ele não é autenticação.

A connection string deve usar as referências internas das variáveis PostgreSQL para que host e tráfego permaneçam privados. Se o valor da senha contiver caracteres especiais da sintaxe de connection string, escape/quote o valor conforme a sintaxe do Npgsql. Não coloque credenciais em `DATABASE_URL` esperando que a API a converta automaticamente: o backend consome `ConnectionStrings__DefaultConnection`.

### Frontend — build

| Variável | Valor |
| --- | --- |
| `VITE_API_BASE_URL` | `https://<dominio-publico-da-api>` |

O Dockerfile declara `ARG VITE_API_BASE_URL` porque Vite incorpora `VITE_` no bundle durante o build. O Railway deve receber esta variável antes da construção da imagem frontend. Ela é pública e não pode conter senha ou token. Alterar a variável de runtime depois de construir a imagem não altera o bundle: atualize o valor de build e gere outro deploy.

`PORT` é fornecida em runtime pelo Railway; os dois contêineres aceitam uma porta diferente de `8080` e têm padrão local `8080`. Não a fixe manualmente em outro valor no painel.

## 3. Evite a dependência circular das origens

O backend precisa saber a origem pública do frontend para produzir URLs de imagem; o bundle frontend precisa saber a origem pública da API. Para resolver ambas antes dos builds:

1. Crie os serviços e gere os dois domínios públicos no painel.
2. Cadastre `FRONTEND_URL` na API e `VITE_API_BASE_URL` como variável de build do frontend.
3. Cadastre também a conexão privada PostgreSQL, `ASPNETCORE_ENVIRONMENT=Production` e `Database__InitializeOnStartup=true` na API.
4. Inicie os deploys. Se domínios/variáveis forem alterados depois, atualize as duas origens correspondentes; reconstruir o frontend é necessário quando muda `VITE_API_BASE_URL`.

Não habilite Swagger/OpenAPI em Production: a aplicação os expõe apenas em Development. O frontend pode servir as rotas de página por fallback SPA, mas solicitações a assets inexistentes continuam sendo 404.

## 4. CI e publicação

`.github/workflows/ci.yml` executa no GitHub Actions para pushes, pull requests e execução manual. Ela verifica o workspace frontend, os testes backend com PostgreSQL Testcontainers e os smoke tests das imagens. A workflow tem permissão somente de leitura do conteúdo e não usa token Railway, `gh`, publicação de imagens ou secrets externos.

**Estado do gate (26/10/2026):** a execução [40247429083](https://github.com/FelipeOliveiraDvP/sesi-industria/actions/runs/40247429083) passou para o commit `7e7cc8c` em `main`, mas `Wait for CI` ainda aparece desativado (`source.checkSuites=false`) nos serviços frontend e API. A ativação precisa ser concluída nas configurações de origem dos dois serviços Railway e confirmada antes de considerar o gate efetivo. Enquanto isso não ocorrer, a integração pode iniciar um deploy sem aguardar a conclusão da CI.

Para habilitar o gate, ative **Wait for CI** nas configurações dos serviços ligados ao repositório e confirme que ambos passam a reportar `source.checkSuites=true`. Faça isso sem alterar a associação ao repositório, branch ou demais campos da origem; depois, valide o comportamento em um push de teste e confirme que o deploy aguarda os checks.

Para validar as imagens localmente antes de conectar o repositório ao Railway, execute `bash scripts/smoke-deployment.sh` a partir da raiz. O host precisa de Bash, Docker com daemon ativo, `curl`, Python 3 e acesso aos registries necessários para buscar as imagens base e dependências. O script usa nomes exclusivos para imagens, containers e rede de teste, inicializa PostgreSQL efêmero e limpa apenas os recursos que criou; não usa o banco do Compose nem requer SDK .NET ou Node no host. Ele verifica o seed, as rotas da SPA, assets, headers e origens configuradas, sem publicar nada no Railway.

Não interprete builds locais ou a existência da workflow como comprovação de que **Wait for CI** está habilitado. A CI e a publicação Railway são integrações distintas; confira o estado efetivo do gate no painel/configuração Railway.

## 5. Migrations no startup: decisão da demonstração

Com `Database__InitializeOnStartup=true`, o processo API aguarda `MigrateAsync` e o callback de seed idempotente antes de começar a atender HTTP. Assim, a primeira inicialização de um PostgreSQL novo cria o esquema versionado e o conjunto fictício sem executar comandos EF no host ou no build. Cada reinício verifica migrations pendentes e o seed existente não apaga nem sobrescreve valores; a opção **não** é um reset de banco.

Esta escolha é uma simplificação consciente para uma apresentação com uma réplica e dados demonstrativos. A mesma identidade de runtime recebe privilégios para alterar o esquema; se o PostgreSQL estiver indisponível, faltar permissão ou houver erro de migration/seed, a API não fica pronta e o deploy pode falhar após as tentativas limitadas. O timeout de healthcheck não interrompe nem corrige uma migration em andamento.

Em produção, prefira migrations como etapa de deploy separada, credencial de migration com privilégios DDL distintos da identidade da API e rollout que considere compatibilidade entre versão antiga e nova. Reverter o código implantado não executa migrations `Down` nem restaura dados automaticamente. Não apague o banco como forma de recuperação; examine logs/estado do esquema e restaure a partir de backup validado quando necessário.

## 6. Verificação após a publicação

As verificações manuais da publicação foram realizadas. Para uma nova versão, confira no painel e nos domínios públicos:

- Deploy da API pronto após migrations e seed; `GET /api/courses` retorna seis cursos e `GET /api/courses/1` retorna detalhes/apresentação.
- Swagger/OpenAPI não é exposto na configuração Production.
- Landing, detalhes e AVA carregam dados do domínio API e as URLs de imagem retornadas carregam do domínio frontend.
- Entrada direta/refresh nas rotas `/`, `/cursos/1`, `/ava/1`, aliases e página desconhecida preservam o comportamento SPA; asset inexistente retorna 404.
- A origem da API no bundle corresponde ao domínio HTTPS público, CORS permite somente leitura conforme a configuração existente, e o navegador não recebe credenciais de banco.
- Reiniciar somente os serviços de aplicação preserva os dados PostgreSQL. Não valide isso removendo o volume ou apagando o banco.

O estado desta implantação foi confirmado em 26/10/2026: os serviços frontend, API e PostgreSQL estão ativos; os domínios públicos acima atendem a aplicação e a API. A ativação de **Wait for CI** permanece pendente conforme a seção anterior.

## 7. Verificações locais desta preparação

Em 2026-10-24, as verificações locais realizadas foram:

- Backend: `dotnet tool restore`, `dotnet restore`, `dotnet build` e `dotnet test` em `apps/backend`; build sem warnings/erros e 193 testes aprovados (unitários e integração/Testcontainers) contra PostgreSQL descartável.
- Frontend: `pnpm frontend:typecheck` e `VITE_API_BASE_URL=https://api.example pnpm frontend:build` na raiz passaram. O Vite reportou o bundle JavaScript com 516.78 kB minificados, acima do limite informativo de 500 kB; não foi configurado lint.
- Imagens: uma execução integral de `bash scripts/smoke-deployment.sh` passou com PostgreSQL efêmero, API e Nginx; incluía verificação de migrations/seed, reinício idempotente, portas, rotas, assets, cache e rejeição de origens inválidas. Uma repetição posterior não completou: BuildKit/WSL falhou ao obter credenciais ao resolver `nginx:stable-alpine`, antes das verificações do script. Depois disso, o teste de navegador abaixo reconstruiu e executou imagens locais com sucesso. A falha de infraestrutura da repetição não é contada como aprovação.
- Navegador: Chromium local com PostgreSQL e containers descartáveis verificou Landing, seis rotas de Detalhes, seis rotas AVA, aliases, retorno, página não encontrada, assets e CORS. As 15 requisições ao catálogo observadas eram GET e retornaram HTTP 200; não houve erros JavaScript ou de console.
- CI: o run [40247429083](https://github.com/FelipeOliveiraDvP/sesi-industria/actions/runs/40247429083) concluiu com sucesso para o commit `7e7cc8c` em `main`.
- Railway: o frontend e a API estão associados à branch `main`, e os serviços públicos estão ativos. **Wait for CI** ainda não está habilitado efetivamente; ver a seção [CI e publicação](#4-ci-e-publicação).

Os builds locais usaram somente origens e credenciais fictícias. O PostgreSQL de teste foi efêmero e separado do banco local/Compose; nenhum valor de produção foi inserido em imagens ou no bundle. Os resultados locais não substituem a revisão dos logs e do estado efetivo dos serviços após futuras alterações.
