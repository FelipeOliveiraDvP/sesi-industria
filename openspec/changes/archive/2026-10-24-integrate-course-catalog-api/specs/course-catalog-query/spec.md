# Spec Delta

## MODIFIED Requirements

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

## ADDED Requirements

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
