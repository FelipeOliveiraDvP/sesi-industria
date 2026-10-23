# course-discovery-page Specification

## Purpose

Permitir conhecer a proposta de capacitação SESI Indústria e percorrer os cursos e depoimentos apresentados na Landing, com conteúdo demonstrativo integral e navegação apenas para destinos documentados.

## Requirements

### Requirement: Landing com conteúdo integral e ordem documentada

A Landing SHALL apresentar header, hero industrial, Principais cursos, Impacto transformador, depoimentos, CTA de matrícula e footer, nesta ordem. Textos e hierarquia SHALL corresponder a `docs/figma-reverse-engineering/pages/001-landing-page.md`, sem reduzir a entrega à primeira dobra ou inserir seções de negócio adicionais.

#### Scenario: Leitura completa da página inicial

- **WHEN** o usuário abre `/` e percorre a página até o fim
- **THEN** encontra o hero com `Impulsione sua carreira na indústria`, descrição e `Conhecer cursos`
- **AND** encontra todas as seções subsequentes, inclusive a CTA `Pronto para transformar sua carreira?` e o footer completo
- **AND** a fotografia do hero, sua sobreposição vermelha e a thumbnail da CTA utilizam os arquivos locais correspondentes

### Requirement: Apresentação dos seis cursos de destaque

A seção Principais cursos SHALL conter os seis cursos documentados, na mesma ordem, com títulos e descrições integrais, badges de Matemática nos três primeiros e Ciências da Natureza nos três seguintes. O trilho SHALL terminar com o card visual Todos os cursos, sem incluir outros cursos ou atribuir novos destinos.

#### Scenario: Conteúdo dos cards

- **WHEN** o usuário percorre todo o trilho de cursos
- **THEN** encontra `Matemática para a Precisão: Medidas e Escalas na Metalurgia`, `Geometria e Modelagem no Design Têxtil` e `Estatística e Controle de Qualidade na Produção`
- **AND** encontra `Física do Movimento e da Energia na Metalurgia`, `Química e Sustentabilidade` e `Ciência dos Materiais na Metalurgia`
- **AND** cada card preserva o título, descrição e badge registrados na documentação, com texto claro e sobreposição vermelha sobre imagem local
- **AND** o card Todos os cursos e o botão Ver todos os cursos permanecem visíveis, sem abrir uma página de catálogo não documentada

### Requirement: Navegação local do carrossel de cursos

O carrossel de cursos SHALL permitir avançar e retornar pelo trilho com controles acessíveis, revelando os cards posteriores e a ação final. A navegação SHALL ser finita, não automática e sem duplicar cursos, gerar espaços vazios indevidos ou produzir rolagem horizontal no documento inteiro.

#### Scenario: Avanço e retorno entre os grupos de cards

- **WHEN** o usuário aciona a seta de avançar uma ou mais vezes
- **THEN** o trilho revela os cursos seguintes até Ciência dos Materiais e o card Todos os cursos
- **AND** ao acionar a seta de voltar consegue retornar ao grupo inicial
- **AND** o conteúdo mantém a ordem documentada em desktop e mobile

#### Scenario: Limites do trilho

- **WHEN** o usuário chega ao começo ou ao fim do carrossel
- **THEN** a ação correspondente ao limite não desloca o conteúdo para além dos cards
- **AND** o estado indisponível do controle é comunicado de forma acessível

### Requirement: Métricas e benefícios da apresentação institucional

A seção Impacto transformador SHALL apresentar o texto de apoio documentado, quatro métricas estáticas e os três benefícios Certificação SESI, Professores especialistas e Suporte contínuo, com suas descrições e ícones locais na variante vermelha.

#### Scenario: Conferência dos indicadores e benefícios

- **WHEN** o usuário alcança Impacto transformador
- **THEN** visualiza `25.000+` Profissionais Capacitados, `500+` Empresas parceiras, `4.8/5` Avaliação média e `2.5M+` Horas de capacitação
- **AND** visualiza os três benefícios completos abaixo da área de métricas
- **AND** os valores são conteúdo estático da referência, não estatísticas consultadas ou calculadas a partir de usuários reais

### Requirement: Depoimentos completos em carrossel

A seção Quem fez recomenda SHALL apresentar os cinco depoimentos documentados com citação, autor, cargo e cinco estrelas. A Landing SHALL usar cards vermelhos sobre vermelho profundo, com navegação local de avanço e retorno, sem avaliações recebidas de serviços externos.

#### Scenario: Percurso pelos cinco depoimentos

- **WHEN** o usuário percorre o carrossel de depoimentos com seus controles
- **THEN** consegue ler as citações completas de Lucas Pereira, Mariana Costa, Thiago Almeida, Fernanda Silva e Gabriel Santos
- **AND** os cargos e a classificação visual correspondem à documentação
- **AND** consegue retornar aos depoimentos iniciais sem alterar de página

### Requirement: Destinos confirmados da Landing

Cursos no header e Conhecer cursos SHALL levar à seção Principais cursos da Landing. O primeiro card e Matricule-se na CTA final SHALL abrir `/cursos/matematica-para-a-precisao`. As demais ações sem destino confirmado SHALL seguir a política de indisponibilidade acessível, sem criar fluxos adicionais.

#### Scenario: Acesso à seção de cursos

- **WHEN** o usuário aciona Cursos no header da Landing ou Conhecer cursos no hero
- **THEN** a seção Principais cursos é trazida para a área visível sem ser encoberta pelo header
- **AND** a tela permanece na Landing, admitindo apenas uma âncora local na URL

#### Scenario: Entrada nos detalhes do curso documentado

- **WHEN** o usuário aciona o primeiro card ou Matricule-se na CTA inferior
- **THEN** a aplicação navega para `/cursos/matematica-para-a-precisao` e apresenta os detalhes do curso
- **AND** não registra uma matrícula nem apresenta confirmação de operação remota

#### Scenario: Controles sem fluxo identificado

- **WHEN** o usuário encontra os outros cards, Todos os cursos, Ver todos os cursos, Sobre, Área do aluno ou o play da thumbnail
- **THEN** nenhum desses elementos abre página, login, player ou operação não documentados
- **AND** os controles indisponíveis e o conteúdo meramente ilustrativo são distinguíveis de ações funcionais por seus recursos de acessibilidade
