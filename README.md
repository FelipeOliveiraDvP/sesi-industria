# Catálogo de cursos educacionais

Monorepo com API somente leitura em [apps/backend](apps/backend), interface demonstrativa em [apps/frontend](apps/frontend) e PostgreSQL. A aplicação pode ser executada individualmente durante o desenvolvimento ou como stack completa com Docker Compose. Os dados específicos de curso nas telas vêm da API; quando ela está indisponível, o frontend apresenta estados recuperáveis.

- [Requisitos funcionais](docs/prd.md)
- [Repositório no GitHub](https://github.com/FelipeOliveiraDvP/sesi-industria) (`main`)
- [Desenvolvimento do backend](apps/backend/README.md)
- [Desenvolvimento do frontend](apps/frontend/README.md)
- [Publicação demonstrativa no Railway](docs/deploy-railway.md)
- [Registro de uso de IA](USO_DE_IA.md)

## Objetivo da aplicação

Apresentar um catálogo demonstrativo de cursos fictícios para consulta, pesquisa pelo nome, combinação de filtros e visualização de detalhes. A API é somente leitura. O projeto não integra com Moodle e não implementa cadastro, edição ou exclusão de cursos, matrícula ou progresso real de aprendizagem.

## Tecnologias e arquitetura

- **Frontend:** React, TypeScript, Vite, Mantine e React Router.
- **Backend:** C#, ASP.NET Core/.NET 10, Entity Framework Core e Npgsql.
- **Banco:** PostgreSQL 17.
- **Execução e publicação:** Docker Compose, imagens Docker e roteiro de publicação no Railway.

```text
Navegador --> Frontend React (porta 8081 no Compose; 5173 no Vite)
    |              |-- assets estáticos e imagens
    |              +-- GET /api/courses e GET /api/courses/{id}
    |                                  |
    +--------------------------------> API ASP.NET Core (porta 5080)
                                           |
                                           +--> PostgreSQL 17
```

No uso local individual, Vite roda no host e consome a API em `http://localhost:5080`. No Compose, o navegador acessa os mesmos endereços locais publicados; dentro da rede Docker, a API conecta ao serviço `postgres`. As imagens dos cursos são assets estáticos do frontend.

## Estrutura do repositório

```text
apps/backend/       API, entidades, migrations, seed e testes
apps/frontend/      SPA React, assets, configuração Vite e telas
docs/               PRD, instruções da atividade e publicação
openspec/           especificações e mudanças do projeto
scripts/            scripts de validação e apoio
docker-compose.yml  PostgreSQL, API e frontend para execução local
```

## Modelo de dados

O catálogo contém dados fictícios: seis cursos, três categorias, duas modalidades e três status. `Course` guarda nome completo e breve, descrições, carga horária, público-alvo, imagem e data opcional de disponibilização. Cada curso referencia uma categoria, modalidade e status; possui módulos e aulas ordenados. O conteúdo editorial tipado de Detalhes/AVA é persistido em `PresentationJson` como `jsonb`. As tabelas de categoria, modalidade e status têm relação um-para-muitos com cursos. O seed é determinístico e idempotente; consulte [os detalhes do seed e das migrations](apps/backend/README.md#migrations).

## Responsividade, acessibilidade e limitações

O frontend adapta as telas para desktop, tablet e celular; rails horizontais e controles do AVA têm interação apropriada às telas menores. A validação visual registrada cobre larguras de 1440, 1024, 768 e 375 px. A interface utiliza estrutura semântica, rótulos e estados anunciados por tecnologias assistivas; controles interativos suportam teclado e animações não essenciais respeitam a preferência por movimento reduzido. Mais detalhes e diferenças visuais em relação à referência estão em [apps/frontend/README.md](apps/frontend/README.md).

Limitações conhecidas: não há integração com Moodle, autenticação, matrícula real, player de mídia, cadastro/edição/exclusão de cursos ou progresso persistido. O progresso exibido no AVA é demonstrativo; conteúdo institucional e depoimentos também são estáticos. As diferenças conhecidas em relação ao protótipo e funcionalidades fora do escopo estão descritas no README do frontend.

## Publicação e itens de entrega

O projeto está publicado no Railway e o código-fonte está no [repositório GitHub](https://github.com/FelipeOliveiraDvP/sesi-industria), com `main` como branch principal.

- **Aplicação:** [abrir o catálogo](https://sesifrontend-production.up.railway.app/)
- **API:** [listar cursos](https://sesi-api-production.up.railway.app/api/courses) · [detalhes do curso 1](https://sesi-api-production.up.railway.app/api/courses/1)
- **Publicação e configuração:** [guia do Railway](docs/deploy-railway.md)

O frontend e a API são serviços separados; o PostgreSQL é acessado pela API na rede privada. O [registro de uso de IA](USO_DE_IA.md) descreve as ferramentas, os objetivos, os pedidos em resumo, a validação e uma lacuna identificada durante a integração.

A workflow de CI executa no GitHub Actions. A opção Railway **Wait for CI** ainda precisa ser habilitada e confirmada nos dois serviços; consulte o [estado e as instruções no guia](docs/deploy-railway.md#4-ci-e-publicação).

## Requisitos mínimos de desenvolvimento

| Parte | Requisitos |
| --- | --- |
| Comum | Git e acesso à raiz do monorepo. |
| Backend individual | SDK .NET 10; Docker Engine e Docker Compose v2 para executar o PostgreSQL 17 local. |
| Frontend individual | Node.js `>=22.13` e pnpm `11.9.0` (fixado no workspace). Não exige Docker para instalar, tipar ou compilar; a API é necessária para carregar dados dos cursos. |
| Stack completa | Docker com daemon ativo e plugin `docker compose`; conexão à internet na primeira compilação para baixar imagens e dependências. |

As portas locais padrão são 5432 (PostgreSQL), 5080 (API), 5173 (Vite), 4173 (preview do Vite) e 8081 (frontend no Compose). Verifique se estão disponíveis. Não há requisito mínimo de CPU ou memória medido/documentado neste projeto; por isso, não é especificado aqui.

## Executar a aplicação completa com Docker Compose

Na raiz, crie o arquivo local `.env` a partir do modelo. Ele fornece as configurações interpoladas pelo Compose e é ignorado pelo Git; revise os valores e não coloque credenciais reais neste ambiente demonstrativo.

Bash:

```bash
[ -f .env ] || cp .env.example .env
docker compose up -d
docker compose ps
docker compose logs --tail=100 api
```

PowerShell:

```powershell
if (-not (Test-Path .env)) { Copy-Item .env.example .env }
docker compose up -d
docker compose ps
docker compose logs --tail=100 api
```

O Compose constrói e inicia PostgreSQL, API e frontend. Aguarde os logs da API indicarem que a aplicação iniciou antes de abrir a interface. No perfil Compose, `Database__InitializeOnStartup=true` faz a API aplicar as migrations pendentes e executar o seed idempotente antes de atender requisições; não é necessário executar `dotnet ef` separadamente nesse fluxo.

Se alterar `API_PORT`, atualize `VITE_API_BASE_URL` para a mesma porta e reconstrua o frontend. Se alterar `FRONTEND_PORT`, atualize `FRONTEND_URL` para refletir a origem local correta das imagens. Os dois endereços devem ser origens HTTP sem caminho adicional.

- Frontend: <http://localhost:8081>
- API: <http://localhost:5080>
- Swagger UI: <http://localhost:5080/swagger>
- OpenAPI: <http://localhost:5080/openapi/v1.json>

Para verificar o catálogo, use `curl --fail http://localhost:5080/api/courses` ou `Invoke-RestMethod -Uri 'http://localhost:5080/api/courses'`. Para acompanhar a inicialização, use `docker compose logs -f api` (Ctrl+C encerra somente a visualização dos logs). Após alterar o código ou `VITE_API_BASE_URL`, reconstrua os serviços com `docker compose up -d --build`; o endereço da API do frontend é incorporado no build.

Para encerrar a stack e manter o banco, execute `docker compose down`. Isso remove contêineres e rede, mas preserva o volume `catalog_postgres_data`; **não** use `docker compose down -v` se quiser manter os dados.

## Desenvolvimento e preview do frontend

Pré-requisitos: Node.js `>=22.13` e pnpm `11.9.0`. Configure `VITE_API_BASE_URL` para a origem HTTP da API (padrão local: `http://localhost:5080`). É uma variável pública incorporada ao bundle, nunca um local para credenciais.

Desenvolvimento em Bash, a partir da raiz:

```bash
[ -f apps/frontend/.env.local ] || cp apps/frontend/.env.example apps/frontend/.env.local
pnpm install
export VITE_API_BASE_URL='http://localhost:5080'
pnpm frontend:dev --host 127.0.0.1 --port 5173 --strictPort
```

PowerShell:

```powershell
if (-not (Test-Path apps/frontend/.env.local)) { Copy-Item apps/frontend/.env.example apps/frontend/.env.local }
pnpm install
$env:VITE_API_BASE_URL = 'http://localhost:5080'
pnpm frontend:dev --host 127.0.0.1 --port 5173 --strictPort
```

Para verificar e visualizar o build, defina `VITE_API_BASE_URL` antes do build. A configuração fica embutida no bundle; mudar o endereço exige compilar novamente.

```bash
export VITE_API_BASE_URL='http://localhost:5080'
pnpm frontend:typecheck
pnpm frontend:build
pnpm frontend:preview --host 127.0.0.1 --port 4173 --strictPort
```

```powershell
$env:VITE_API_BASE_URL = 'http://localhost:5080'
pnpm frontend:typecheck
pnpm frontend:build
pnpm frontend:preview --host 127.0.0.1 --port 4173 --strictPort
```

O backend usa `FRONTEND_URL` para compor as URLs das imagens. Para desenvolvimento, configure `FRONTEND_URL=http://127.0.0.1:5173`; para preview, use `FRONTEND_URL=http://127.0.0.1:4173` e reinicie a API. O CORS permite qualquer origem para GET, sem credenciais; métodos de escrita continuam indisponíveis. Os comandos fixam host e porta e encerram com erro se a porta estiver ocupada. O backend não carrega `.env` automaticamente; veja os exemplos Bash e PowerShell abaixo e no [README do backend](apps/backend/README.md#origem-do-frontend).

Para trocar a API da origem de desenvolvimento para a de preview antes de reiniciá-la:

```bash
export FRONTEND_URL='http://127.0.0.1:4173'
```

```powershell
$env:FRONTEND_URL = 'http://127.0.0.1:4173'
```

As rotas, os limites da demonstração e as instruções para hospedagem estática estão em [apps/frontend/README.md](apps/frontend/README.md).

## Execução manual do backend

Para iniciar somente o banco em Docker e executar API/frontend no host, use as instruções abaixo. Os requisitos por projeto estão na tabela acima.

## Execução local

### 1. Clonar e entrar na raiz

Clone o repositório oficial por SSH (é necessário ter uma chave SSH cadastrada no GitHub). Os comandos funcionam em Bash e PowerShell:

```sh
git clone git@github.com:FelipeOliveiraDvP/sesi-industria.git sesi-industria
cd sesi-industria
```

### 2. Iniciar o PostgreSQL e verificar a saúde

Execute na raiz do monorepo:

```sh
docker compose up -d postgres
docker compose ps
docker compose exec postgres pg_isready -U catalog_user -d catalog_db
```

Aguarde `healthy` no resultado de `docker compose ps` e `accepting connections` no `pg_isready` antes de aplicar migrations. Para investigar uma falha de inicialização:

```sh
docker compose logs postgres
```

### 3. Configurar a conexão no ambiente do processo

Bash:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
export FRONTEND_URL='http://127.0.0.1:5173'
```

Como alternativa no Bash, crie o `.env` local do backend uma vez e carregue-o explicitamente. O ASP.NET Core não lê esse arquivo sozinho:

```bash
[ -f apps/backend/.env ] || cp apps/backend/.env.example apps/backend/.env
set -a
. ./apps/backend/.env
set +a
```

No PowerShell, crie o arquivo de referência se necessário, mas configure as variáveis no processo conforme abaixo; o arquivo `.env` não é carregado automaticamente pelo .NET:

```powershell
if (-not (Test-Path apps/backend/.env)) { Copy-Item apps/backend/.env.example apps/backend/.env }
$env:ConnectionStrings__DefaultConnection = 'Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
$env:FRONTEND_URL = 'http://127.0.0.1:5173'
```

Mantenha esse terminal aberto para os comandos .NET. Se sobrescreveu a configuração do PostgreSQL, use os mesmos valores na conexão. `FRONTEND_URL` deve apontar para o frontend que serve as imagens; para usar o preview em `127.0.0.1:4173`, troque a variável e reinicie a API. O CORS não depende dessa origem.

`apps/backend/.env.example` é o modelo para criar `apps/backend/.env`. A API não carrega `.env` automaticamente: no Bash, carregue o arquivo explicitamente como mostrado; no PowerShell, defina as variáveis no processo. `FRONTEND_URL` é a base das URLs de imagem; o CORS permite leitura de qualquer origem sem credenciais. Não versione arquivos com credenciais locais reais.

### 4. Restaurar e aplicar as migrations

Da raiz, entre no backend e execute:

```sh
cd apps/backend
dotnet tool restore
dotnet restore
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

O repositório inclui a migration `InitialCreate`. Quem clona aplica as migrations existentes; não precisa gerar outra migration inicial. A criação de migrations durante desenvolvimento está no [README do backend](apps/backend/README.md#migrations).

A atualização também prepara o seed fictício: seis cursos, três categorias (Linguagens permanece sem cursos), duas modalidades e três status, com módulos e aulas relacionais específicos para Detalhes e uma apresentação demonstrativa para Detalhes/AVA. A sidebar do AVA recebe os mesmos seis módulos genéricos e duas aulas de texto por módulo para todos os cursos; essa estrutura é expansível, não navega e não registra progresso. A migration `AddGenericAvaLessons` atualiza essa seção JSONB nos seis cursos já semeados. As imagens são servidas pelos assets locais do frontend; o seed armazena apenas caminhos `/assets/`. O seed é determinístico e idempotente: repetir o comando não duplica dados nem sobrescreve outros registros existentes. Aplique-o antes de iniciar a API no fluxo local padrão. Na demonstração Railway, `Database__InitializeOnStartup=true` habilita a preparação aguardada no startup, sem reset do banco; veja o [roteiro de publicação](docs/deploy-railway.md).

### 5. Executar a API e acessar o Swagger

No mesmo terminal, ainda em `apps/backend`:

```sh
dotnet run --project src/Catalog.Api --launch-profile http
```

A aplicação informa o endereço HTTP no terminal. O perfil `http` usa `http://localhost:5080` e ambiente Development, sem certificado HTTPS ou IDE. Abra manualmente:

- Swagger: <http://localhost:5080/swagger>
- Documento OpenAPI: <http://localhost:5080/openapi/v1.json>

Se alterar o endereço de execução, utilize o endereço informado pela aplicação acrescido de `/swagger`. A documentação HTTP fica habilitada somente em Development. Mantenha a API em execução para utilizar os exemplos abaixo.

Se já houver uma instância antiga de `Catalog.Api` ocupando a porta 5080, encerre e inicie novamente essa instância depois de aplicar migrations ou recriar o banco. Um processo HTTP já iniciado não recarrega a conexão nem o esquema automaticamente.

## Configuração do PostgreSQL

| Variável | Padrão de desenvolvimento |
| --- | --- |
| `POSTGRES_DB` | `catalog_db` |
| `POSTGRES_USER` | `catalog_user` |
| `POSTGRES_PASSWORD` | `catalog_dev_password` |
| `POSTGRES_PORT` | `5432` |

Os valores são fictícios, exclusivos de desenvolvimento. A porta é publicada somente em `127.0.0.1`. Defina as variáveis no `.env` da raiz ou no terminal antes de executar o Compose para sobrescrever os padrões. Por exemplo, para outra porta:

Bash:

```bash
export POSTGRES_PORT=55432
docker compose up -d postgres
```

PowerShell:

```powershell
$env:POSTGRES_PORT = '55432'
docker compose up -d postgres
```

Atualize também a conexão usada pela API se alterar banco, porta ou credenciais. Os valores de banco, usuário e senha inicializam um volume vazio; alterá-los no ambiente não modifica os usuários de um banco já existente.

## Consultar a API

Com a API executando em `http://localhost:5080`:

| Método e rota | Resultado |
| --- | --- |
| `GET /api/courses` | Cursos com descrições completa e breve, classificações, URLs de imagem e quantidade de resultados |
| `GET /api/courses/{id}` | Campos do resumo, conteúdo editorial tipado de Detalhes/AVA, módulos e aulas ordenados |

A listagem aceita `search`, `categoryId`, `modalityId` e `statusId`, todos opcionais e combináveis. A pesquisa considera os nomes completo e breve (não as descrições); os filtros são cumulativos. Os itens incluem descrições completa e breve, classificações e URLs absolutas de imagem baseadas em `FRONTEND_URL`. A resposta contém `items` e `totalCount`, sem paginação. Sem correspondências, retorna `{"items":[],"totalCount":0}` com HTTP 200.

O detalhe acrescenta `shortDescription` e `presentation`, com certificação, `detailsImageUrl`, informações de aulas/avaliações, FAQs e a prévia demonstrativa do AVA. As imagens usam os PNGs em `apps/frontend/public/assets`, servidos pela aplicação frontend. Navegadores podem ler os dois endpoints de qualquer origem; a política não habilita cookies, credenciais ou operações de escrita.

Bash:

```bash
curl --fail 'http://localhost:5080/api/courses'
curl --fail 'http://localhost:5080/api/courses?search=Metrologia&categoryId=1&modalityId=1&statusId=1'
curl --fail 'http://localhost:5080/api/courses/1'
```

PowerShell:

```powershell
Invoke-RestMethod -Uri 'http://localhost:5080/api/courses'
Invoke-RestMethod -Uri 'http://localhost:5080/api/courses?search=Metrologia&categoryId=1&modalityId=1&statusId=1'
Invoke-RestMethod -Uri 'http://localhost:5080/api/courses/1'
```

O seed permite consultar o curso `1` pelos exemplos acima. Não há operações de escrita. Veja os [contratos e parâmetros no README do backend](apps/backend/README.md#endpoints).

## Parar a aplicação e os contêineres

Para parar a stack completa iniciada pelo Compose, execute na raiz:

```sh
docker compose stop
# Ou remover contêineres e rede, preservando os dados:
docker compose down
```

O volume nomeado mantém os dados entre execuções. Não utilize `docker compose down -v` se quiser preservá-los. No modo individual, encerre os servidores do frontend e da API com `Ctrl+C` nos respectivos terminais; o PostgreSQL pode ser parado separadamente com `docker compose stop postgres`.

## Verificação da integração (2026-10-24)

- Backend: `dotnet tool restore`, `dotnet restore`, `dotnet ef database update` (repetido para idempotência), `dotnet build` e `dotnet test` foram executados em `apps/backend`. Build sem warnings/erros; 182 testes passaram após a mudança de CORS. A atualização preparou seis cursos, três categorias e seis apresentações JSONB.
- Frontend: `pnpm frontend:typecheck` e `pnpm frontend:build` passaram. O build emitiu somente o aviso de tamanho do bundle JavaScript (>500 kB após minificação); o projeto não configura um comando de lint frontend.
- Navegador: os seis ciclos Landing/Detalhes/AVA, entrada direta, refresh, aliases e histórico passaram em desenvolvimento e preview. Foram observadas somente chamadas GET de catálogo, CORS wildcard sem credenciais em origens `localhost` e `127.0.0.1`, assets locais HTTP 200/304, acordeões e footer alcançáveis, e ausência de overflow em 1440, 1024, 768 e 375 px. Atraso, falha recuperável e atualização em cache também foram exercitados; o 503 simulado gerou apenas os avisos de recurso HTTP esperados, sem exceções JavaScript.
- O banco confirmado foi `localhost:5432/catalog_db`. Após autorização, somente esse banco foi recriado para substituir o seed educacional antigo; o container e o volume do Compose foram preservados. A instância antiga da API em 5080 respondeu 500 logo após a recriação; para o aceite inicial foram usadas as portas 5082/5083. Em 2026-10-24, os três processos da API foram reiniciados com o build atualizado e passaram a responder 200 com seis cursos. O `.env.local` ignorado do frontend ainda aponta para 5082; o bundle de preview foi validado com 5083.

Não foram adicionados endpoints de escrita, busca/filtros visuais, catálogo extra, autenticação, matrícula, player ou progresso persistido. A verificação de navegador foi feita em Chromium; outros motores e implantação em hospedagem externa não foram avaliados.
