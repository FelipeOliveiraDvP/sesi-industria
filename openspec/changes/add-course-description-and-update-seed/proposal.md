# Proposal

## Why

O catálogo já persiste a descrição do curso e a devolve nos detalhes, mas não a inclui na listagem; além disso, o seed atual não representa os cursos documentados na engenharia reversa. A mudança alinha o contrato de consulta e os dados fictícios do catálogo, inclusive para bancos que já receberam o seed anterior.

## What Changes

- Incluir a descrição do curso nos itens de `GET /api/courses`, mantendo o contrato de detalhes.
- Atualizar o seed para conter exatamente seis cursos, três categorias, duas modalidades e três status, com dados fictícios e diversidade de cargas horárias, módulos, descrições de aulas e públicos-alvo.
- Basear nomes e descrições breves dos cursos no conteúdo transcrito da engenharia reversa; completar os dados não documentados com conteúdo fictício. A documentação visual confirma duas categorias, então a terceira será uma extensão fictícia para atender à quantidade solicitada.
- Planejar uma migration de dados direcionada aos registros do seed atual para que bancos existentes recebam o novo catálogo, sem remover registros fora do conjunto inicial.
- Cobrir o campo adicional e a atualização/reprodução dos dados com testes de contrato, persistência e seed.

## Capabilities

### New Capabilities

Nenhuma.

### Modified Capabilities

- `course-catalog-query`: os itens da listagem passam a incluir a descrição do curso.
- `catalog-persistence`: atualizar o conjunto inicial fictício e especificar a atualização dos registros do seed em bancos existentes.

## Impact

- API: DTO de resumo e projeção da listagem; o DTO de detalhes já expõe a descrição.
- Persistência: o campo obrigatório `Courses.Description` e seu mapeamento já existem; a migration prevista atualiza dados do seed, sem adicionar outra coluna.
- Dados iniciais e testes em `apps/backend/src/Catalog.Api/Data/Seed` e `apps/backend/tests/Catalog.Api.Tests`.
- O contrato JSON da listagem recebe um campo aditivo. A migration de dados deve limitar alterações aos IDs gerenciados pelo seed e preservar registros fora desse conjunto.
