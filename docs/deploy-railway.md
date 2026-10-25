# Publicação demonstrativa no Railway

Este roteiro prepara uma publicação manual da aplicação demonstrativa. As imagens e configurações Railway estão versionadas no repositório; a associação do repositório, o provisionamento do banco, os domínios e as variáveis são ações do operador no painel Railway. Este projeto não cria repositório, configura remoto, executa `gh`, envia commits ou publica serviços.

> **Pré-requisito:** os arquivos desta change devem estar em um repositório GitHub existente, na branch escolhida para deploy. A criação/publicação inicial do repositório não faz parte deste roteiro. Confirme também qual branch será usada; a configuração não pressupõe `main`.

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

## 1. Prepare os serviços no painel

1. Crie um projeto/ambiente Railway para a demonstração e adicione um serviço PostgreSQL `17`. Aguarde o serviço ficar disponível. Confirme no painel que os dados estão em armazenamento persistente e revise a política de backup/restauração disponível no plano usado; não assuma que backups foram configurados automaticamente.
2. Adicione um serviço de aplicação ligado ao repositório existente para a API. Em **Settings**, mantenha **Root Directory** em `/` e selecione como arquivo de configuração Railway `/apps/backend/railway.json`.
3. Adicione outro serviço ligado ao mesmo repositório para o frontend. Mantenha **Root Directory** em `/` e selecione `/apps/frontend/railway.json`.
4. Para cada serviço de aplicação, gere um domínio público HTTPS no painel. Use os domínios atribuídos pelo Railway, não nomes internos `*.railway.internal`, para as URLs consumidas pelo navegador.
5. Defina a branch de deploy de acordo com o fluxo de apresentação acordado. A integração GitHub do Railway observa essa branch; não é configurada por um job de deploy neste repositório.

Os arquivos Railway selecionam o builder Dockerfile, os caminhos de build e as verificações de disponibilidade. Eles não criam serviços, domínio, volume, secrets, Root Directory, associação GitHub nem ativam **Wait for CI**. A rota `/api/courses` é o healthcheck da API: enquanto banco, migrations ou seed impedirem a resposta HTTP, o novo serviço ainda não estará pronto. O timeout declarado é 300 segundos para a API e 60 para o frontend; a política de reinício é `ON_FAILURE`, com até três tentativas, e cada serviço usa uma réplica.

## 2. Configure as variáveis

Use variáveis do serviço, e não valores versionados em Dockerfiles, workflow ou `.env.example`. O exemplo abaixo supõe que o serviço de banco se chama `Postgres`; se o nome escolhido for outro, ajuste cada referência ao nome exato no Railway.

### API — runtime

| Variável | Valor |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | `Host=${{Postgres.PGHOST}};Port=${{Postgres.PGPORT}};Database=${{Postgres.PGDATABASE}};Username=${{Postgres.PGUSER}};Password=${{Postgres.PGPASSWORD}}` |
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

No painel Railway, habilite **Wait for CI** para os serviços ligados ao repositório depois de confirmar que a workflow aparece e conclui para a branch escolhida. A integração Railway mantém o deploy aguardando os check suites da GitHub Actions; uma CI com falha não deve liberar o novo deploy. Essa opção é configuração externa e não é ativada por este repositório. Sem ela, a integração automática pode tentar publicar um push independentemente do resultado dos testes.

Para validar as imagens localmente antes de conectar o repositório, execute `bash scripts/smoke-deployment.sh` a partir da raiz. O host precisa de Bash, Docker com daemon ativo, `curl`, Python 3 e acesso aos registries necessários para buscar as imagens base e dependências. O script usa nomes exclusivos para imagens, containers e rede de teste, inicializa PostgreSQL efêmero e limpa apenas os recursos que criou; não usa o banco do Compose nem requer SDK .NET ou Node no host. Ele verifica o seed, as rotas da SPA, assets, headers e origens configuradas, sem publicar nada no Railway.

Não interprete builds locais ou a existência da workflow como comprovação de que o repositório foi associado, **Wait for CI** foi habilitado ou uma publicação externa foi executada. Essas ações permanecem no painel e ficam a cargo do operador.

## 5. Migrations no startup: decisão da demonstração

Com `Database__InitializeOnStartup=true`, o processo API aguarda `MigrateAsync` e o callback de seed idempotente antes de começar a atender HTTP. Assim, a primeira inicialização de um PostgreSQL novo cria o esquema versionado e o conjunto fictício sem executar comandos EF no host ou no build. Cada reinício verifica migrations pendentes e o seed existente não apaga nem sobrescreve valores; a opção **não** é um reset de banco.

Esta escolha é uma simplificação consciente para uma apresentação com uma réplica e dados demonstrativos. A mesma identidade de runtime recebe privilégios para alterar o esquema; se o PostgreSQL estiver indisponível, faltar permissão ou houver erro de migration/seed, a API não fica pronta e o deploy pode falhar após as tentativas limitadas. O timeout de healthcheck não interrompe nem corrige uma migration em andamento.

Em produção, prefira migrations como etapa de deploy separada, credencial de migration com privilégios DDL distintos da identidade da API e rollout que considere compatibilidade entre versão antiga e nova. Reverter o código implantado não executa migrations `Down` nem restaura dados automaticamente. Não apague o banco como forma de recuperação; examine logs/estado do esquema e restaure a partir de backup validado quando necessário.

## 6. Verificação após a publicação

Confirme manualmente no painel e nos domínios públicos:

- Deploy da API pronto após migrations e seed; `GET /api/courses` retorna seis cursos e `GET /api/courses/1` retorna detalhes/apresentação.
- Swagger/OpenAPI não é exposto na configuração Production.
- Landing, detalhes e AVA carregam dados do domínio API e as URLs de imagem retornadas carregam do domínio frontend.
- Entrada direta/refresh nas rotas `/`, `/cursos/1`, `/ava/1`, aliases e página desconhecida preservam o comportamento SPA; asset inexistente retorna 404.
- A origem da API no bundle corresponde ao domínio HTTPS público, CORS permite somente leitura conforme a configuração existente, e o navegador não recebe credenciais de banco.
- Reiniciar somente os serviços de aplicação preserva os dados PostgreSQL. Não valide isso removendo o volume ou apagando o banco.

Não há publicação Railway, serviço remoto, repositório, domínio nem configuração externa realizados por este roteiro. Faça essa operação apenas depois de escolher o repositório/branch e revisar os arquivos e as variáveis no painel.

## 7. Verificações locais desta preparação

Em 2026-10-24, as verificações locais realizadas foram:

- Backend: `dotnet tool restore`, `dotnet restore`, `dotnet build` e `dotnet test` em `apps/backend`; build sem warnings/erros e 193 testes aprovados (unitários e integração/Testcontainers) contra PostgreSQL descartável.
- Frontend: `pnpm frontend:typecheck` e `VITE_API_BASE_URL=https://api.example pnpm frontend:build` na raiz passaram. O Vite reportou o bundle JavaScript com 516.78 kB minificados, acima do limite informativo de 500 kB; não foi configurado lint.
- Imagens: uma execução integral de `bash scripts/smoke-deployment.sh` passou com PostgreSQL efêmero, API e Nginx; incluía verificação de migrations/seed, reinício idempotente, portas, rotas, assets, cache e rejeição de origens inválidas. Uma repetição posterior não completou: BuildKit/WSL falhou ao obter credenciais ao resolver `nginx:stable-alpine`, antes das verificações do script. Depois disso, o teste de navegador abaixo reconstruiu e executou imagens locais com sucesso. A falha de infraestrutura da repetição não é contada como aprovação.
- Navegador: Chromium local com PostgreSQL e containers descartáveis verificou Landing, seis rotas de Detalhes, seis rotas AVA, aliases, retorno, página não encontrada, assets e CORS. As 15 requisições ao catálogo observadas eram GET e retornaram HTTP 200; não houve erros JavaScript ou de console.
- CI: o YAML foi analisado localmente; os comandos equivalentes de backend, frontend e smoke foram executados nesta máquina. A workflow não foi executada num repositório GitHub.
- Railway: associação do repositório, configuração de serviços/domínios/variáveis, **Wait for CI** e deploy remoto não foram executados.

Os builds locais usaram somente origens e credenciais fictícias. O PostgreSQL de teste foi efêmero e separado do banco local/Compose; nenhum valor de produção foi inserido em imagens ou no bundle. Os resultados acima não substituem a configuração no painel nem a revisão dos logs e domínios na implantação futura.
