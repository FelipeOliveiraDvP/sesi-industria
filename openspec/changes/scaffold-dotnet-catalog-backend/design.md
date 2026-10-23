# Design

## Context

Ver `proposal.md`, seção Why, para a motivação. A inspeção encontrou somente `openspec/` e `.opencode/`, sem aplicações, READMEs, Compose, `.gitignore` ou especificações de capacidades existentes. Não há convenção de backend a adaptar; a configuração e as skills atuais serão preservadas.

O ambiente inspecionado oferece SDK .NET `10.0.401`, runtime ASP.NET Core `10.0.12` e Docker Compose `v5.1.3`. Isso confirma disponibilidade das ferramentas, não execução de restore, build, testes ou disponibilidade do daemon Docker. O diretório atual não possui metadados Git ou remoto conhecido.

Os contratos estão em `specs/course-catalog-query/spec.md`, `specs/catalog-persistence/spec.md` e `specs/backend-local-runtime/spec.md`. Os detalhes de busca, representação de identificadores e tipos de dados abaixo são escolhas técnicas para completar o contrato, não novas regras de negócio a acrescentar ao PRD.

## Goals / Non-Goals

**Goals:**

- Concentrar produção em um projeto Web API e testes em outro, dentro de `apps/backend`, com comandos executáveis a partir desse diretório.
- Usar o mesmo provedor e a mesma versão principal de PostgreSQL no desenvolvimento e nos testes relacionais.
- Separar contratos HTTP de entidades, mantendo composição de consultas simples e inteiramente traduzida para SQL.
- Tornar atualização do esquema, carga inicial e testes reproduzíveis, explícitos e não destrutivos para o banco de desenvolvimento.

**Non-Goals:**

- Não criar projetos separados de domínio, aplicação ou infraestrutura, repositório genérico, CQRS, MediatR, mensageria ou serviços adicionais.
- Não preparar publicação, contêiner da API, frontend, autenticação, administração ou integrações externas.
- Não introduzir paginação, ordenação configurável, busca aproximada, normalização de acentos ou regras de visibilidade baseadas em status ou data.
- Não converter escolhas técnicas em novas exigências funcionais no `docs/prd.md`.

## Decisions

### 1. Solução independente com dois projetos e ferramentas locais

Criar a estrutura proposta abaixo. O SDK .NET 10 suporta solução `.slnx`; mantê-la no backend permite `dotnet restore`, `dotnet build` e `dotnet test` sem projetos de outras aplicações.

```text
apps/backend/
├── Catalog.slnx
├── .config/dotnet-tools.json
├── .env.example
├── README.md
├── src/Catalog.Api/
│   ├── Entities/
│   ├── Data/
│   │   ├── Migrations/
│   │   └── Seed/
│   ├── Configurations/
│   ├── Controllers/
│   ├── DTOs/
│   ├── Services/
│   ├── Properties/launchSettings.json
│   ├── wwwroot/images/courses/
│   ├── appsettings.json
│   ├── Program.cs
│   └── Catalog.Api.csproj
└── tests/Catalog.Api.Tests/
    ├── Unit/
    └── Integration/
```

Ambos os projetos usarão `net10.0` e referências anuláveis habilitadas. Fixar versões exatas e estáveis de EF Core, `Microsoft.EntityFrameworkCore.Design`, Npgsql e `dotnet-ef` da linha 10, com dependências Microsoft alinhadas e compatibilidade verificada pelo restore. As versões de patch serão selecionadas na implementação; não usar versões flutuantes, previews ou o ramo de desenvolvimento 11. Configurar `Microsoft.EntityFrameworkCore.Design` como dependência de desenvolvimento e versionar o manifesto local de `dotnet-ef`.

**Alternativa:** solução na raiz com múltiplas camadas. Rejeitada por acoplar aplicações futuras e aumentar a estrutura sem benefício para duas consultas.

### 2. Controllers, serviço de consulta e tratamento HTTP nativo

Adotar controllers porque não existe padrão anterior e a estrutura solicitada já os contempla. `CoursesController` delegará a um serviço concreto de consultas, registrado com escopo por requisição junto de `CatalogDbContext`. Não criar interface ou camada de repositório apenas para encapsular o EF Core.

Usar `[ApiController]`, model binding tipado, DTOs e os mecanismos nativos de Problem Details, exception handler e status code pages. Cursos inexistentes produzirão 404; identificadores não interpretáveis produzirão 400; métodos de escrita nas rotas existentes produzirão 405. Usar parâmetro de rota tipado sem restrição `:int` que transformaria erro de formato em 404. Exceções inesperadas produzirão 500 genérico, inclusive em desenvolvimento, sem expor stack traces ou conexão. Não habilitar logging de dados sensíveis.

Gerar OpenAPI com `Microsoft.AspNetCore.OpenApi` 10 e usar somente o pacote `Swashbuckle.AspNetCore.SwaggerUI` para a interface, apontando para `/openapi/v1.json`. Habilitar documento e UI em Development, com Swagger em `/swagger`. Não duplicar geradores OpenAPI nem conservar WeatherForecast. Disponibilizar o ponto de entrada para `WebApplicationFactory` sem criar um segundo host de produção.

O perfil local `http` utilizará `http://localhost:5080` e ambiente Development, sem exigir IDE, certificado HTTPS ou abertura automática de navegador. Isso é configuração de desenvolvimento, não decisão de publicação.

**Alternativas:** Minimal APIs continuam válidas tecnicamente, mas controllers aproveitam a organização pedida e a validação nativa; geração completa por outro pacote Swagger adicionaria uma responsabilidade já atendida pelo suporte nativo.

### 3. Modelo relacional explícito, sem regras adicionais

Implementar `Course`, `Category`, `Modality`, `CourseStatus`, `CourseModule` e `Lesson`, com navegações nos dois sentidos e uma configuração `IEntityTypeConfiguration<T>` por entidade. Aplicar configurações pelo assembly do contexto.

| Entidade | Propriedades e tipos propostos |
| --- | --- |
| `Category`, `Modality`, `CourseStatus` | `Id: int`, `Name: string` obrigatório, máximo 120 caracteres |
| `Course` | `Id: int`; `FullName` obrigatório até 200; `ShortName` obrigatório até 80; `Description` obrigatório até 4000; `CategoryId`, `ModalityId`, `CourseStatusId`: FKs obrigatórias; `WorkloadHours: decimal(7,2)`; `TargetAudience` obrigatório até 1000; `ImageUrl` obrigatório até 512; `AvailableOn: DateOnly?` |
| `CourseModule` | `Id: int`; `CourseId` obrigatório; `Title` obrigatório até 200; `Description` obrigatório até 4000; `DisplayOrder: int` obrigatório |
| `Lesson` | `Id: int`; `CourseModuleId` obrigatório; `Title` obrigatório até 200; `Description` obrigatório até 4000; `DisplayOrder: int` obrigatório |

Identificadores inteiros serão atribuídos explicitamente pelo seed e configurados sem geração automática, evitando conflitos de sequências com IDs fixos. Não há operações de cadastro que exijam outra estratégia de geração. Datas são datas de calendário opcionais, armazenadas como `date` e expostas como `yyyy-MM-dd` ou `null`, sem conversões de fuso horário.

Configurar PKs, NOT NULL, limites textuais e FKs. Restringir exclusão de classificações referenciadas; permitir cascata entre curso, módulos e aulas como escolha técnica de integridade, sem disponibilizar exclusão por API. Não impor unicidade de nomes ou ordens, quantidades mínimas por curso, duração mínima, datas condicionadas ao status ou outras invariantes não solicitadas.

Indexar as FKs de classificação para filtros e os pares `(CourseId, DisplayOrder)` e `(CourseModuleId, DisplayOrder)` para navegação ordenada, evitando índices redundantes. Uma busca por substring com `ILIKE` não será apresentada como beneficiária de um índice B-tree comum; não adicionar extensão de busca ao catálogo mínimo.

**Alternativas:** enums para classificações não atendem à exigência de entidades com identificador e nome; JSON para módulos ou aulas esconderia relacionamentos exigidos; GUIDs e geração automática de inteiros não trazem benefício ao conjunto fixo e somente leitura desta entrega.

### 4. Contratos DTO e consulta cumulativa no PostgreSQL

Contratos JSON em camelCase:

- `CourseListResponse`: `items` e `totalCount`.
- `CourseSummaryDto`: `id`, `fullName`, `shortName`, `category`, `modality`, `status`, `workloadHours`, `targetAudience`, `imageUrl` e `availableOn`.
- Cada classificação: objeto com `id` e `name`.
- `CourseDetailDto`: mesmos campos do resumo, mais `description` e `modules`.
- Cada módulo: `id`, `title`, `description`, `order` e `lessons`; cada aula: `id`, `title`, `description` e `order`.

Construir uma consulta sem tracking. Normalizar apenas os espaços externos de `search`; ignorar termo vazio e usar substring literal sem distinção de caixa em `FullName` OU `ShortName`, por tradução PostgreSQL `ILIKE`, escapando `%`, `_` e barra invertida. Cada filtro opcional acrescentará um `AND` por igualdade de FK. Não carregar o catálogo para filtrar em memória e não filtrar implicitamente por status ou data.

Projetar diretamente para DTOs e executar assincronamente, propagando cancelamento. Ordenar a listagem por nome completo e identificador para estabilidade. Sem paginação, calcular `totalCount` a partir da própria lista materializada evita duas consultas e divergência entre contagem e itens. No detalhe, projetar módulos e aulas por `DisplayOrder`, com desempate por ID, sem lazy loading ou exposição de entidades.

**Alternativas:** filtrar coleções em memória não valida a semântica real do provedor; paginação, busca sem acentos e full-text alterariam o escopo. A escolha de substring literal, caixa indiferente e termo em branco é um padrão técnico documentado nesta mudança, não uma regra extra para o PRD.

### 5. Seed fixo na atualização do banco

Usar `UseSeeding` e `UseAsyncSeeding` com uma definição comum de dados constantes. A versão síncrona é necessária porque `dotnet ef database update` utiliza esse caminho; a assíncrona atende às operações assíncronas de migrations nos testes. O mecanismo de migrations do EF protege a inicialização por lock.

Inserir por IDs conhecidos ausentes, respeitando a ordem de dependências. Não usar uma verificação única de tabela não vazia para abandonar o seed inteiro; não gerar IDs, datas ou conteúdo aleatórios; não sobrescrever registros existentes nem apagar registros adicionais. Repetir a atualização do banco deve executar a verificação idempotente mesmo sem nova migration.

Preparar três categorias, duas modalidades e três status fictícios. Um conjunto inicial concreto para orientar testes é:

| ID | Nome completo / nome breve | Categoria | Modalidade | Status | Horas | Público-alvo |
| --- | --- | --- | --- | --- | --- | --- |
| 1 | Fundamentos de Tecnologia Educacional / TecEdu | Tecnologia Educacional | Online | Disponível | 12 | Educadores |
| 2 | Oficina de Aprendizagem Digital / DigitalLab | Tecnologia Educacional | Presencial | Em breve | 20 | Instrutores |
| 3 | Projetos Colaborativos para Educação / Projetos | Gestão e Projetos | Online | Disponível | 24 | Coordenadores |
| 4 | Comunicação em Equipes Educacionais / Conecta | Gestão e Projetos | Presencial | Encerrado | 8 | Lideranças educacionais |
| 5 | Segurança nas Rotinas de Aprendizagem / AprenderSeguro | Segurança e Bem-estar | Online | Em breve | 16 | Comunidade escolar |
| 6 | Bem-estar em Ambientes Educacionais / Cuidar | Segurança e Bem-estar | Presencial | Encerrado | 6 | Equipes de apoio |

Os rótulos são dados fictícios, não uma enumeração fechada de regras. Usar descrições próprias, diferentes quantidades de módulos e aulas, ordens explícitas e algumas datas fixas e outras nulas, sem associar regras de consulta ao status. Incluir imagens SVG simples como arquivos estáticos locais em `wwwroot/images/courses/`, referenciadas por caminhos relativos servidos pelo backend; isso não cria frontend nem dependência remota.

Gerar e versionar a migration `InitialCreate` e seu snapshot em `Data/Migrations`. A API não aplicará migrations automaticamente ao iniciar. O fluxo local é atualizar o banco explicitamente e depois executar a API; testes relacionais usarão migrations, nunca `EnsureCreated` como substituto.

**Alternativas:** `HasData` é possível para dados fixos, mas não reexecuta a checagem de inserções a cada atualização sem migration nova; carga no startup da API mistura execução HTTP com preparação do banco. Downloads, relógio e geradores externos impediriam reprodução determinística.

### 6. Compose mínimo e configuração nativa de ambiente

O Compose da raiz terá somente o serviço `postgres`, com imagem oficial `postgres:17`, volume nomeado `catalog_postgres_data` montado em `/var/lib/postgresql/data` e `restart: unless-stopped`. A versão 17 é estável e compatível, sem exigir funcionalidades recentes ou o layout de volume de outra versão principal.

Configurar interpolação com padrões estritamente locais:

| Variável do Compose | Padrão |
| --- | --- |
| `POSTGRES_DB` | `catalog_db` |
| `POSTGRES_USER` | `catalog_user` |
| `POSTGRES_PASSWORD` | `catalog_dev_password` |
| `POSTGRES_PORT` | `5432` |

Publicar `127.0.0.1:${POSTGRES_PORT:-5432}:5432` para restringir exposição ao host. O healthcheck utilizará `pg_isready` com as variáveis de usuário e banco do contêiner, escapadas como `$${POSTGRES_USER}` e `$${POSTGRES_DB}` para evitar expansão prematura pelo Compose. Adotar intervalo e timeout de 5 segundos, dez tentativas e período inicial de 10 segundos.

`apps/backend/.env.example` conterá pelo menos:

```dotenv
ConnectionStrings__DefaultConnection=Host=localhost;Port=5432;Database=catalog_db;Username=catalog_user;Password=catalog_dev_password
```

Ler a conexão por `GetConnectionString("DefaultConnection")` usando a configuração padrão. Não armazenar uma conexão secreta em appsettings e não criar carregador dotenv. Validar ausência de conexão com erro claro no startup. O Compose pode interpolar seu próprio ambiente, mas não exporta automaticamente variáveis ao processo .NET no host. Qualquer sobrescrita de porta ou credenciais exige atualização correspondente da conexão, conforme documentação.

**Alternativas:** contêiner da API contraria a execução pedida no host; imagem `latest` permite mudança de versão principal; arquivo `.env` do backend como configuração automática da API não é comportamento nativo do ASP.NET Core.

### 7. Testes isolados usando PostgreSQL real

Usar xUnit com runner compatível, `Microsoft.NET.Test.Sdk`, `Microsoft.AspNetCore.Mvc.Testing` 10 e o módulo `Testcontainers.PostgreSql`. O Testcontainers iniciará PostgreSQL 17 efêmero em porta dinâmica e fornecerá sua conexão à configuração isolada de `WebApplicationFactory` antes de iniciar o host. Isso não altera o serviço Compose nem utiliza o banco de desenvolvimento.

Uma fixture de coleção gerenciará o contêiner e sua liberação. O catálogo somente leitura pode compartilhar uma base inicializada; testes de migrations, integridade ou alterações de dados usarão bancos isolados no contêiner para não depender da ordem dos testes. Não criar paralelismo de testes que disputem o mesmo esquema mutável. Falta de Docker deve produzir falha ou impedimento explícito, nunca sucesso com integração silenciosamente ignorada.

- **Unitários, `Category=Unit`:** normalização e escape do termo, definições determinísticas de seed e transformações puras que existirem, sem criar abstrações apenas para simulação de EF.
- **Integração, `Category=Integration`:** inicialização do host; OpenAPI/Swagger; criação e reaplicação de migrations; PKs, FKs e campos obrigatórios; quantidades e diversidade do seed; repetição dos callbacks de seed e comparação em bancos vazios; imagens locais; consultas por ambos os nomes; filtros individuais e combinados; contagens; detalhes ordenados; data nula; coleções vazias; 400, 404, 405 e tratamento 500 sem detalhes internos.

Garantir pelo menos um teste em que somente o nome breve corresponde ao termo e uma combinação de pesquisa com os três filtros que exclua cursos correspondentes apenas parcialmente. Não usar EF InMemory ou SQLite para aprovar comportamento relacional ou `ILIKE`.

**Alternativa:** banco de testes manual no serviço de desenvolvimento evita uma biblioteca, mas aumenta configuração, risco de colisão de dados e variação de ambiente. O contêiner efêmero justifica a dependência de testes e mantém o fluxo `dotnet test` reproduzível.

### 8. Documentação com separação entre produto e operação

`docs/prd.md` será um documento funcional em português brasileiro: objetivo do produto, seis entidades com seus campos e relacionamentos, todas as regras explícitas solicitadas e os sete casos de uso com finalidade e comportamento esperado. Não conterá infraestrutura, bibliotecas, comandos, critérios de processo seletivo, uso de IA, frontend, dúvidas, lacunas, pendências, menção a protótipo ou referências externas. Também não incorporará decisões técnicas adicionais de busca, tipos ou limites de campos deste design.

O README da raiz conterá pré-requisitos (.NET SDK 10, Docker/Compose e Git), clone com URL parametrizada enquanto não houver remoto conhecido, diretório correto para cada comando, inicialização e saúde do banco, exportação da conexão em Bash e PowerShell, ferramentas/dependências, migrations, execução, Swagger, exemplos dos dois endpoints e parada não destrutiva dos contêineres.

O README do backend concentrará tecnologias, árvore de diretórios e manutenção diária, referenciando a raiz para o primeiro preparo. Os comandos-base, executados em `apps/backend`, serão:

```text
dotnet tool restore
dotnet restore
dotnet build
dotnet ef database update --project src/Catalog.Api --startup-project src/Catalog.Api
dotnet run --project src/Catalog.Api --launch-profile http
dotnet test
dotnet test --filter "Category=Unit"
dotnet test --filter "Category=Integration"
```

Para criar migrations durante desenvolvimento, documentar `dotnet ef migrations add <NomeDaMigration> --project src/Catalog.Api --startup-project src/Catalog.Api --output-dir Data/Migrations`. O scaffold gera `InitialCreate` uma única vez; quem clonar aplica as migrations existentes, sem gerar outra inicial.

O `.gitignore` da raiz abrangerá `bin`, `obj`, configurações locais, `.env` e variantes locais, IDEs, logs e resultados gerados, com exceção explícita para `.env.example`. Preservar manifesto de ferramentas, solução, migrations, fontes, assets e documentação. Não ignorar appsettings versionados de configuração não secreta ou diretórios OpenSpec.

**Alternativa:** reunir instruções técnicas no PRD violaria sua finalidade e o conteúdo pedido; repetir todo o tutorial nos dois READMEs aumentaria divergência sem ajudar a manutenção.

### Referências técnicas consultadas

- [Compatibilidade do EF Core 10 com .NET 10](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-10.0/whatsnew).
- [Release estável do provedor Npgsql 10](https://www.npgsql.org/efcore/release-notes/10.0.html).
- [Seeding por callbacks e migrations](https://learn.microsoft.com/en-us/ef/core/modeling/data-seeding).
- [Swagger UI com OpenAPI nativo](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/using-openapi-documents?view=aspnetcore-10.0).
- [Interpolação de ambiente do Compose](https://docs.docker.com/compose/how-tos/environment-variables/variable-interpolation/) e [healthcheck PostgreSQL](https://docs.docker.com/compose/how-tos/startup-order/).
- [PostgreSQL no Testcontainers para .NET](https://dotnet.testcontainers.org/modules/postgres/).

## Risks / Trade-offs

- [Docker ou obtenção da imagem indisponíveis] → Documentar o pré-requisito dos testes, identificar o impedimento e não relatar integração como aprovada.
- [Pacotes com versões principais ou patches incompatíveis] → Fixar versões estáveis compatíveis, manter ferramentas na linha 10 e verificar restore, build e testes na implementação.
- [Busca por substring sem índice especializado] → Aceitar varredura no catálogo inicial pequeno; manter índices relacionais e não adicionar extensão ou mecanismo de busca sem requisito concreto.
- [Listagem sem paginação cresce com o catálogo] → Cumprir o escopo atual e manter contrato simples; eventual paginação exigirá uma mudança própria.
- [Callbacks de seed executados por caminhos síncronos e assíncronos] → Compartilhar a definição dos dados, implementar ambos e testar repetição e estabilidade de IDs e contagens.
- [Credenciais padrão confundidas com credenciais de produção] → Identificá-las como fictícias e locais, limitar a porta ao loopback e ignorar configurações locais reais.
- [Mudança de credenciais em volume PostgreSQL já inicializado] → Explicar que variáveis de inicialização não reconfiguram usuários existentes; nunca sugerir remoção automática do volume para corrigir isso.
- [Escolhas técnicas confundidas com regras de negócio] → Restringir o PRD ao conteúdo explícito e manter semântica adicional de transporte e persistência apenas no design e nas especificações técnicas.

## Migration Plan

1. Na implementação posterior, criar os dois projetos, configurações e testes sem alterar as ferramentas OpenSpec existentes.
2. Gerar a migration inicial, configurar seed, validar o Compose e iniciar apenas PostgreSQL. Aguardar o estado saudável antes de atualizar o banco.
3. Configurar a conexão no processo, restaurar ferramentas e dependências e aplicar a migration pelo comando documentado. Repetir a atualização para confirmar ausência de duplicações.
4. Executar a API no host e verificar os dois endpoints, imagens locais e Swagger. Executar `dotnet restore`, `dotnet build` e `dotnet test`, registrando comandos, diretórios e resultados reais.
5. Validar as instruções dos READMEs e as restrições do PRD. Parar somente processos e contêineres iniciados para a validação, preservando o volume local.

Não existem dados legados a migrar nem publicação a planejar. `docker compose down` preservará o volume; remover volumes ou reverter migrations que apaguem dados não fará parte da inicialização, dos testes contra desenvolvimento ou de uma recuperação automática. Qualquer rollback destrutivo de um banco local existente requer avaliação e autorização próprias.
