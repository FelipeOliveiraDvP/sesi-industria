# Spec Delta

## MODIFIED Requirements

### Requirement: Informações persistidas do curso

O catálogo SHALL persistir identificador único, nomes completo e breve, descrições completa e breve, categoria, modalidade, carga horária, público-alvo, status e referência da imagem de cada curso. Esses dados são obrigatórios; somente a data de disponibilização é opcional. A descrição completa existente MUST ser preservada como conceito distinto da descrição breve do card.

#### Scenario: Persistência e recuperação das informações

- **WHEN** um curso do conjunto inicial é consultado após a preparação do banco
- **THEN** todas as informações obrigatórias estão persistidas e podem ser recuperadas
- **AND** seu identificador o distingue dos demais cursos
- **AND** a descrição breve preserva o texto do card documentado e a descrição completa apresenta o curso sem depender de texto local do frontend

#### Scenario: Data opcional

- **WHEN** um curso é persistido sem data de disponibilização
- **THEN** a ausência da data é aceita sem comprometer os demais dados obrigatórios

#### Scenario: Informação obrigatória ausente

- **WHEN** uma gravação de preparação de dados omite um campo obrigatório
- **THEN** a persistência rejeita a ausência em vez de armazenar um registro incompleto

### Requirement: Conjunto inicial fictício com diversidade mínima

A inicialização SHALL disponibilizar os seis cursos industriais da Landing, pelo menos três categorias, duas modalidades e três status, com dados fictícios, associações válidas e diversidade de cargas horárias, públicos, módulos e aulas. Os seis cursos MUST manter os dois badges documentados; a terceira categoria ficará cadastrada sem cursos neste conjunto inicial.

#### Scenario: Catálogo inicial preparado

- **WHEN** a inicialização é concluída em um banco vazio
- **THEN** o catálogo contém as quantidades mínimas de cursos e classificações
- **AND** os cursos possuem cargas horárias e públicos-alvo variados
- **AND** existem módulos e aulas com títulos, descrições e organização variados entre os cursos
- **AND** todos os dados iniciais são fictícios e possuem relacionamentos válidos

#### Scenario: Cursos e badges correspondentes à Landing

- **WHEN** os cursos do novo seed são recuperados
- **THEN** estão presentes Matemática para a Precisão: Medidas e Escalas na Metalurgia, Geometria e Modelagem no Design Têxtil e Estatística e Controle de Qualidade na Produção, associados a Matemática
- **AND** estão presentes Física do Movimento e da Energia na Metalurgia, Química e Sustentabilidade e Ciência dos Materiais na Metalurgia, associados a Ciências da Natureza
- **AND** os títulos e as descrições breves correspondem integralmente à documentação da Landing
- **AND** a terceira categoria não exige um sétimo card nem a reclassificação dos seis cursos

#### Scenario: Conteúdo de referência e dados inferidos

- **WHEN** o curso Matemática para a Precisão é recuperado
- **THEN** sua descrição completa, dez módulos com tópicos e conteúdo demonstrativo reproduzem as transcrições de Detalhes e AVA
- **AND** sua data de disponibilização no seed é `2026-01-05`
- **AND** os demais cursos possuem conteúdo fictício próprio e metadados constantes compatíveis com capacitação industrial, incluindo datas preenchidas e nulas

### Requirement: Dados iniciais independentes de serviços externos

A preparação do seed SHALL funcionar sem geração, downloads ou consulta a serviços externos, inclusive sem o frontend em execução. As imagens MUST ser referências aos assets estáticos existentes em `apps/frontend/public/assets`, com caminhos públicos sem `public` e sem host de ambiente persistido; sua exibição dependerá do servidor estático do frontend.

#### Scenario: Inicialização sem integrações externas

- **WHEN** o PostgreSQL e as dependências necessárias já estão disponíveis localmente e o seed é executado com o frontend desligado
- **THEN** o conjunto inicial é carregado usando somente os dados e referências incluídos no projeto
- **AND** nenhuma imagem é baixada, copiada ou consultada por HTTP durante o seed

#### Scenario: Referências públicas dos assets

- **WHEN** as referências de imagens do seed são inspecionadas
- **THEN** apontam para arquivos existentes sob `apps/frontend/public/assets`
- **AND** seus caminhos públicos começam com `/assets/`, sem incluir o diretório `public`, uma URL absoluta ou o diretório de trabalho da máquina

#### Scenario: Mesmo seed em ambientes diferentes

- **WHEN** dois bancos vazios são preparados para frontends hospedados em endereços diferentes
- **THEN** recebem os mesmos dados, identificadores e caminhos relativos de imagens
- **AND** a diferença de host é resolvida apenas ao produzir as respostas HTTP

## ADDED Requirements

### Requirement: Apresentação específica de Detalhes e AVA persistida

Cada curso do seed SHALL possuir apresentação recuperável para Detalhes e AVA: certificação, imagens, informações de aulas e avaliações, perguntas e respostas, estrutura lateral demonstrativa e artigo com recomendações. Esse conteúdo MUST pertencer ao curso consultado e representar uma prévia, sem matrícula ou progresso de usuário persistidos.

#### Scenario: Apresentações diferentes por curso

- **WHEN** as apresentações de dois cursos distintos são recuperadas após a preparação do banco
- **THEN** cada uma acompanha o identificador correto e contém descrição, módulos e textos de leitura coerentes com seu assunto
- **AND** os cursos não compartilham por engano o artigo específico de Matemática

#### Scenario: Estado demonstrativo reproduzível

- **WHEN** o seed é repetido ou preparado em outro banco vazio
- **THEN** a apresentação preserva textos, imagens, ordem das listas e estados demonstrativos constantes
- **AND** nenhum registro de aluno, inscrição, conclusão ou avanço é criado

#### Scenario: Estrutura de conteúdo e sidebar com finalidades distintas

- **WHEN** o curso de Matemática é recuperado
- **THEN** seus módulos e aulas relacionais preservam os dez módulos documentados nos Detalhes
- **AND** a apresentação do AVA preserva Introdução e as seis linhas recolhidas da referência, sem alegar que sejam uma segunda estrutura relacional de aulas
