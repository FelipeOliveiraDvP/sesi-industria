# Spec Delta

## Purpose

Permitir a consulta pública e somente leitura do catálogo educacional, com pesquisa, classificações combináveis, contagem de resultados e detalhes da estrutura de módulos e aulas.

## ADDED Requirements

### Requirement: Listagem de cursos com classificações e quantidade

A API SHALL disponibilizar `GET /api/courses`, retornando HTTP 200 com `items` e `totalCount`. Cada item deve identificar o curso pelos nomes completo e breve e informar categoria, modalidade e status com identificador e nome. A quantidade deve corresponder aos cursos retornados após todos os critérios da consulta, sem exigir paginação.

#### Scenario: Consulta sem critérios

- **WHEN** o consumidor solicita `GET /api/courses` sem parâmetros
- **THEN** a API retorna todos os cursos do catálogo com seus identificadores, nomes e classificações
- **AND** `totalCount` é igual à quantidade de elementos de `items`

### Requirement: Pesquisa pelos nomes completo e breve

A API SHALL aceitar `search` opcional e localizar cursos quando o texto ocorrer no nome completo OU no nome breve. A busca deve considerar o texto literal, sem diferenciar maiúsculas e minúsculas. Espaços nas extremidades devem ser desconsiderados; um termo vazio após esse tratamento não deve restringir a consulta.

#### Scenario: Correspondência no nome completo

- **WHEN** `search` corresponde a parte do nome completo de um curso
- **THEN** esse curso é incluído nos resultados

#### Scenario: Correspondência apenas no nome breve

- **WHEN** `search` corresponde ao nome breve, mas não ao nome completo de um curso
- **THEN** esse curso também é incluído nos resultados

#### Scenario: Variação de caixa e espaços externos

- **WHEN** o consumidor pesquisa um nome com diferenças de maiúsculas e minúsculas e espaços nas extremidades
- **THEN** os resultados equivalem à pesquisa pelo mesmo texto sem essas diferenças

#### Scenario: Pesquisa em branco

- **WHEN** `search` está vazio ou contém apenas espaços
- **THEN** a consulta se comporta como se o parâmetro de pesquisa estivesse ausente

#### Scenario: Caracteres especiais tratados como texto

- **WHEN** o termo contém `%`, `_` ou barra invertida
- **THEN** esses caracteres são pesquisados literalmente e não ampliam a consulta como curingas

### Requirement: Filtros individuais por classificação

A API SHALL aceitar `categoryId`, `modalityId` e `statusId` como parâmetros opcionais independentes. Cada parâmetro informado deve restringir o resultado aos cursos associados ao identificador correspondente; parâmetros ausentes não devem restringir a consulta.

#### Scenario: Filtro por categoria

- **WHEN** o consumidor informa apenas `categoryId`
- **THEN** todos os cursos retornados pertencem à categoria informada
- **AND** a quantidade corresponde somente aos cursos dessa categoria

#### Scenario: Filtro por modalidade

- **WHEN** o consumidor informa apenas `modalityId`
- **THEN** todos os cursos retornados pertencem à modalidade informada

#### Scenario: Filtro por status

- **WHEN** o consumidor informa apenas `statusId`
- **THEN** todos os cursos retornados pertencem ao status informado

### Requirement: Aplicação cumulativa de pesquisa e filtros

A API SHALL permitir qualquer combinação de `search`, `categoryId`, `modalityId` e `statusId`. Um curso somente deve integrar os resultados quando satisfizer TODOS os critérios informados, mantendo a alternativa entre nome completo e nome breve dentro do critério de pesquisa.

#### Scenario: Pesquisa combinada aos três filtros

- **WHEN** o consumidor informa um termo e os identificadores de categoria, modalidade e status
- **THEN** cada curso retornado corresponde ao termo em pelo menos um dos nomes e às três classificações
- **AND** cursos que atendem apenas a parte desses critérios são excluídos
- **AND** `totalCount` corresponde à interseção dos critérios

#### Scenario: Combinação de dois filtros sem pesquisa

- **WHEN** o consumidor informa categoria e modalidade sem `search` ou status
- **THEN** o resultado corresponde à interseção da categoria e modalidade informadas

### Requirement: Consulta sem correspondências

A API SHALL retornar HTTP 200 com `items: []` e `totalCount: 0` quando a consulta não encontrar cursos, inclusive para combinações incompatíveis ou identificadores de classificação válidos em formato, mas inexistentes.

#### Scenario: Termo sem correspondência

- **WHEN** nenhum curso corresponde ao termo pesquisado
- **THEN** a API retorna uma coleção vazia e quantidade zero

#### Scenario: Critérios sem interseção

- **WHEN** há cursos para cada filtro isolado, mas nenhum atende à combinação solicitada
- **THEN** a API retorna uma coleção vazia e quantidade zero

#### Scenario: Classificação inexistente

- **WHEN** um identificador de classificação informado não existe no catálogo
- **THEN** a API retorna uma coleção vazia e quantidade zero, sem tratar isso como erro de recurso de curso

### Requirement: Detalhes completos do curso

A API SHALL disponibilizar `GET /api/courses/{id}` com HTTP 200 para um curso existente. A resposta deve conter identificador, nomes, descrição, categoria, modalidade, carga horária, público-alvo, status, imagem, data de disponibilização opcional e módulos com suas aulas. Módulos e aulas devem incluir identificador, título, descrição e ordem, apresentados por essa ordem.

#### Scenario: Curso com módulos e aulas

- **WHEN** o consumidor consulta o identificador de um curso existente
- **THEN** recebe todos os dados do curso e suas classificações
- **AND** recebe os módulos em ordem de apresentação e as aulas de cada módulo na respectiva ordem
- **AND** não recebe campos internos de persistência ou referências circulares

#### Scenario: Curso sem data de disponibilização

- **WHEN** um curso existente não possui data de disponibilização
- **THEN** a API retorna seus detalhes normalmente e representa a data opcional como `null`

#### Scenario: Estrutura educacional vazia

- **WHEN** um curso não possui módulos ou um módulo não possui aulas
- **THEN** a API representa a coleção correspondente como vazia, sem impor cardinalidade mínima não prevista

### Requirement: Erros de consulta consistentes

A API SHALL retornar HTTP 404 para curso inexistente e HTTP 400 para parâmetros que não possam ser interpretados no formato esperado. Esses erros devem utilizar respostas JSON Problem Details, sem revelar credenciais ou detalhes internos de exceções.

#### Scenario: Identificador de curso inexistente

- **WHEN** o consumidor consulta um identificador de curso bem formado, mas não cadastrado
- **THEN** recebe HTTP 404 com Problem Details

#### Scenario: Identificador inválido na consulta

- **WHEN** o consumidor informa `categoryId=abc` ou um identificador de curso incompatível com o formato aceito
- **THEN** recebe HTTP 400 com Problem Details
- **AND** o erro não é convertido em uma listagem sem filtros

### Requirement: Catálogo sem operações de escrita

A API SHALL expor somente operações de leitura para o catálogo. Não deve disponibilizar cadastro, edição ou exclusão de cursos ou de suas classificações e estrutura educacional.

#### Scenario: Tentativa de cadastrar curso

- **WHEN** o consumidor envia `POST /api/courses`
- **THEN** a operação não é disponibilizada, retorna HTTP 405 e não altera os dados

#### Scenario: Tentativa de editar ou excluir curso

- **WHEN** o consumidor envia `PUT`, `PATCH` ou `DELETE` para `/api/courses/{id}`
- **THEN** a operação não é disponibilizada, retorna HTTP 405 e não altera os dados
