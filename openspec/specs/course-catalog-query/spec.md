# course-catalog-query Specification

## Purpose

Permitir a consulta pública e somente leitura do catálogo educacional, com pesquisa, classificações combináveis, contagem de resultados e detalhes da estrutura de módulos e aulas.

## Requirements

### Requirement: Listagem de cursos com classificações e quantidade

A API SHALL disponibilizar `GET /api/courses` com HTTP 200, `items` e `totalCount`. Cada item MUST conter identificador, nomes completo e breve, `description`, `shortDescription`, classificações com ID e nome, carga horária, público-alvo, imagem e data opcional. A contagem corresponderá aos itens após todos os critérios, sem paginação ou ranking de destaque/popularidade.

#### Scenario: Consulta sem critérios

- **WHEN** o consumidor solicita `GET /api/courses` sem parâmetros
- **THEN** a API retorna todos os cursos do catálogo com seus identificadores, nomes e classificações
- **AND** cada item inclui descrições completa e breve, carga horária, público-alvo, imagem e data opcional
- **AND** `totalCount` é igual à quantidade de elementos de `items`
- **AND** cursos não são omitidos implicitamente por status ou data

#### Scenario: Cards sem consultas individuais adicionais

- **WHEN** o consumidor recebe a coleção de cursos
- **THEN** os campos retornados são suficientes para apresentar título, descrição breve, categoria e imagem de cada card
- **AND** não precisa consultar os detalhes de cada curso para completar a Landing

#### Scenario: Critérios existentes preservados

- **WHEN** a listagem recebe pesquisa e filtros de categoria, modalidade e status
- **THEN** mantém a aplicação cumulativa dos critérios e a contagem correspondente
- **AND** a introdução das descrições não altera a pesquisa pelos nomes completo e breve

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

A API SHALL disponibilizar `GET /api/courses/{id}` com HTTP 200 para curso existente, contendo os campos do resumo, módulos, aulas e `presentation`, um objeto tipado com o conteúdo específico de Detalhes e AVA. Módulos e aulas MUST incluir ID, título, descrição e ordem, apresentados por ordem e ID. A resposta não exporá entidades, navegações internas ou conteúdo serializado como texto opaco.

#### Scenario: Curso com módulos e aulas

- **WHEN** o consumidor consulta o identificador de um curso existente
- **THEN** recebe todos os dados do curso e suas classificações
- **AND** recebe os módulos em ordem de apresentação e as aulas de cada módulo na respectiva ordem, com desempate por ID
- **AND** não recebe campos internos de persistência ou referências circulares

#### Scenario: Curso sem data de disponibilização

- **WHEN** um curso existente não possui data de disponibilização
- **THEN** a API retorna seus detalhes normalmente e representa a data opcional como `null`

#### Scenario: Estrutura educacional vazia

- **WHEN** um curso não possui módulos ou um módulo não possui aulas
- **THEN** a API representa a coleção correspondente como vazia, sem impor cardinalidade mínima não prevista

#### Scenario: Conteúdo para todas as páginas do curso

- **WHEN** o consumidor consulta qualquer identificador dos seis cursos do seed
- **THEN** `presentation` inclui certificação, imagem de detalhes, informações de aulas e avaliações, perguntas e respostas e uma prévia do AVA com banner, sidebar, artigo e recomendações
- **AND** os dados são objetos, textos e coleções JSON tipados, sem HTML executável ou necessidade de outra API de conteúdo
- **AND** o mesmo detalhe permite abrir Detalhes ou AVA diretamente sem consulta prévia à listagem

#### Scenario: Prévia sem operações de aluno

- **WHEN** o consumidor lê os estados e o progresso da prévia do AVA
- **THEN** recebe valores demonstrativos constantes identificados como tal
- **AND** a consulta não cria matrícula, conclusão ou progresso de usuário

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

### Requirement: URLs públicas de imagens baseadas no frontend

As respostas da coleção e dos detalhes SHALL apresentar URLs HTTP ou HTTPS absolutas para as imagens de curso e apresentação, compostas a partir de `FRONTEND_URL` e dos caminhos públicos persistidos. A composição MUST preservar o asset relativo sem expor caminhos locais ou exigir sua cópia para o backend.

#### Scenario: Imagens servidas pelo frontend configurado

- **WHEN** `FRONTEND_URL` é `http://127.0.0.1:5173` e um curso referencia `/assets/banner-ava.png`
- **THEN** sua URL pública é `http://127.0.0.1:5173/assets/banner-ava.png`
- **AND** nenhum segmento `public`, `wwwroot` ou caminho absoluto do sistema de arquivos aparece na resposta

#### Scenario: Mudança de endereço sem alterar os dados

- **WHEN** a API é reiniciada com outro `FRONTEND_URL` válido sobre o mesmo banco
- **THEN** as respostas passam a utilizar a nova base para todas as imagens de apresentação
- **AND** não é necessário reexecutar o seed nem modificar as referências persistidas

#### Scenario: Separação dos contratos de listagem e detalhes

- **WHEN** a listagem e os detalhes do mesmo curso são comparados
- **THEN** os campos comuns preservam os mesmos valores e a mesma URL de imagem do card
- **AND** as imagens próprias de Detalhes e AVA são retornadas dentro da apresentação sem substituir a identidade da imagem do card

### Requirement: Aulas genéricas tipadas na prévia do AVA

O detalhe do curso SHALL incluir, em `presentation.learningPreview.modules`, módulos ordenados com suas aulas ordenadas. Cada aula SHALL expor rótulo e descrição em texto simples; essa coleção demonstrativa SHALL ser idêntica para os seis cursos e distinta de `modules`, que permanece a estrutura relacional específica de Detalhes.

#### Scenario: Consulta do detalhe com aulas do AVA

- **WHEN** o consumidor consulta `GET /api/courses/{id}` para qualquer um dos seis cursos
- **THEN** cada módulo da prévia contém duas aulas com rótulo e descrição
- **AND** a ordem de módulos e aulas é estável
- **AND** a resposta preserva os módulos relacionais específicos do curso no campo superior `modules`

#### Scenario: Conteúdo uniforme entre identificadores

- **WHEN** o consumidor compara `presentation.learningPreview.modules` de dois ou mais cursos
- **THEN** rótulos, descrições, quantidades e ordens dos módulos e aulas da prévia são iguais
- **AND** nenhuma matrícula, conclusão ou progresso de usuário é incluído
