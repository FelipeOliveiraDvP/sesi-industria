# Catalog.Api — desenvolvimento do backend

Backend somente leitura do catálogo educacional. Para preparar o ambiente local desde o clone, consulte o [README da raiz](../../README.md). Os requisitos funcionais estão em [docs/prd.md](../../docs/prd.md).

## Tecnologias

- C# / ASP.NET Core e SDK .NET 10.
- Entity Framework Core 10 e provedor Npgsql 10.
- PostgreSQL 17.
- OpenAPI nativo e Swagger UI.
- xUnit, WebApplicationFactory e Testcontainers para integração com PostgreSQL real.

## Estrutura

```text
Catalog.slnx
.config/dotnet-tools.json
src/Catalog.Api/
  Entities/          # Entidades e navegações
  Configurations/    # Mapeamentos relacionais
  Data/
    CatalogDbContext.cs
    Migrations/      # Migration inicial e snapshot
    Seed/            # Definições fictícias e inicialização idempotente
  Controllers/       # Rotas HTTP somente leitura
  DTOs/              # Contratos HTTP
  Services/          # Consultas do catálogo
  Properties/        # Perfil HTTP de desenvolvimento
  wwwroot/           # Imagens locais dos cursos
  Program.cs
tests/Catalog.Api.Tests/
  Unit/
  Integration/
```

## Restaurar e compilar

Execute os comandos deste documento em `apps/backend`:

```sh
dotnet tool restore
dotnet restore
dotnet build
```

A solução contém apenas a API e seus testes. Não requer frontend ou IDE. A ferramenta `dotnet-ef` é local e tem sua versão fixada no manifesto; não é necessária instalação global.

## Conexão

O PostgreSQL deve estar saudável antes de atualizar o banco. A API usa `ConnectionStrings:DefaultConnection` e o host HTTP exige `FRONTEND_URL`. Defina ambas as variáveis no terminal em que executará os comandos .NET.

O modelo `.env.example` pode ser copiado para `.env` nesta pasta. O ASP.NET Core não carrega esse arquivo automaticamente. Execute os comandos abaixo em `apps/backend`.

```bash
[ -f .env ] || cp .env.example .env
set -a
. ./.env
set +a
```

No PowerShell, crie o arquivo como referência e defina as variáveis no processo usando os comandos a seguir. Não versione `.env` nem coloque credenciais reais nele.

Bash:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
export FRONTEND_URL='http://127.0.0.1:5173'
```

PowerShell:

```powershell
if (-not (Test-Path .env)) { Copy-Item .env.example .env }
$env:ConnectionStrings__DefaultConnection = 'Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
$env:FRONTEND_URL = 'http://127.0.0.1:5173'
```

Esses valores correspondem aos padrões locais do Compose. Ajuste-os se tiver sobrescrito banco, porta ou credenciais. Copiar `.env.example` não faz com que o ASP.NET Core carregue o arquivo: as variáveis precisam estar no ambiente do processo. Não versione configurações locais com credenciais reais.

## Origem do frontend

O host HTTP exige `FRONTEND_URL` como origem absoluta HTTP ou HTTPS para compor as URLs de imagem. Desenvolvimento usa `http://127.0.0.1:5173`; preview usa `http://127.0.0.1:4173`. A barra final é opcional. Ao trocar o host ou a porta do frontend, altere a variável e reinicie a API para que as imagens apontem ao endereço correto. `localhost` e `127.0.0.1` são origens distintas.

O CORS permite requisições de leitura (`GET`) de qualquer origem, sem cookies ou credenciais; métodos de escrita continuam indisponíveis. CORS não é autenticação: as respostas do catálogo são públicas. `FRONTEND_URL` não precisa de servidor frontend ativo durante migrations, seed ou testes de persistência, mas é obrigatória para iniciar o host HTTP. O cliente frontend usa `VITE_API_BASE_URL` para o endereço público da API; essa variável é independente de `FRONTEND_URL`.

Para iniciar a API com a origem do preview, exporte a variável antes de executar `dotnet run`:

```bash
export FRONTEND_URL='http://127.0.0.1:4173'
dotnet run --project src/Catalog.Api --launch-profile http
```

```powershell
$env:FRONTEND_URL = 'http://127.0.0.1:4173'
dotnet run --project src/Catalog.Api --launch-profile http
```

## Migrations

Com a conexão configurada, execute em `apps/backend`:

```sh
dotnet tool restore
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

`InitialCreate` e seu snapshot já estão versionados. Não crie outra migration inicial ao clonar. Por padrão, a API não aplica migrations no startup.

A migration incremental `20261023202554_AddCoursePresentationFields` adiciona `ShortDescription` e `PresentationJson` às linhas de curso. `PresentationJson` é `jsonb` obrigatório. As colunas não recebem valores padrão artificiais; por isso, não aplique essa transição diretamente a um banco local populado pelo seed anterior.

Somente depois de alterar o modelo, crie uma nova migration, substituindo `NomeDaMigration` por um nome descritivo:

```sh
dotnet ef migrations add NomeDaMigration --project src/Catalog.Api --startup-project src/Catalog.Api --output-dir Data/Migrations
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

Revise o código gerado antes de atualizar um banco que contenha dados. `--output-dir` é relativo ao projeto da API; os arquivos ficam em `src/Catalog.Api/Data/Migrations`.

### Inicialização automática para a demonstração

O padrão local continua sendo preparação manual: com `Database__InitializeOnStartup` ausente ou `false`, a API não cria o esquema nem executa o seed ao iniciar. Use `dotnet ef database update` conforme as instruções acima. `.env.example` é somente referência e não habilita nem carrega essa opção automaticamente.

Para a demonstração no Railway, configure `Database__InitializeOnStartup=true` como variável de runtime do serviço da API. Antes de iniciar HTTP, a API aguarda a aplicação das migrations pendentes e o callback idempotente de seed já registrado no contexto. Não é necessário executar a ferramenta EF no contêiner nem copiar o banco local. Cada inicialização habilitada verifica novamente as migrations; o seed insere apenas os IDs ausentes e não sobrescreve registros existentes.

Essa opção exige que a conexão configurada tenha permissões para alterar o esquema PostgreSQL. Se o banco estiver indisponível ou ocorrer falha na migration ou no seed, a API encerra o startup em vez de atender parcialmente. A aplicação não apaga, recria ou restaura o banco como recuperação. A configuração padrão permanece `false` em todos os ambientes; habilite-a somente no ambiente demonstrativo descartável. Para produção, prefira uma etapa de migrations separada e uma identidade com permissões de esquema próprias. Consulte o [roteiro do Railway](../../docs/deploy-railway.md) para as limitações e os demais parâmetros da demonstração.

### Transição explícita do banco local

O seed industrial substitui os cursos fictícios anteriores, mas continua inserindo apenas IDs ausentes: ele não renomeia, atualiza nem faz upsert dos IDs antigos. Para usar o novo catálogo, confirme primeiro que `ConnectionStrings__DefaultConnection` aponta exclusivamente para o PostgreSQL local de desenvolvimento (`Host=localhost`, banco `catalog_db`). A recriação elimina os dados desse banco; não a execute contra banco compartilhado, externo ou de produção.

No Bash, a partir de `apps/backend`, depois de configurar a variável de conexão:

```bash
dotnet ef database drop --project src/Catalog.Api --startup-project src/Catalog.Api --force
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

No PowerShell, no mesmo diretório e após configurar `$env:ConnectionStrings__DefaultConnection`:

```powershell
dotnet ef database drop --project src/Catalog.Api --startup-project src/Catalog.Api --force
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

Esses comandos recriam somente o banco indicado pela conexão. Eles não removem contêineres nem volumes do Docker Compose; não use `docker compose down -v` para esta transição. A sequência versionada aplica `InitialCreate` e as migrations incrementais antes de executar o seed.

## Dados iniciais

`dotnet ef database update` também executa o seed, inclusive quando nenhuma migration nova precisa ser aplicada. Os IDs são estáveis e o catálogo contém seis cursos:

| ID | Nome breve | Categoria | Modalidade | Status | Horas | Disponibilização |
| --- | --- | --- | --- | ---: | ---: | --- |
| 1 | Metrologia | Matemática | Online | Disponível | 40 | 05/01/2026 |
| 2 | GeoTêxtil | Matemática | Presencial | Disponível | 32 | A definir |
| 3 | QualiDados | Matemática | Online | Em breve | 24 | 10/02/2027 |
| 4 | Física Industrial | Ciências da Natureza | Presencial | Encerrado | 36 | 20/08/2025 |
| 5 | Química Têxtil | Ciências da Natureza | Online | Em breve | 20 | 05/03/2027 |
| 6 | Materiais Industriais | Ciências da Natureza | Presencial | Encerrado | 30 | A definir |

As categorias são Matemática (ID 1), Ciências da Natureza (ID 2) e Linguagens (ID 3). Linguagens é mantida sem cursos no conjunto inicial; os seis badges continuam divididos entre as duas primeiras categorias.

Os módulos e as aulas relacionais são a estrutura educacional específica de Detalhes: Matemática possui dez módulos e trinta aulas; os demais cursos seguem suas estruturas temáticas do seed. `PresentationJson` é conteúdo editorial separado: a prévia do AVA contém os mesmos seis módulos genéricos e duas aulas de texto por módulo para todos os cursos, além de informações de aulas e avaliações, FAQs, artigo e recomendações. No AVA, módulos podem ser expandidos, mas as aulas não navegam nem registram conclusão; o progresso permanece demonstrativo e fixo em 10%. A migration `AddGenericAvaLessons` atualiza essa seção JSONB nos seis cursos já semeados; `dotnet ef database update` aplica a atualização sem recriar o banco ou substituir as estruturas relacionais.

As imagens são referências relativas aos PNGs existentes em `apps/frontend/public/assets`, expostas como caminhos `/assets/`; o seed não copia arquivos nem faz downloads. A inicialização insere somente IDs ausentes e não duplica nem sobrescreve registros existentes. Atualize o banco antes de executar a API quando a opção automática estiver ausente ou desabilitada.

## Executar a API

Após configurar a conexão e aplicar as migrations, execute neste diretório:

```sh
dotnet run --project src/Catalog.Api --launch-profile http
```

O perfil usa `http://localhost:5080`, ambiente Development e não exige certificado HTTPS. A URL efetiva aparece no terminal. Encerre a API com `Ctrl+C`.

- Swagger UI: <http://localhost:5080/swagger>.
- OpenAPI: <http://localhost:5080/openapi/v1.json>.

O OpenAPI é gerado pelo suporte nativo do ASP.NET Core; o Swagger UI consome esse documento. Ambos ficam disponíveis somente em Development. Para outro endereço de execução, use a URL informada pela aplicação com os mesmos caminhos.

## Imagem Docker de runtime

O contexto de build é a raiz do monorepo. Construa a imagem com Docker instalado:

```sh
docker build --file apps/backend/Dockerfile --tag catalog-api:local .
```

A imagem final usa o runtime ASP.NET Core 10, executa como usuário não-root `app` e não contém SDK, `dotnet-ef` ou os testes. Ela usa `PORT` fornecida pelo ambiente, com padrão `8080`, e `ASPNETCORE_ENVIRONMENT=Production`; Swagger e OpenAPI não ficam habilitados nesse ambiente. Antes de iniciar a imagem com `Database__InitializeOnStartup=false`, aplique manualmente as migrations ao PostgreSQL acessível pelo contêiner usando o comando `dotnet ef database update` deste README. A imagem de runtime não instala a ferramenta EF.

Exemplo para um PostgreSQL acessível pela mesma rede Docker (substitua os nomes, usuário, senha fictícia e origem pelos valores do ambiente descartável):

```sh
docker run --rm --network 'REDE_DO_POSTGRES' --publish 8080:8080 \
  --env PORT=8080 \
  --env 'ConnectionStrings__DefaultConnection=Host=postgres;Port=5432;Database=catalog_db;Username=catalog_user;Password=substitua-este-valor' \
  --env FRONTEND_URL='https://frontend.example' \
  --env Database__InitializeOnStartup=false \
  catalog-api:local
```

`REDE_DO_POSTGRES` representa uma rede Docker que dá acesso ao serviço PostgreSQL pelo nome `postgres`; não publique o banco apenas para permitir essa conexão. Não use este exemplo contra o banco de desenvolvimento ou de demonstração. Para o Railway, a flag `Database__InitializeOnStartup=true` é configurada somente como variável de runtime, como descrito em [Inicialização automática para a demonstração](#inicialização-automática-para-a-demonstração) e no roteiro do Railway.

## Endpoints

### `GET /api/courses`

| Parâmetro opcional | Tipo | Comportamento |
| --- | --- | --- |
| `search` | texto | Substring literal em nome completo **ou** breve, sem diferenciar caixa; ignora espaços externos e termo em branco |
| `categoryId` | inteiro | Categoria do curso |
| `modalityId` | inteiro | Modalidade do curso |
| `statusId` | inteiro | Status do curso |

Todos os critérios informados são aplicados cumulativamente. `%`, `_` e barra invertida na pesquisa são caracteres literais, não curingas; não há normalização de acentos. Não existe filtragem implícita por status ou data de disponibilização.

A resposta é `{"items":[...],"totalCount":N}`. Cada item contém `id`, `fullName`, `shortName`, `description`, `shortDescription`, `category`, `modality`, `status`, `workloadHours`, `targetAudience`, `imageUrl` e `availableOn`. `imageUrl` é absoluto e usa `FRONTEND_URL`; cada classificação é um objeto com `id` e `name`.

`totalCount` é a quantidade após todos os critérios e corresponde ao tamanho de `items`. Não há paginação. Sem resultados, a resposta HTTP 200 é:

```json
{"items":[],"totalCount":0}
```

### `GET /api/courses/{id}`

Retorna os campos do resumo, `shortDescription`, `modules` e `presentation` tipado. A apresentação inclui certificação, `detailsImageUrl`, informações de aulas e avaliações, FAQs e `learningPreview` (incluindo `bannerImageUrl`, sidebar demonstrativa, artigo e recomendações). URLs de imagem apontam para os assets do frontend, não para arquivos copiados ao backend. Cada módulo contém `id`, `title`, `description`, `order` e `lessons`; cada aula contém `id`, `title`, `description` e `order`. Módulos e aulas são apresentados por ordem e, em empate, por ID. Coleções sem conteúdo são `[]`. `availableOn` é uma data `yyyy-MM-dd` ou `null`.

Não há endpoints de cadastro, edição ou exclusão. Os [exemplos Bash e PowerShell na raiz](../../README.md#consultar-a-api) usam o curso fictício `1` e mostram a combinação dos quatro parâmetros.

## Erros HTTP

As respostas de erro usam JSON Problem Details (`application/problem+json`).

| Status | Situação |
| --- | --- |
| 400 | Identificador de rota ou filtro não pode ser interpretado como inteiro; a resposta inclui os erros de validação |
| 404 | Curso ou rota inexistente |
| 405 | Método de escrita não disponibilizado na rota do catálogo |
| 500 | Falha inesperada, com mensagem genérica e sem detalhes internos |

Identificador de classificação bem formado, mas inexistente, não é erro: retorna listagem vazia com quantidade zero. Stack traces e credenciais não são incluídos nas respostas, inclusive em Development.

## Testes

```sh
dotnet test
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
```

Os testes unitários não exigem banco. Os testes de integração exigem Docker instalado e o daemon em execução. O Testcontainers inicia PostgreSQL 17 em porta dinâmica e cria bancos exclusivos para os testes; não usa nem apaga o banco do Compose. O contêiner e seus recursos são liberados ao terminar.

Na primeira execução, é necessário acesso ao registro de imagens para baixar `postgres:17` e os recursos auxiliares do Testcontainers. É possível antecipar o download do PostgreSQL:

```sh
docker pull postgres:17
```

Não é preciso iniciar o Compose para executar os testes. Se Docker não estiver disponível, os testes de integração falham explicitamente; não são substituídos por banco em memória nem ignorados como se tivessem passado.
