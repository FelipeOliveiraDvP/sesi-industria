# Proposal

## Why

O repositório ainda não possui aplicações, persistência ou documentação de domínio. É necessário estabelecer uma base pequena e executável para consultar um catálogo educacional, com backend independente em .NET 10 e dados fictícios persistidos no PostgreSQL.

## What Changes

- Criar o backend em `apps/backend`, com uma API ASP.NET Core `net10.0`, Entity Framework Core e Npgsql da linha 10, além de um projeto de testes.
- Modelar cursos, categorias, modalidades, status, módulos e aulas, com relacionamentos, restrições, migrations e seed determinístico e idempotente.
- Disponibilizar `GET /api/courses` e `GET /api/courses/{id}` com DTOs, pesquisa nos nomes completo e breve, filtros cumulativos e quantidade de resultados. Os endpoints serão funcionais nesta entrega, conforme os critérios de conclusão.
- Preparar execução local com API no host e apenas PostgreSQL no Docker Compose, configuração por variáveis de ambiente, OpenAPI/Swagger e respostas de erro consistentes.
- Criar `docs/prd.md` em português brasileiro, limitado aos requisitos funcionais, entidades, regras e sete casos de uso explicitamente solicitados.
- Documentar os fluxos reais de execução e desenvolvimento nos READMEs da raiz e do backend; fornecer `.env.example` e regras de exclusão de arquivos locais e gerados.
- Cobrir inicialização, migrations, integridade, seed e consultas com testes automatizados, incluindo integração contra PostgreSQL real.
- Manter fora do escopo frontend, autenticação, autorização, operações administrativas de escrita, matrículas, pagamentos, Moodle, integrações externas e arquiteturas adicionais sem necessidade.

## Capabilities

### New Capabilities

- `course-catalog-query`: consulta HTTP somente leitura do catálogo, com pesquisa, filtros combináveis, contagem e detalhes ordenados de módulos e aulas.
- `catalog-persistence`: modelo relacional do catálogo no PostgreSQL, integridade dos relacionamentos, evolução por migrations e dados fictícios iniciais reproduzíveis.
- `backend-local-runtime`: execução e teste independentes do backend no monorepo, infraestrutura PostgreSQL local, configuração de ambiente e documentação operacional da API.

### Modified Capabilities

Nenhuma. O inventário atual não contém especificações de capacidades.

## Impact

- Novos arquivos em `apps/backend/src/Catalog.Api`, `apps/backend/tests/Catalog.Api.Tests`, configuração local de ferramentas .NET e solução do backend.
- Novos `docker-compose.yml`, `README.md`, `.gitignore`, `docs/prd.md`, `apps/backend/README.md` e `apps/backend/.env.example`.
- Dependências: SDK .NET 10, EF Core 10, provedor Npgsql compatível, ferramentas de migrations, documentação OpenAPI/Swagger e bibliotecas de teste compatíveis com `net10.0`; PostgreSQL oficial executado por Docker Compose.
- Novos contratos públicos de leitura em `/api/courses`; não existem contratos anteriores a migrar nem alterações incompatíveis identificadas.
- A configuração OpenSpec e as skills locais existentes serão preservadas. Esta mudança contém somente planejamento; código, infraestrutura executável, PRD e READMEs serão produzidos apenas na aplicação posterior.
