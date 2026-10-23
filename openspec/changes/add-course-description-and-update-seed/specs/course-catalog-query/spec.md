# Spec Delta

## MODIFIED Requirements

### Requirement: Listagem de cursos com classificações e quantidade

A API SHALL disponibilizar `GET /api/courses`, retornando HTTP 200 com `items` e `totalCount`. Cada item deve identificar o curso pelos nomes completo e breve, incluir sua descrição e informar categoria, modalidade e status com identificador e nome. A quantidade deve corresponder aos cursos retornados após todos os critérios da consulta, sem exigir paginação.

#### Scenario: Consulta sem critérios

- **WHEN** o consumidor solicita `GET /api/courses` sem parâmetros
- **THEN** a API retorna todos os cursos do catálogo com seus identificadores, nomes, descrições e classificações
- **AND** `totalCount` é igual à quantidade de elementos de `items`

#### Scenario: Descrição do curso na listagem

- **WHEN** o consumidor consulta a listagem e os detalhes de um curso existente
- **THEN** o item correspondente contém a descrição persistida do curso
- **AND** o valor corresponde ao campo `description` retornado por `GET /api/courses/{id}`
