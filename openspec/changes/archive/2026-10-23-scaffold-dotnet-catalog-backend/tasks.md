# Tasks

## 1. Base independente do backend

- [x] 1.1 Criar `apps/backend/Catalog.slnx`, `src/Catalog.Api` e `tests/Catalog.Api.Tests`, ambos em `net10.0`, com referência de testes à API e ponto de entrada acessível a `WebApplicationFactory`; verificar a estrutura solicitada, a descoberta dos dois projetos na solução e a ausência de dependência de frontend.
- [x] 1.2 Fixar dependências estáveis compatíveis de EF Core 10, Npgsql 10, ferramentas de design, OpenAPI/Swagger e testes; versionar `.config/dotnet-tools.json` com `dotnet-ef` 10 e verificar `dotnet tool restore` e `dotnet restore` em `apps/backend` sem conflitos de dependências.
- [x] 1.3 Criar `.gitignore` na raiz para `bin`, `obj`, configurações locais, `.env`, IDEs, logs e resultados gerados; verificar explicitamente que `.env.example`, solução, manifesto de ferramentas, migrations, assets e documentação não são excluídos e que `.opencode/` e `openspec/` existentes são preservados.
- [x] 1.4 Iniciar `apps/backend/README.md` com tecnologias, estrutura e comandos reais de restore e build; verificar os caminhos e executar `dotnet build` da solução a partir desse diretório.

## 2. Requisitos funcionais do produto

- [x] 2.1 Criar `docs/prd.md` em português brasileiro com objetivo do catálogo, entidades Curso, Categoria, Modalidade, Status do curso, Módulo e Aula, campos, relacionamentos e todas as regras explícitas de consulta e dados iniciais; verificar cobertura dos atributos obrigatórios, data opcional, mínimos de dados fictícios, filtros cumulativos, contagem e proibição de escrita.
- [x] 2.2 Documentar no PRD finalidade e comportamento esperado dos sete casos de uso: listar, pesquisar por nome, filtrar por categoria, filtrar por modalidade, filtrar por status, combinar pesquisa e filtros e consultar detalhes; verificar que todos estão presentes sem novas funcionalidades.
- [x] 2.3 Revisar o PRD contra as restrições do pedido; verificar que contém apenas conteúdo funcional explícito e não inclui infraestrutura, frontend, publicação, processo seletivo, IA, pendências, dúvidas, lacunas, protótipo, referências externas ou escolhas técnicas adicionais deste design.

## 3. PostgreSQL local e infraestrutura de testes

- [x] 3.1 Criar `docker-compose.yml` na raiz com somente `postgres:17`, serviço `postgres`, interpolação das quatro variáveis documentadas, porta local padrão 5432, volume nomeado, healthcheck e `unless-stopped`; verificar `docker compose config`, os valores padrão e a resolução de sobrescritas, inclusive da porta.
- [x] 3.2 Criar `apps/backend/.env.example` com `ConnectionStrings__DefaultConnection` correspondente aos padrões do Compose; verificar igualdade de host, porta, banco, usuário e senha e ausência de credenciais reais ou carregador dotenv.
- [x] 3.3 Criar fixture PostgreSQL 17 efêmera com `Testcontainers.PostgreSql`, bancos isolados para testes mutáveis e liberação dos recursos; verificar um teste de conexão real, porta dinâmica e ausência de acesso ao banco de desenvolvimento, sem ignorar silenciosamente falta de Docker.
- [x] 3.4 Configurar a identificação dos testes por categoria e documentar Docker, aquisição inicial da imagem e filtros de execução no README do backend; verificar que `Category=Integration` executa o teste de conexão e que as instruções também apresentam `Category=Unit` para os testes isolados dos grupos seguintes.
- [x] 3.5 Documentar no README da raiz pré-requisitos, clone com URL parametrizada, entrada na raiz, Compose, saúde, variáveis sobrescrevíveis e persistência do volume; executar os comandos documentados `docker compose up -d` e `docker compose ps` e verificar estado saudável, sem iniciar serviços extras.

## 4. Modelo relacional, configuração e migrations

- [x] 4.1 Implementar as seis entidades em `Entities/` com campos, tipos, IDs estáveis, navegações e FKs definidos no design; verificar a compilação e a correspondência de campos e cardinalidades com `specs/catalog-persistence/spec.md`.
- [x] 4.2 Implementar configurações por entidade para PKs, NOT NULL, limites textuais, precisão da carga horária, data opcional, FKs, integridade de exclusão e índices de classificação e ordem; verificar metadados do modelo sem acrescentar unicidade de nomes, duração mínima ou outras invariantes não solicitadas.
- [x] 4.3 Implementar `CatalogDbContext`, aplicação das configurações e DI com Npgsql, recebendo `ConnectionStrings:DefaultConnection` pela configuração nativa; verificar que variável de processo é utilizada e que inicialização sem conexão falha com mensagem clara, sem ler automaticamente `.env.example` nem revelar segredos.
- [x] 4.4 Gerar e versionar `InitialCreate` e snapshot em `Data/Migrations` usando `dotnet ef migrations add InitialCreate --project src/Catalog.Api --startup-project src/Catalog.Api --output-dir Data/Migrations` a partir de `apps/backend`; verificar criação e reaplicação do esquema em PostgreSQL vazio, sem usar `EnsureCreated` no lugar das migrations.
- [x] 4.5 Implementar testes de integração do modelo, esquema e integridade em bancos efêmeros isolados; verificar PKs, relações curso/classificações, curso/módulos e módulo/aulas, rejeição de referências inválidas e campos obrigatórios ausentes, aceitação de data nula e ausência de migrations pendentes após atualização.
- [x] 4.6 Acrescentar aos READMEs a exportação da conexão em Bash e PowerShell e os comandos reais para criar/aplicar migrations, com `--project`, `--startup-project` e diretório corretos; verificar os comandos no terminal e explicar que o clone aplica `InitialCreate` existente, sem gerar outra migration inicial.

## 5. Dados fictícios e imagens locais

- [x] 5.1 Definir o conjunto inicial constante de seis cursos, três categorias, duas modalidades e três status, com IDs e datas fixos, diferentes cargas horárias e públicos-alvo e módulos/aulas variados; verificar testes unitários dos mínimos, campos obrigatórios, diversidade e estabilidade das definições, sem relógio ou aleatoriedade.
- [x] 5.2 Implementar callbacks `UseSeeding` e `UseAsyncSeeding` sobre a mesma definição de dados, inserindo IDs ausentes em ordem de dependências sem duplicar ou apagar dados existentes; verificar por integração os caminhos síncrono e assíncrono e a repetição da atualização sem migration nova.
- [x] 5.3 Incluir imagens SVG locais simples em `wwwroot/images/courses/`, associá-las aos cursos e habilitar seu atendimento estático; adicionar teste HTTP verificando que cada referência inicial resolve um recurso local e conferir que nenhuma carga ou imagem depende de serviço remoto.
- [x] 5.4 Implementar testes PostgreSQL de seed em bancos isolados, comparando duas inicializações no mesmo banco e em bancos vazios independentes; verificar quantidades, IDs, valores e relações de todas as entidades, diversidade de módulos/aulas e presença de datas preenchidas e nulas.
- [x] 5.5 Documentar nos READMEs que a atualização do banco também prepara o seed, é idempotente e antecede a execução da API; verificar os comandos documentados e confirmar que instruções operacionais não foram incorporadas ao PRD.

## 6. Contratos e consultas do catálogo

- [x] 6.1 Implementar DTOs de parâmetros, resumo, classificações, resposta com `items`/`totalCount`, detalhes, módulos e aulas; verificar serialização camelCase, data opcional como `null`, campos previstos no design e ausência de entidades ou navegações internas no contrato.
- [x] 6.2 Implementar o serviço concreto de listagem com consulta sem tracking, pesquisa em nome completo OU breve, filtros opcionais cumulativos, projeção SQL, ordenação estável e quantidade derivada dos itens; verificar contra PostgreSQL que não há filtragem em memória, paginação ou restrição implícita de status/data.
- [x] 6.3 Implementar detalhes com projeção dos dados completos, classificações e módulos/aulas ordenados com desempate estável; verificar retorno de dados completos, coleções vazias quando aplicável e ausência de referências circulares ou lazy loading.
- [x] 6.4 Implementar apenas `GET /api/courses` e `GET /api/courses/{id}` em controller com model binding tipado e DI do serviço; verificar 200 nas consultas válidas, 404 para curso inexistente, 400 para identificadores não interpretáveis e ausência de ações de escrita.
- [x] 6.5 Implementar testes unitários da normalização de `search` e escape dos caracteres de `ILIKE`; verificar espaços externos, termo em branco e tratamento literal de `%`, `_` e barra invertida sem executar funções específicas do provedor em memória.
- [x] 6.6 Implementar testes HTTP de listagem e pesquisa com `WebApplicationFactory` e PostgreSQL efêmero; verificar resposta sem critérios, ambos os nomes separadamente, correspondência somente no nome breve, caixa indiferente, termo em branco e caracteres literais.
- [x] 6.7 Implementar testes HTTP dos três filtros individuais, combinações parciais e pesquisa com os três filtros; verificar interseção cumulativa, exclusão de correspondências apenas parciais, contagens exatas, classificação inexistente e `items: []`/`totalCount: 0` quando não houver resultados.
- [x] 6.8 Implementar testes HTTP de detalhes, incluindo dados completos, ordem dos módulos/aulas, data nula, curso sem módulos, módulo sem aulas e curso inexistente; verificar os contratos de sucesso e ausência de recurso usando bancos isolados quando os dados de teste forem modificados.
- [x] 6.9 Documentar os dois endpoints, os quatro parâmetros, formato das respostas e exemplos de pesquisa/filtros e detalhes nos READMEs, evitando duplicação; verificar os exemplos contra a API com dados iniciais e conferir ausência de endpoints adicionais de negócio.

## 7. Inicialização HTTP, erros e Swagger

- [x] 7.1 Configurar Problem Details, exception handler e respostas de erro coerentes para 400, 404, 405 e 500, sem stack traces, conexão ou dados sensíveis; verificar por testes HTTP respostas de erro, tentativas POST/PUT/PATCH/DELETE sem mutação e uma exceção controlada somente no host de testes.
- [x] 7.2 Configurar OpenAPI nativo e Swagger UI em Development, apontando para `/openapi/v1.json`, e remover exemplos como WeatherForecast; verificar que o documento descreve apenas os dois endpoints de negócio, os parâmetros e DTOs previstos, e que `/swagger` é atendido.
- [x] 7.3 Configurar o perfil `http` em `http://localhost:5080` e manter migrations fora do startup HTTP; verificar inicialização da aplicação com conexão de teste isolada e execução pelo terminal sem IDE ou certificado HTTPS.
- [x] 7.4 Completar o fluxo operacional do README da raiz, do clone à parada não destrutiva: saúde do banco, conexão, `apps/backend`, ferramentas, restore, migrations, execução e Swagger; verificar a ordem dos passos e exemplos distintos de Bash/PowerShell, deixando explícito que `.env.example` não configura sozinho o processo.
- [x] 7.5 Completar o README do backend com execução da API, OpenAPI, erros, estrutura final e comandos de teste completo e por categoria; verificar os caminhos, a necessidade de Docker para integração e as referências à raiz, sem repetir integralmente seu tutorial.

## 8. Validação integrada e relato da implementação

- [x] 8.1 Executar o fluxo documentado com PostgreSQL saudável, atualização do banco repetida e API no host; verificar endpoints, contagem, pesquisa/filtros, detalhes, imagens e Swagger e confirmar preservação do esquema e dos dados após recriação não destrutiva do serviço, sem remover volumes.
- [x] 8.2 Executar `dotnet restore`, `dotnet build` e `dotnet test` em `apps/backend`, incluindo integração PostgreSQL real; registrar comandos, diretórios e resultados, corrigir erros introduzidos e verificar que nenhum comando não executado ou teste ignorado foi declarado aprovado.
- [x] 8.3 Conferir a entrega contra os doze critérios de conclusão e as três especificações; verificar independência do frontend, escopo somente leitura, conteúdo funcional estrito do PRD, documentação correspondente aos comandos reais, arquivos ignorados e preservação das configurações preexistentes.
- [x] 8.4 Apresentar resumo dos arquivos criados/modificados, validações efetivamente executadas e instruções mínimas de início; verificar que impedimentos estão informados e que apenas processos/contêineres iniciados para validação são encerrados, com volumes e dados locais preservados.
