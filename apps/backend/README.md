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

O PostgreSQL deve estar saudável antes de atualizar o banco. A API usa a configuração padrão `ConnectionStrings:DefaultConnection`. Defina a variável no terminal em que executará os comandos .NET.

Bash:

```bash
export ConnectionStrings__DefaultConnection='Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
```

PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = 'Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password'
```

Esses valores correspondem aos padrões locais do Compose. Ajuste-os se tiver sobrescrito banco, porta ou credenciais. `.env.example` é apenas referência e não é carregado automaticamente pelo ASP.NET Core. Não versione configurações locais com credenciais reais.

## Migrations

Com a conexão configurada, execute em `apps/backend`:

```sh
dotnet tool restore
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

`InitialCreate` e seu snapshot já estão versionados. Não crie outra migration inicial ao clonar. A API não aplica migrations no startup.

Somente depois de alterar o modelo, crie uma nova migration, substituindo `NomeDaMigration` por um nome descritivo:

```sh
dotnet ef migrations add NomeDaMigration --project src/Catalog.Api --startup-project src/Catalog.Api --output-dir Data/Migrations
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
```

Revise o código gerado antes de atualizar um banco que contenha dados. `--output-dir` é relativo ao projeto da API; os arquivos ficam em `src/Catalog.Api/Data/Migrations`.

## Dados iniciais

`dotnet ef database update` também executa o seed, inclusive quando nenhuma migration nova precisa ser aplicada. Ele insere os dados fictícios definidos em `Data/Seed`: seis cursos, três categorias, duas modalidades, três status e suas estruturas de módulos e aulas.

IDs, conteúdo e datas são estáveis. A inicialização insere somente IDs ausentes; não duplica nem sobrescreve registros existentes. As imagens de apresentação são arquivos SVG locais em `wwwroot/images/courses`, sem downloads ou serviços de geração. Atualize o banco antes de executar a API, pois o startup HTTP não executa migrations ou seed.

## Executar a API

Após configurar a conexão e aplicar as migrations, execute neste diretório:

```sh
dotnet run --project src/Catalog.Api --launch-profile http
```

O perfil usa `http://localhost:5080`, ambiente Development e não exige certificado HTTPS. A URL efetiva aparece no terminal. Encerre a API com `Ctrl+C`.

- Swagger UI: <http://localhost:5080/swagger>.
- OpenAPI: <http://localhost:5080/openapi/v1.json>.

O OpenAPI é gerado pelo suporte nativo do ASP.NET Core; o Swagger UI consome esse documento. Ambos ficam disponíveis somente em Development. Para outro endereço de execução, use a URL informada pela aplicação com os mesmos caminhos.

## Endpoints

### `GET /api/courses`

| Parâmetro opcional | Tipo | Comportamento |
| --- | --- | --- |
| `search` | texto | Substring literal em nome completo **ou** breve, sem diferenciar caixa; ignora espaços externos e termo em branco |
| `categoryId` | inteiro | Categoria do curso |
| `modalityId` | inteiro | Modalidade do curso |
| `statusId` | inteiro | Status do curso |

Todos os critérios informados são aplicados cumulativamente. `%`, `_` e barra invertida na pesquisa são caracteres literais, não curingas; não há normalização de acentos. Não existe filtragem implícita por status ou data de disponibilização.

A resposta é `{"items":[...],"totalCount":N}`. Cada item contém `id`, `fullName`, `shortName`, `category`, `modality`, `status`, `workloadHours`, `targetAudience`, `imageUrl` e `availableOn`. Cada classificação é um objeto com `id` e `name`.

`totalCount` é a quantidade após todos os critérios e corresponde ao tamanho de `items`. Não há paginação. Sem resultados, a resposta HTTP 200 é:

```json
{"items":[],"totalCount":0}
```

### `GET /api/courses/{id}`

Retorna os campos do resumo, mais `description` e `modules`. Cada módulo contém `id`, `title`, `description`, `order` e `lessons`; cada aula contém `id`, `title`, `description` e `order`. Módulos e aulas são apresentados por ordem e, em empate, por ID. Coleções sem conteúdo são `[]`. `availableOn` é uma data `yyyy-MM-dd` ou `null`.

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
