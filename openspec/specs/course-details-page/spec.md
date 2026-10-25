# course-details-page Specification

## Purpose

Permitir consultar a apresentação integral do curso de Matemática para a Precisão, expandir suas informações e acessar a demonstração do AVA sem executar uma matrícula ou outra operação remota.

## Requirements

### Requirement: Apresentação visual do curso documentado

A página de Detalhes SHALL apresentar o curso Matemática para a Precisão: Medidas e Escalas na Metalurgia com badge Matemática, início em `05/01/2026`, certificação SESI, CTA Matricule-se e thumbnail local. Header, hero em duas colunas, benefícios, informações, depoimentos, contato e footer SHALL seguir a ordem da referência.

#### Scenario: Entrada direta na página do curso

- **WHEN** o usuário abre `/cursos/matematica-para-a-precisao`
- **THEN** encontra o título integral, a data estática da referência, a certificação e o CTA de matrícula
- **AND** visualiza a mesma imagem de curso usada pelo card da Landing, recebida em `imageUrl`, na proporção documentada
- **AND** o play permanece apenas ilustrativo, sem reproduzir vídeo externo
- **AND** consegue percorrer todas as seções da página até o footer

#### Scenario: Curso de referência

- **WHEN** o usuário abre os detalhes de Matemática, inclusive pela URL antiga
- **THEN** encontra o título integral documentado, data `05/01/2026`, certificação SESI e CTA Matricule-se
- **AND** os dados são recuperados da API, não do mock antigo

#### Scenario: Data opcional

- **WHEN** o curso retorna `availableOn: null`
- **THEN** o campo de data informa A definir sem inventar uma data
- **AND** datas preenchidas são formatadas em dia, mês e ano sem deslocamento por fuso horário

### Requirement: Quatro benefícios e descrição do curso

A página SHALL preservar os quatro benefícios institucionais, seus ícones e descrições documentados. Sobre o curso MUST apresentar a descrição completa retornada para o identificador atual, mantendo o texto integral de referência para Matemática e conteúdo próprio para os demais cursos.

#### Scenario: Conteúdo introdutório completo

- **WHEN** o usuário percorre a faixa de benefícios e Sobre o curso
- **THEN** encontra Certificação SESI, Professores especialistas, Suporte contínuo e Flexibilidade com suas descrições
- **AND** lê a descrição completa do curso selecionado
- **AND** a apresentação preserva a relação visual entre benefícios, descrição e coluna expansível

#### Scenario: Descrição breve não substitui apresentação completa

- **WHEN** o curso possui descrições breve e completa distintas
- **THEN** Sobre o curso usa a descrição completa da API
- **AND** não repete somente o resumo do card nem o texto de outro curso

### Requirement: Três seções expansíveis independentes

A página SHALL disponibilizar Módulos, Aulas online e avaliações e Dúvidas frequentes inicialmente recolhidos. Cada seção SHALL expandir e recolher inline, comunicar seu estado e permitir que as três permaneçam abertas simultaneamente, sem modal ou troca de rota.

#### Scenario: Expansão simultânea

- **WHEN** o usuário abre Módulos, depois Aulas online e avaliações e depois Dúvidas frequentes
- **THEN** os três conteúdos permanecem expandidos e aumentam a altura da página
- **AND** os controles indicam visual e semanticamente o estado aberto

#### Scenario: Recolhimento independente

- **WHEN** o usuário fecha uma das seções abertas por mouse ou teclado
- **THEN** somente o conteúdo correspondente é recolhido
- **AND** o foco permanece utilizável e os outros conteúdos continuam abertos

### Requirement: Conteúdo integral dos acordeões

Módulos SHALL apresentar os módulos e as aulas do curso atual na ordem recebida. Aulas online e avaliações e Dúvidas frequentes MUST usar a apresentação retornada pela API, preservando suas estruturas visuais. Matemática manterá os dez módulos, tópicos, informações e dez FAQs transcritos; os demais cursos terão conteúdo próprio compatível com sua modalidade.

#### Scenario: Conferência dos módulos e perguntas

- **WHEN** o usuário expande Módulos e Dúvidas frequentes do curso de Matemática até o fim
- **THEN** encontra os dez módulos, de Introdução à Matemática na Metalurgia a Avaliação Final e Certificação, com todos os tópicos documentados
- **AND** encontra as dez perguntas e respostas transcritas
- **AND** não perde conteúdo por truncamento, altura fixa ou sobreposição da barra de matrícula

#### Scenario: Leitura das informações de aulas e avaliações

- **WHEN** o usuário expande Aulas online e avaliações
- **THEN** consegue ler os parágrafos, listas e subtítulos enviados para aquele curso
- **AND** as menções a fóruns, provas, antifraude e certificado permanecem conteúdo informativo, sem implementar essas funcionalidades
- **AND** informações de cursos presenciais não afirmam indevidamente que toda a formação ocorre online

#### Scenario: Conteúdo de outro curso

- **WHEN** qualquer um dos outros cursos do seed é aberto
- **THEN** módulos, tópicos e informações específicas pertencem ao identificador consultado
- **AND** não são preenchidos pela lista fixa de Matemática
- **AND** coleções educacionais vazias são apresentadas de forma reconhecível, sem criar módulos ou aulas fictícios

### Requirement: Barra de matrícula condicionada à rolagem

A barra SHALL permanecer oculta no topo e durante carregamento inicial ou falha sem dados. Com o curso carregado, aparecerá após ultrapassar o hero, exibirá o título e a ação do mesmo curso e desaparecerá ao retornar ao topo. MUST permanecer acessível e não impedir acesso ao conteúdo inferior, inclusive após trocas de rota e carregamento assíncrono.

#### Scenario: Alternância da barra fixa

- **WHEN** o usuário parte do topo, percorre o conteúdo após o hero e depois retorna ao topo
- **THEN** a barra é exibida durante a leitura inferior e removida ao retornar à região inicial
- **AND** o header dos Detalhes rola com o documento

#### Scenario: Conteúdo final alcançável

- **WHEN** o usuário alcança os últimos itens dos acordeões e o footer com a barra visível
- **THEN** consegue visualizar e alcançar esse conteúdo, inclusive a linha inferior do footer
- **AND** a barra adapta título e botão à largura disponível sem provocar overflow do documento

#### Scenario: Carregamento e troca do curso

- **WHEN** o carregamento termina ou a rota muda para outro curso
- **THEN** a barra passa a acompanhar o limite do hero e a identidade do curso atual
- **AND** não conserva título, link ou estado visível de um curso anterior

### Requirement: Matrícula demonstrativa e retorno à Landing

Matricule-se no hero e na barra SHALL abrir `/ava/{id}` com o identificador do curso atual, independentemente do status demonstrativo. Início retornará à Landing. Essas transições MUST NOT inscrever, autenticar, solicitar dados pessoais ou confirmar matrícula; consultas GET para carregar o AVA são permitidas.

#### Scenario: Entrada no AVA pelas duas CTAs

- **WHEN** o usuário aciona Matricule-se no hero ou na barra fixa de qualquer curso do seed
- **THEN** a aplicação abre o AVA daquele identificador
- **AND** apresenta a prévia demonstrativa específica com progresso constante de 10%, sem operação de escrita

#### Scenario: Retorno à apresentação inicial

- **WHEN** o usuário aciona Início no header dos Detalhes
- **THEN** a aplicação navega para `/` e apresenta a Landing

#### Scenario: Status não transforma a prévia em matrícula

- **WHEN** o curso do seed está disponível, em breve ou encerrado
- **THEN** a ação documentada continua permitindo consultar sua prévia
- **AND** o status não produz cobrança, elegibilidade de inscrição ou confirmação fictícia

### Requirement: Depoimentos e contato visual dos Detalhes

A página SHALL apresentar os cinco depoimentos documentados em cards brancos sobre superfície pêssego e permitir percorrê-los localmente. A CTA Precisa de ajuda para decidir? / Fale com a gente SHALL preservar sua apresentação; Mandar uma mensagem SHALL permanecer indisponível sem abrir formulário ou enviar comunicação.

#### Scenario: Depoimentos com variante visual própria

- **WHEN** o usuário percorre a seção Quem fez recomenda nos Detalhes
- **THEN** encontra as cinco citações, autores, cargos e estrelas documentados
- **AND** consegue avançar e retornar pelo carrossel sem sair da página
- **AND** a variante clara da seção difere da variante vermelha da Landing

#### Scenario: Contato sem operação externa

- **WHEN** o usuário encontra a CTA Fale com a gente
- **THEN** visualiza o título e o botão Mandar uma mensagem com indicação acessível de indisponibilidade
- **AND** nenhuma mensagem, modal de contato ou confirmação fictícia é produzida
