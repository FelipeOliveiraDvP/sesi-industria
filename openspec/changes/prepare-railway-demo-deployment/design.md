# Design

## Context

Ver `proposal.md`, seção Why, para a motivação. A inspeção do projeto identificou os pontos relevantes para o desenho:

- `apps/frontend` é uma SPA React/Vite com pnpm `11.9.0` e lockfile/workspace na raiz. O build já verifica tipos; não existem comandos de lint ou testes unitários frontend.
- `VITE_API_BASE_URL` é incorporada ao bundle. A API exige `FRONTEND_URL` para compor URLs dos assets que só o frontend serve; não consulta a disponibilidade desse host ao iniciar.
- A API utiliza .NET/EF Core 10 e PostgreSQL 17. `Program.cs` não migra o banco, e `StartupConfigurationTests.DoesNotCreateSchemaOrRunSeedDuringHttpStartup` comprova esse comportamento atual.
- `CatalogDbContext.OnConfiguring` já registra `UseSeeding` e `UseAsyncSeeding`. `CatalogSeed` insere somente IDs ausentes, inclusive em um grafo parcialmente inicializado. `SeedInitializationTests` já cobre idempotência e preservação de valores existentes.
- Existem três migrations, incluindo a adição de campos editoriais obrigatórios e a atualização JSONB do AVA. A sequência atende um banco vazio; não é uma promessa de atualização de qualquer banco legado populado.
- Não há Dockerfiles, arquivos Railway ou workflows existentes a adaptar. Há alterações locais não commitadas de outras entregas; não reverter, reorganizar ou incluir esse trabalho automaticamente em commits.

O delta de `backend-local-runtime` delimita a exceção à preparação exclusivamente manual atual. As capacidades de persistência e de consumo da API não mudam.

## Goals / Non-Goals

**Goals:**

- Fazer o empacotamento funcionar sem alterar a separação de desenvolvimento do monorepo.
- Manter URLs públicas de navegador, conexão privada do banco e fases de build/runtime inequívocas.
- Testar o comportamento real das imagens sem usar dados ou contas da demonstração.
- Isolar a simplificação de startup em uma opção explícita, com falha antes do atendimento HTTP.

**Non-Goals:**

- Não unificar frontend e API no mesmo processo nem transformar o Nginx em proxy da API.
- Não criar endpoint de saúde, projetos .NET adicionais, runner de testes frontend ou dependências de aplicação apenas para o deploy.
- Não fazer migrations no build, em pré-deploy, por bundle ou por comandos EF dentro da imagem de runtime.
- Não configurar produção de alta disponibilidade, múltiplas regiões, rollback automático de banco ou integração externa por CLI/API.

## Decisions

### 1. Dois serviços de aplicação e PostgreSQL separado

Manter um serviço `frontend` e um serviço `api` no mesmo projeto/ambiente Railway, além de um serviço PostgreSQL 17 com volume persistente. Gerar domínios públicos para os serviços de aplicação; a API usa a rede privada para acessar o banco.

```text
Repositorio futuro --> GitHub Actions --> resultado de CI
        |
        +--> Railway (aguarda CI quando habilitado)
                  |
                  +--> Frontend/Nginx (publico)
                  +--> API .NET (publica)
                  +--> PostgreSQL (privado, persistente)

Navegador --> Frontend/Nginx
    |
    +------> API .NET --> rede privada --> PostgreSQL
```

As duas setas de navegador são tráfego HTTP público. Somente a API recebe a conexão do PostgreSQL. Não usar `*.railway.internal` em `VITE_API_BASE_URL` nem em URLs de imagens.

**Alternativa:** uma imagem única serviria a SPA pela API e reduziria serviços, mas mudaria roteamento e empacotamento sem necessidade. A separação escolhida preserva o fluxo atual e atende ao Nginx solicitado.

### 2. Contexto de build na raiz, Dockerfiles por aplicação

Adicionar `apps/backend/Dockerfile` e `apps/frontend/Dockerfile`, ambos construídos com a raiz como contexto. Isso mantém disponíveis `package.json`, `pnpm-workspace.yaml` e `pnpm-lock.yaml` para o frontend; o backend copia apenas seu projeto necessário.

- **API:** estágio SDK oficial .NET 10 para restore/publish Release e estágio final `mcr.microsoft.com/dotnet/aspnet:10.0`, com apenas a saída publicada e execução como usuário não privilegiado suportado pela imagem. Não incluir SDK, ferramenta EF ou testes no runtime.
- **Frontend:** estágio `node:24-alpine`, instalação explícita do pnpm declarado no projeto e `pnpm install --frozen-lockfile` pela raiz, seguido do build de `@sesi/frontend`. O estágio final `nginx:stable-alpine` recebe somente `apps/frontend/dist` e o template do servidor; não recebe Node, pnpm ou `node_modules`.
- **Contexto:** `.dockerignore` na raiz exclui `.git`, configurações `.env` locais em qualquer nível, `node_modules`, `bin`, `obj`, `dist`, resultados de testes e saídas locais das ferramentas. Não excluir migrations, código de seed, assets ou os manifestos necessários.
- Copiar manifestos antes dos fontes permite cache de dependências, mas nenhuma saída local preexistente será insumo do build.

`ARG VITE_API_BASE_URL` pertence ao estágio que compila a SPA. Validar ali a presença e a sintaxe de origem, com o mesmo formato já aceito pelo cliente: HTTP/HTTPS, sem credenciais, caminho além de `/`, query, fragmento ou espaços externos. Não testar disponibilidade por HTTP no build. Uma origem local válida continua permitida para smoke tests; o roteiro exige origem pública no Railway.

**Alternativas:** definir a raiz Railway como `apps/frontend` perderia o lockfile compartilhado; usar `vite preview` ou incluir Node no runtime não atende ao servidor estático solicitado. Não habilitar fallback silencioso para localhost na construção da imagem.

### 3. Nginx mínimo com template de porta e fallback restrito

Adicionar `apps/frontend/nginx/default.conf.template`, copiado para `/etc/nginx/templates/default.conf.template`. Manter o entrypoint oficial, que renderiza o template e inicia Nginx em foreground.

- `PORT` terá padrão `8080` na imagem; o template escuta nessa porta em todas as interfaces. Restringir a substituição a `PORT` com `NGINX_ENVSUBST_FILTER`, preservando variáveis nativas como `$uri`.
- A raiz estática será `/usr/share/nginx/html`. Rotas de aplicação usam fallback para `index.html`; `/assets/` e solicitações de arquivos estáticos usam existência real e retornam 404 quando ausentes.
- Preservar MIME de JavaScript, CSS, imagens e fontes. O documento `index.html` deve ser revalidado, sem cache permanente; não aplicar cache imutável indiscriminado aos assets públicos sem hash.
- O Nginx não faz chamadas de negócio, proxy da API ou terminação TLS própria. O Railway fornece HTTPS no acesso público e encaminha HTTP ao contêiner.

**Alternativa:** reescrever qualquer caminho ausente para HTML ocultaria erros de bundle/assets. A regra específica de estáticos evita esse problema sem alterar as rotas React.

### 4. Startup automático explícito, aguardado e sem reset

Ler `Database:InitializeOnStartup` como booleano pelo mecanismo padrão de configuração, aceitando `Database__InitializeOnStartup`; padrão `false` para qualquer ambiente. O roteiro Railway exige `true` na demo. Não ativar implicitamente só por estar em Production.

No startup:

1. Validar a conexão obrigatória, o booleano e a origem do frontend antes de acessar o banco. Como `ValidateOnStart` só atua quando o host inicia, resolver/validar explicitamente as options de origem antes do passo de migrations.
2. Depois de construir o host e antes de iniciar o atendimento HTTP, quando habilitado, criar um escopo assíncrono e aguardar `CatalogDbContext.Database.MigrateAsync`.
3. Deixar o callback `UseAsyncSeeding` existente executar o seed. Não chamar `CatalogSeed` uma segunda vez nem introduzir `EnsureCreated`, `EnsureDeleted`, drop ou reconstrução de tabelas como atalho.
4. Descartar o escopo e somente então iniciar o servidor. Registrar começo/conclusão da etapa; em falha, encerrar o startup com diagnóstico sanitizado, sem conexão completa ou senha. Não continuar servindo HTTP nem criar retentativas infinitas.

Não são necessárias migrations novas ou mudanças no conjunto inicial. A preparação manual por `dotnet ef database update` continua funcionando, inclusive sem frontend configurado/em execução, pois as options HTTP não serão acopladas ao contexto ou aos callbacks.

Para os testes, estender `CatalogApiFactory` com uma opção de inicialização explicitamente controlada por fábrica, sem alterar variáveis globais do processo. A fábrica existente continua no modo manual. Adicionar testes com banco descartável vazio, reinício com valores alterados/registros extras, flags ausente/false/true/inválida, origem inválida antes de qualquer DDL e falhas de conexão/migration/seed.

**Alternativa:** bundle/pré-deploy seria mais adequado para separação de responsabilidades em produção, mas o usuário escolheu startup para simplificar a demonstração. O opt-in mantém essa decisão visível e preserva o comportamento local/testes por padrão.

### 5. Porta da API adaptada pela imagem, não pelo perfil local

A imagem da API usará `PORT` com padrão `8080` e iniciará `dotnet Catalog.Api.dll` com URL de escuta em `0.0.0.0` e a porta expandida em runtime. Se houver shell para expansão, ele deverá transferir o processo com `exec`, preservando sinais/encerramento.

Definir o ambiente de runtime da imagem como `Production`; manter Swagger/OpenAPI somente em Development. Não modificar `launchSettings.json`, a porta local `5080` ou o cliente para isso. Nenhuma credencial de banco participa do build ou de argumentos da imagem.

**Alternativa:** interpolar `${PORT}` literalmente em um valor fixo de configuração .NET não garante expansão. Fazer a resolução no comando de runtime evita depender dessa suposição e não acopla a aplicação ao Railway.

### 6. Configuração Railway por arquivo, variáveis e recursos pelo painel

Adicionar dois arquivos com referência ao schema oficial e os campos abaixo:

| Configuração | API | Frontend |
| --- | --- | --- |
| Arquivo selecionado no painel | `/apps/backend/railway.json` | `/apps/frontend/railway.json` |
| Root Directory | `/` | `/` |
| `build.builder` | `DOCKERFILE` | `DOCKERFILE` |
| `build.dockerfilePath` | `apps/backend/Dockerfile` | `apps/frontend/Dockerfile` |
| `deploy.healthcheckPath` | `/api/courses` | `/` |
| `deploy.healthcheckTimeout` | `300` segundos | `60` segundos |
| `deploy.restartPolicyType` | `ON_FAILURE` | `ON_FAILURE` |
| `deploy.restartPolicyMaxRetries` | `3` | `3` |
| `deploy.numReplicas` | `1` | `1` |

Não sobrescrever `startCommand` ou configurar `preDeployCommand`: os comandos das imagens são suficientes. Os healthchecks são verificações de disponibilidade do deploy, não monitoramento contínuo. A rota de catálogo evita acrescentar endpoints e comprova acesso relacional; a API só começa a atendê-la após o seed.

Usar `build.watchPatterns` específicos. Para API: fontes/projeto em `apps/backend/src`, Dockerfile, arquivo Railway e `.dockerignore`. Para frontend: `src`, `public`, `index.html`, manifestos/configurações Vite/TypeScript/PostCSS, Dockerfile, template Nginx, arquivo Railway e os três manifestos/lockfile do workspace na raiz, além de `.dockerignore`. READMEs e `docs/` não precisam disparar builds.

Os arquivos Railway não criam serviços, volumes, domínios, secrets, associação de repositório ou a opção Wait for CI. Documentar essa limitação e as configurações manuais:

| Serviço | Variável | Fase / valor esperado |
| --- | --- | --- |
| API | `ConnectionStrings__DefaultConnection` | Runtime; string Npgsql composta com referências às variáveis privadas do PostgreSQL |
| API | `FRONTEND_URL` | Runtime; `https://<dominio-publico-frontend>` |
| API | `Database__InitializeOnStartup` | Runtime; `true` na demonstração |
| API | `ASPNETCORE_ENVIRONMENT` | Runtime; `Production`, também padrão da imagem |
| Frontend | `VITE_API_BASE_URL` | Build; `https://<dominio-publico-api>` |
| Ambos | `PORT` | Runtime; fornecida pelo Railway, padrão local da imagem `8080` |

Para um serviço chamado `Postgres`, usar uma referência como `Host=${{Postgres.PGHOST}};Port=${{Postgres.PGPORT}};Database=${{Postgres.PGDATABASE}};Username=${{Postgres.PGUSER}};Password=${{Postgres.PGPASSWORD}}`. O nome do serviço pode ser ajustado pelo operador. Não pressupor que a aplicação leia `DATABASE_URL` automaticamente nem persistir senhas nos arquivos.

**Alternativa:** um único arquivo Railway compartilhado ou seleção implícita do builder pode construir a aplicação errada. Os arquivos por serviço tornam a associação explícita sem introduzir uma ferramenta de provisionamento.

### 7. GitHub Actions como CI, integração Railway como futuro publicador

Adicionar `.github/workflows/ci.yml` com eventos `push` para branches, `pull_request` normal e `workflow_dispatch`, sem filtro de caminhos. Não pressupor uma branch `main`: o checkout atual usa `dev`, e qualquer branch escolhida futuramente pelo operador terá CI.

Usar runners Linux com Docker, permissões `contents: read`, actions mantidas para checkout/setup e três jobs obrigatórios:

- **frontend:** Node 24 e pnpm `11.9.0`, instalação congelada na raiz, `pnpm frontend:typecheck` e `pnpm frontend:build` com origem fictícia válida. O cache pnpm, se usado, é baseado no lockfile da raiz.
- **backend:** SDK .NET 10 e, em `apps/backend`, restauração da ferramenta local, restore, build Release e `dotnet test` da suíte completa. Testcontainers já gerencia PostgreSQL 17; não adicionar um banco externo ou serviço Compose concorrente. Não habilitar cache automático de setup-dotnet que exija `packages.lock.json`, inexistente no projeto.
- **containers:** executar o smoke script local das imagens, com timeouts finitos, sem login de registry e sem push.

Não adicionar jobs de deploy, tokens Railway, uso de `gh`, escrita em packages/repositório ou `pull_request_target`. Não dispensar falhas obrigatórias nem introduzir concorrência que cancele silenciosamente a única execução usada para liberar um deploy.

O roteiro orienta o operador a conectar futuramente o repositório/branch no Railway e habilitar Wait for CI. Essa opção pertence ao painel e não estará configurada só porque o YAML existe.

**Alternativa:** deploy por action com token duplicaria a responsabilidade da integração Railway e exigiria credenciais externas, sem benefício para esta preparação.

### 8. Smoke test reutilizável com recursos descartáveis

Adicionar `scripts/smoke-deployment.sh`, usado localmente e pelo job de contêineres. Pré-requisitos documentados: Bash, Docker disponível, curl e Python 3 para validar respostas; SDK .NET e Node no host não serão necessários para esse teste das imagens.

O script construirá as duas imagens com contexto na raiz e configuração fictícia, criará recursos Docker com nomes exclusivos da execução e aguardará PostgreSQL 17 ficar disponível. Nenhuma variável de conexão do ambiente do usuário será reutilizada.

Verificações mínimas:

- API em Production com inicialização habilitada, banco vazio e `PORT` diferente de `8080`: primeiro GET retorna os seis cursos; detalhe contém apresentação e módulos esperados; imagens apontam para a origem de frontend configurada.
- Reiniciar a API contra o mesmo PostgreSQL e comparar identificadores/contagens; provar que não foi necessário `dotnet ef` antes do primeiro GET.
- Nginx com `PORT` diferente do padrão: HTML para rotas e aliases, arquivos reais para imagens/fontes/bundles e 404 para assets ausentes; verificar que o bundle contém a origem de build e não é reconfigurado por uma variável tardia.
- Build frontend sem a origem ou com origem inválida deve falhar. Os testes de configuração/falha do startup também ficam cobertos pela suíte .NET.
- Em erro, exibir somente logs locais necessários e executar limpeza delimitada por identificadores criados pelo próprio script, inclusive via trap. Não usar `docker system prune`, remover recursos alheios ou tocar no banco/volume do Compose.

HTTP comprova o servidor/fallback, não a renderização React. Na implementação, complementar com verificação local no navegador das três telas, entrada direta/refresh, aliases e carregamento das imagens usando as imagens produzidas, sem instalar uma suíte frontend nova.

### 9. Roteiro operacional e explicação para a apresentação

Adicionar `docs/deploy-railway.md` e referenciá-lo na raiz e nos READMEs das aplicações. Atualizar os trechos que hoje afirmam incondicionalmente que o startup nunca migra: isso continua verdadeiro por padrão, mas não com a flag demonstrativa. Acrescentar a opção, desabilitada por padrão, ao exemplo do backend e explicar o endereço de build no exemplo frontend.

O roteiro distinguirá preparação local, configuração manual futura e verificações externas ainda não realizadas. Incluir os caminhos da tabela, variáveis, obtenção prévia dos dois domínios para evitar referências circulares, banco/volume privado, branch selecionável e Wait for CI. Não incluir instruções de criação de repositório ou uso de `gh`.

Para a apresentação, registrar em texto curto: migrations no startup são uma simplificação consciente para uma demo com uma réplica, não um padrão de produção. O processo recebe permissão de DDL, a disponibilidade depende da preparação e o rollback de código não reverte alterações do banco. Em produção, preferir etapa própria de migrations, credenciais separadas e revisão de compatibilidade/rollout.

## Risks / Trade-offs

- **Startup com privilégios de esquema:** o mesmo processo lê o catálogo e aplica migrations. Mitigação: opção explícita apenas habilitada na demo e documentação da alternativa com identidade/etapa separadas.
- **Banco indisponível ou migration incompatível:** a API não fica pronta. Mitigação: falhar antes de HTTP, healthcheck com janela de 300 segundos e três reinícios em falha; não mascarar o erro ou apagar dados.
- **Uma réplica não elimina sobreposição de deploys:** uma versão antiga ainda pode atender enquanto a nova migra. O lock do EF coordena migradores, não requisições de versões antigas. Mitigação: não adicionar migrations nesta change; revisar compatibilidade em evoluções futuras e aceitar parada coordenada na demo se necessária, sem promessa de zero downtime.
- **Banco legado com seed anterior:** migrations editoriais podem exigir transição específica. Mitigação: primeiro deploy em banco Railway novo; não transferir o banco local e não automatizar reset como recuperação.
- **URL embutida ou cache antigo:** trocar a origem da API requer novo build. Mitigação: validar build arg, revalidar HTML e explicar rebuild no roteiro.
- **CI e Railway ainda não conectados:** o preparo não comprova integração real ou publicação. Mitigação: relatar validações locais separadamente e deixar ações do painel/aceite externo explicitamente para o operador.

## Migration Plan

### Implementação local da preparação

1. Implementar o opt-in e os testes sem alterar migrations/seed existentes.
2. Adicionar empacotamento, Nginx, arquivos Railway, smoke script e CI, preservando o Compose local.
3. Executar a suíte .NET, tipos/build frontend e smoke tests das imagens; validar arquivos de configuração e testar a SPA local no navegador.
4. Atualizar documentação e relatar o que passou, falhou ou não foi executado. Não configurar remotos, fazer commit/push nem operar contas como parte dessa entrega.

### Operação posterior do responsável, fora da implementação desta change

1. Com o repositório já disponível por ação independente do operador, criar no painel Railway o projeto, PostgreSQL 17 persistente e os dois serviços de aplicação.
2. Reservar os dois domínios públicos, escolher a branch e preencher configurações/variáveis antes de liberar os builds. Selecionar cada arquivo Railway com Root Directory `/` e confirmar ausência de acesso público necessário ao banco.
3. Aguardar CI, habilitar Wait for CI e efetuar a primeira publicação pelo painel/integração. Não há job próprio de deploy.
4. Confirmar catálogo, detalhe, imagens, rotas/refresh, ausência de Swagger em Production e preservação do catálogo após reinício.

### Rollback

Reverter a imagem/commit implantado pode reverter código e configuração, mas não executa migrations `Down` nem restaura o banco. Para falhas de migração, interromper o rollout e avaliar o estado do PostgreSQL; não apagar/recriar o banco automaticamente. A flag pode ser desabilitada após preparação bem-sucedida, mas isso não corrige um esquema incompatível.

## Open Questions

Não há decisão de arquitetura pendente. Nome do projeto/serviços, branch de publicação, região e domínios são parâmetros operacionais do painel, preenchidos posteriormente pelo responsável; não bloqueiam a preparação dos arquivos nem autorizam acesso externo.

## References

Documentação oficial consultada em 2026-10-24:

- [Railway: monorepos](https://docs.railway.com/deployments/monorepo), [Dockerfiles e build args](https://docs.railway.com/builds/dockerfiles), [schema dos arquivos Railway](https://railway.com/railway.schema.json) e [GitHub autodeploys / Wait for CI](https://docs.railway.com/deployments/github-autodeploys).
- [EF Core: migrations em runtime e locking](https://learn.microsoft.com/ef/core/managing-schemas/migrations/applying#apply-migrations-at-runtime) e [callbacks de seed](https://learn.microsoft.com/ef/core/modeling/data-seeding).
- [Imagem oficial Nginx: templates e envsubst](https://github.com/nginx/docker-nginx/blob/master/entrypoint/20-envsubst-on-templates.sh) e [Nginx: try_files](https://nginx.org/en/docs/http/ngx_http_core_module.html#try_files).
- [GitHub Actions: builds .NET](https://docs.github.com/en/actions/tutorials/build-and-test-code/net) e [cache de dependências](https://docs.github.com/en/actions/reference/workflows-and-actions/dependency-caching).
