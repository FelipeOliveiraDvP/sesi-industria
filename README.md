# Catálogo de cursos educacionais

Monorepo com backend somente leitura em [apps/backend](apps/backend) e frontend demonstrativo opcional em [apps/frontend](apps/frontend). A API roda no terminal do host; apenas o PostgreSQL roda no Docker. O frontend não depende da API, do banco ou do Docker.

- [Requisitos funcionais](docs/prd.md)
- [Desenvolvimento do backend](apps/backend/README.md)
- [Desenvolvimento do frontend](apps/frontend/README.md)

## Execução independente do frontend

Pré-requisitos: Node.js `>=22.13` e pnpm `11.9.0`. A instalação e os comandos abaixo são executados na raiz e não iniciam a API, o PostgreSQL nem o Docker:

```sh
pnpm install
pnpm frontend:dev --host 127.0.0.1
```

Para verificar e visualizar o build:

```sh
pnpm frontend:typecheck
pnpm frontend:build
pnpm frontend:preview --host 127.0.0.1
```

As rotas, os limites da demonstração e as instruções para hospedagem estática estão em [apps/frontend/README.md](apps/frontend/README.md).

## Pré-requisitos

- .NET SDK 10.
- Docker com o daemon em execução e Docker Compose (`docker compose`).
- Git.

## Execução local

### 1. Clonar e entrar na raiz

Substitua `URL_DO_REPOSITORIO` pela URL do seu repositório. Os comandos funcionam em Bash e PowerShell:

```sh
git clone URL_DO_REPOSITORIO sesi-industria
cd sesi-industria
```

### 2. Iniciar o PostgreSQL e verificar a saúde

Execute na raiz do monorepo:

```sh
docker compose up -d
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
```

PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
```

Mantenha esse terminal aberto para os comandos .NET. Se sobrescreveu a configuração do PostgreSQL, use os mesmos valores na conexão.

`apps/backend/.env.example` serve somente como referência. A API não carrega arquivos `.env` automaticamente: as variáveis precisam estar disponíveis no ambiente do processo. A chave lida pelo ASP.NET Core é `ConnectionStrings:DefaultConnection`. Não versione arquivos com credenciais locais reais.

### 4. Restaurar e aplicar as migrations

Da raiz, entre no backend e execute:

```sh
cd apps/backend
dotnet tool restore
dotnet restore
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

O repositório inclui a migration `InitialCreate`. Quem clona aplica as migrations existentes; não precisa gerar outra migration inicial. A criação de migrations durante desenvolvimento está no [README do backend](apps/backend/README.md#migrations).

A atualização também prepara o seed fictício: seis cursos, três categorias, duas modalidades e três status, com módulos, aulas e imagens locais. O seed é determinístico e idempotente: repetir o comando não duplica dados nem sobrescreve registros existentes. Aplique-o antes de iniciar a API; o startup HTTP não modifica o esquema ou carrega dados automaticamente.

### 5. Executar a API e acessar o Swagger

No mesmo terminal, ainda em `apps/backend`:

```sh
dotnet run --project src/Catalog.Api --launch-profile http
```

A aplicação informa o endereço HTTP no terminal. O perfil `http` usa `http://localhost:5080` e ambiente Development, sem certificado HTTPS ou IDE. Abra manualmente:

- Swagger: <http://localhost:5080/swagger>
- Documento OpenAPI: <http://localhost:5080/openapi/v1.json>

Se alterar o endereço de execução, utilize o endereço informado pela aplicação acrescido de `/swagger`. A documentação HTTP fica habilitada somente em Development. Mantenha a API em execução para utilizar os exemplos abaixo.

## Configuração do PostgreSQL

| Variável | Padrão de desenvolvimento |
| --- | --- |
| `POSTGRES_DB` | `catalog_db` |
| `POSTGRES_USER` | `catalog_user` |
| `POSTGRES_PASSWORD` | `catalog_dev_password` |
| `POSTGRES_PORT` | `5432` |

Os valores são fictícios, exclusivos de desenvolvimento. A porta é publicada somente em `127.0.0.1`. Defina as variáveis no terminal antes do `docker compose up -d` para sobrescrever os padrões. Por exemplo, para outra porta:

Bash:

```bash
export POSTGRES_PORT=55432
```

PowerShell:

```powershell
$env:POSTGRES_PORT = '55432'
```

Atualize também a conexão usada pela API se alterar banco, porta ou credenciais. Os valores de banco, usuário e senha inicializam um volume vazio; alterá-los no ambiente não modifica os usuários de um banco já existente.

## Consultar a API

Com a API executando em `http://localhost:5080`:

| Método e rota | Resultado |
| --- | --- |
| `GET /api/courses` | Coleção de cursos com classificações e quantidade de resultados |
| `GET /api/courses/{id}` | Informações do curso, módulos e aulas ordenados |

A listagem aceita `search`, `categoryId`, `modalityId` e `statusId`, todos opcionais e combináveis. A pesquisa considera os nomes completo e breve; os filtros são cumulativos. A resposta contém `items` e `totalCount`, sem paginação. Sem correspondências, retorna `{"items":[],"totalCount":0}` com HTTP 200.

Bash:

```bash
curl --fail 'http://localhost:5080/api/courses'
curl --fail 'http://localhost:5080/api/courses?search=TecEdu&categoryId=1&modalityId=1&statusId=1'
curl --fail 'http://localhost:5080/api/courses/1'
```

PowerShell:

```powershell
Invoke-RestMethod -Uri 'http://localhost:5080/api/courses'
Invoke-RestMethod -Uri 'http://localhost:5080/api/courses?search=TecEdu&categoryId=1&modalityId=1&statusId=1'
Invoke-RestMethod -Uri 'http://localhost:5080/api/courses/1'
```

O seed permite consultar o curso `1` pelos exemplos acima. Não há operações de escrita. Veja os [contratos e parâmetros no README do backend](apps/backend/README.md#endpoints).

## Parar os contêineres

Encerre a API com `Ctrl+C` no terminal em que ela está executando. Se estiver em `apps/backend`, volte à raiz antes de operar o Compose:

```sh
cd ../..
docker compose stop
# Ou remover os contêineres e a rede, preservando os dados:
docker compose down
```

O volume nomeado mantém os dados entre execuções. Não utilize `docker compose down -v` se quiser preservá-los.
