# Spec Delta

## Purpose

Preservar os dados e os relacionamentos do catálogo educacional em armazenamento relacional durável, com evolução de esquema e um conjunto inicial fictício que possa ser reproduzido sem duplicações.

## ADDED Requirements

### Requirement: Informações persistidas do curso

O catálogo SHALL persistir para cada curso identificador único, nome completo, nome breve, descrição, categoria, modalidade, carga horária, público-alvo, status e imagem de apresentação. Esses dados são obrigatórios; somente a data de disponibilização é opcional.

#### Scenario: Persistência e recuperação das informações

- **WHEN** um curso do conjunto inicial é consultado após a preparação do banco
- **THEN** todas as informações obrigatórias estão persistidas e podem ser recuperadas
- **AND** seu identificador o distingue dos demais cursos

#### Scenario: Data opcional

- **WHEN** um curso é persistido sem data de disponibilização
- **THEN** a ausência da data é aceita sem comprometer os demais dados obrigatórios

#### Scenario: Informação obrigatória ausente

- **WHEN** uma gravação de preparação de dados omite um campo obrigatório
- **THEN** a persistência rejeita a ausência em vez de armazenar um registro incompleto

### Requirement: Classificações com integridade referencial

O catálogo SHALL representar categoria, modalidade e status como classificações com identificador e nome. Cada curso deve estar associado a exatamente uma classificação válida de cada tipo, sem aceitar referências inexistentes ou nulas.

#### Scenario: Curso associado às classificações

- **WHEN** um curso persistido é recuperado
- **THEN** sua categoria, modalidade e status possuem identificadores e nomes válidos

#### Scenario: Referência de classificação inexistente

- **WHEN** uma operação de preparação tenta persistir um curso associado a categoria, modalidade ou status inexistente
- **THEN** o banco rejeita a operação por integridade referencial

### Requirement: Estrutura educacional ordenada

O catálogo SHALL representar módulos associados a um único curso e aulas associadas a um único módulo. Cada módulo e aula deve possuir identificador, título, descrição e ordem de apresentação. Cursos podem possuir vários módulos e módulos podem possuir várias aulas, sem impor quantidade mínima a cada associação.

#### Scenario: Recuperação da estrutura do curso

- **WHEN** um curso com vários módulos e aulas é recuperado
- **THEN** cada módulo pertence ao curso correto e cada aula ao módulo correto
- **AND** títulos, descrições e ordens de apresentação são preservados

#### Scenario: Referência educacional inexistente

- **WHEN** uma operação tenta persistir módulo sem curso válido ou aula sem módulo válido
- **THEN** o banco rejeita a operação por integridade referencial

### Requirement: Persistência PostgreSQL com esquema versionado

O backend SHALL armazenar os dados do catálogo no PostgreSQL e permitir preparar um banco vazio por meio das migrations versionadas do projeto. Reaplicar as migrations já executadas não deve recriar estruturas nem apagar dados existentes.

#### Scenario: Inicialização de banco vazio

- **WHEN** o operador executa o comando documentado de atualização do banco em uma instância PostgreSQL vazia
- **THEN** o esquema necessário ao catálogo é criado com chaves e relacionamentos válidos
- **AND** o banco fica apto a atender às consultas da API

#### Scenario: Migrations já aplicadas

- **WHEN** o operador repete o comando de atualização sem novas migrations
- **THEN** o esquema permanece utilizável e os dados existentes são preservados

### Requirement: Conjunto inicial fictício com diversidade mínima

A inicialização SHALL disponibilizar pelo menos seis cursos, três categorias distintas, duas modalidades distintas e três status distintos, todos fictícios. Os cursos devem variar em carga horária, público-alvo, módulos e descrições de aulas, mantendo todas as associações válidas.

#### Scenario: Catálogo inicial preparado

- **WHEN** a inicialização é concluída em um banco vazio
- **THEN** o catálogo contém as quantidades mínimas de cursos e classificações
- **AND** os cursos possuem cargas horárias e públicos-alvo variados
- **AND** existem módulos e aulas com títulos, descrições e organização variados entre os cursos
- **AND** todos os dados iniciais são fictícios e possuem relacionamentos válidos

### Requirement: Seed determinístico e idempotente

A inicialização SHALL utilizar um conjunto de dados local e estável. Executá-la repetidamente deve preservar identificadores e valores do conjunto inicial, sem duplicar cursos, classificações, módulos ou aulas e sem gerar dados aleatórios ou dependentes do momento da execução.

#### Scenario: Repetição da inicialização

- **WHEN** o seed é executado duas vezes no mesmo banco já inicializado
- **THEN** as quantidades de registros de todas as entidades permanecem iguais
- **AND** os identificadores, valores e relacionamentos do conjunto inicial permanecem estáveis

#### Scenario: Reprodução em outro banco vazio

- **WHEN** o seed é executado em dois bancos vazios independentes
- **THEN** ambos recebem o mesmo conjunto inicial de identificadores, valores e relacionamentos

### Requirement: Dados iniciais independentes de serviços externos

A preparação dos dados iniciais SHALL funcionar sem serviços de geração, downloads ou integrações externas, após a obtenção das dependências de desenvolvimento e da imagem do banco. As imagens de apresentação do conjunto inicial devem ter referências utilizáveis com os recursos locais entregues pelo backend.

#### Scenario: Inicialização sem integrações externas

- **WHEN** o PostgreSQL e as dependências necessárias já estão disponíveis localmente e o seed é executado
- **THEN** o conjunto inicial é carregado usando somente os dados e recursos incluídos no projeto
- **AND** as imagens de apresentação não dependem de um serviço remoto para serem utilizadas
