# Spec Delta

## Purpose

Permitir consultar a apresentação integral do curso de Matemática para a Precisão, expandir suas informações e acessar a demonstração do AVA sem executar uma matrícula ou outra operação remota.

## ADDED Requirements

### Requirement: Apresentação visual do curso documentado

A página de Detalhes SHALL apresentar o curso Matemática para a Precisão: Medidas e Escalas na Metalurgia com badge Matemática, início em `05/01/2026`, certificação SESI, CTA Matricule-se e thumbnail local. Header, hero em duas colunas, benefícios, informações, depoimentos, contato e footer SHALL seguir a ordem da referência.

#### Scenario: Entrada direta na página do curso

- **WHEN** o usuário abre `/cursos/matematica-para-a-precisao`
- **THEN** encontra o título integral, a data estática da referência, a certificação e o CTA de matrícula
- **AND** visualiza a thumbnail na proporção documentada, com play apenas ilustrativo e sem vídeo externo
- **AND** consegue percorrer todas as seções da página até o footer

### Requirement: Quatro benefícios e descrição do curso

A página SHALL apresentar Certificação SESI, Professores especialistas, Suporte contínuo e Flexibilidade com ícones locais na variante pêssego e descrições documentadas. A seção Sobre o curso SHALL preservar integralmente o texto de apresentação de `docs/figma-reverse-engineering/pages/002-detalhes-curso.md`.

#### Scenario: Conteúdo introdutório completo

- **WHEN** o usuário percorre a faixa de benefícios e Sobre o curso
- **THEN** encontra os quatro benefícios, suas descrições e a explicação completa sobre medidas e escalas na metalurgia
- **AND** a apresentação preserva as relações visuais entre benefícios, descrição e coluna de informações expansíveis

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

Módulos SHALL conter os dez módulos e seus tópicos; Aulas online e avaliações SHALL conter os textos, listas e subtítulos Aulas online e Provas e Avaliações; Dúvidas frequentes SHALL conter as dez perguntas e respostas. Todo esse conteúdo SHALL corresponder à transcrição dos Detalhes, sem substituição por resumos ou textos genéricos.

#### Scenario: Conferência dos módulos e perguntas

- **WHEN** o usuário expande Módulos e Dúvidas frequentes e percorre seus conteúdos até o fim
- **THEN** encontra os dez módulos, de Introdução à Matemática na Metalurgia a Avaliação Final e Certificação, com todos os tópicos transcritos
- **AND** encontra as dez perguntas e respostas, da modalidade online ao reconhecimento por órgãos oficiais
- **AND** não perde o conteúdo final por truncamento, altura fixa ou sobreposição da barra de matrícula

#### Scenario: Leitura das informações de aulas e avaliações

- **WHEN** o usuário expande Aulas online e avaliações
- **THEN** consegue ler todos os parágrafos e itens sobre mídia, materiais, atividades, acessibilidade, provas, estudos de caso e certificação
- **AND** as menções textuais a fóruns, avaliações, antifraude e certificado permanecem conteúdo informativo da referência, sem implementar essas funcionalidades

### Requirement: Barra de matrícula condicionada à rolagem

A barra de matrícula SHALL estar ausente no estado inicial no topo e surgir após o usuário ultrapassar a área do hero. Ela SHALL permanecer acessível durante a rolagem, exibir o título do curso e Matricule-se, desaparecer ao retornar ao topo e não impedir a leitura ou o acesso ao conteúdo inferior.

#### Scenario: Alternância da barra fixa

- **WHEN** o usuário parte do topo, percorre o conteúdo após o hero e depois retorna ao topo
- **THEN** a barra é exibida durante a leitura inferior e removida ao retornar à região inicial
- **AND** o header dos Detalhes rola com o documento

#### Scenario: Conteúdo final alcançável

- **WHEN** o usuário alcança os últimos itens dos acordeões e o footer com a barra visível
- **THEN** consegue visualizar e alcançar esse conteúdo, inclusive a linha inferior do footer
- **AND** a barra adapta título e botão à largura disponível sem provocar overflow do documento

### Requirement: Matrícula demonstrativa e retorno à Landing

Matricule-se no hero e na barra fixa SHALL abrir `/ava/matematica-para-a-precisao` no mesmo estado demonstrativo. Início no header SHALL retornar à Landing. Essas transições SHALL NOT criar inscrição, solicitar dados pessoais, autenticar o usuário ou apresentar confirmação de matrícula.

#### Scenario: Entrada no AVA pelas duas CTAs

- **WHEN** o usuário aciona Matricule-se no hero ou na barra fixa
- **THEN** a aplicação navega para `/ava/matematica-para-a-precisao`
- **AND** apresenta o mesmo conteúdo demonstrativo com progresso de 10%, sem operação de servidor

#### Scenario: Retorno à apresentação inicial

- **WHEN** o usuário aciona Início no header dos Detalhes
- **THEN** a aplicação navega para `/` e apresenta a Landing

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
