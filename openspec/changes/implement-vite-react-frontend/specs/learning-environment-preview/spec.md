# Spec Delta

## Purpose

Apresentar o ambiente de aprendizagem demonstrativo do curso documentado, permitindo ler integralmente Boas práticas e Recomendações e retornar aos Detalhes sem simular progresso ou aulas adicionais.

## ADDED Requirements

### Requirement: Layout próprio do ambiente de aprendizagem

O AVA SHALL apresentar sidebar lateral e coluna de leitura, sem header ou footer de marketing. Em desktop, a sidebar SHALL permanecer fixa enquanto o artigo rola. Sua área de identificação e progresso SHALL ser escura e a área inferior de busca e módulos clara, conforme as capturas locais.

#### Scenario: Rolagem independente do artigo

- **WHEN** o usuário abre `/ava/matematica-para-a-precisao` e rola o conteúdo principal até o fim
- **THEN** a posição da sidebar permanece estável em desktop
- **AND** a coluna de leitura exibe o final de Recomendações sem rolagem horizontal
- **AND** não são exibidas as seções comerciais da Landing ou dos Detalhes

#### Scenario: Navegação lateral em tela pequena

- **WHEN** o AVA é aberto em uma largura incompatível com sidebar e artigo lado a lado
- **THEN** o conteúdo lateral permanece acessível por navegação adaptada com abertura e fechamento locais
- **AND** a leitura do artigo e o retorno aos Detalhes continuam disponíveis sem conteúdo encoberto

### Requirement: Identificação e progresso demonstrativos

A sidebar SHALL apresentar Certificação, o título integral do curso e progresso fixo de 10%. A indicação SHALL ser acessível como progresso demonstrativo e não representar matrícula, conclusão ou sincronização de um usuário real.

#### Scenario: Estado inicial de progresso

- **WHEN** o usuário entra no AVA por acesso direto ou por qualquer CTA de matrícula
- **THEN** visualiza o mesmo título de curso e o indicador de 10%
- **AND** não precisa de login ou de inscrição prévia

#### Scenario: Ausência de avanço fictício

- **WHEN** o usuário percorre o artigo, encontra os itens laterais ou atualiza a página
- **THEN** o progresso permanece em 10%
- **AND** nenhum registro de conclusão ou progresso é criado em servidor ou armazenamento persistente

### Requirement: Estrutura lateral correspondente ao estado documentado

A sidebar SHALL exibir Introdução expandida com 1.1 Introdução concluída, 1.2 Boas práticas como lição atual e 1.3 Extras pendente, além de Módulo 1 a Módulo 6 visualmente recolhidos. Busca e itens sem interação confirmada SHALL permanecer ilustrativos e acessivelmente indisponíveis, sem filtrar ou trocar a lição.

#### Scenario: Conferência dos itens e estados

- **WHEN** o usuário examina a navegação lateral
- **THEN** encontra os três itens de Introdução com os ícones de check, play e círculo vazio documentados
- **AND** encontra as seis linhas numeradas de módulos seguintes, de `2. Módulo 1` a `7. Módulo 6`
- **AND** o campo visual de busca conserva a lupa e não acrescenta placeholder ou resultados inexistentes na referência

#### Scenario: Ausência de conteúdo de lições não documentadas

- **WHEN** o usuário encontra Introdução, Extras, um módulo recolhido ou o campo de busca
- **THEN** esses elementos não abrem novas rotas, conteúdos, resultados ou estados de conclusão
- **AND** sua indisponibilidade é indicada de forma acessível sem criar fluxos alternativos

### Requirement: Artigo integral de Boas práticas e Recomendações

A coluna principal SHALL apresentar o banner local de medição com marca SESI, o título Boas práticas com seus três parágrafos e Recomendações com seus quatro parágrafos, integralmente conforme `docs/figma-reverse-engineering/pages/003-ava.md`. O conteúdo SHALL permanecer textual, selecionável e sem truncamento.

#### Scenario: Leitura até o último parágrafo

- **WHEN** o usuário percorre a coluna principal do começo ao fim
- **THEN** visualiza os sete parágrafos documentados, incluindo a recomendação final sobre conexão com outros alunos e profissionais
- **AND** o banner preserva o recorte panorâmico e a marca sobreposta sem carregar recursos remotos
- **AND** o texto não é substituído por uma imagem da tela ou por conteúdo genérico de aula

### Requirement: Retorno determinístico aos Detalhes

A seta de retorno do AVA SHALL abrir `/cursos/matematica-para-a-precisao`, inclusive quando o AVA tiver sido aberto diretamente. O retorno SHALL dispensar histórico prévio e não apresentar confirmação de salvamento, matrícula ou progresso.

#### Scenario: Voltar após entrada direta no AVA

- **WHEN** o usuário abre a URL do AVA em uma nova aba e aciona a seta de retorno
- **THEN** a aplicação exibe os Detalhes do curso na URL aprovada
- **AND** não depende de voltar para um endereço anterior do navegador

#### Scenario: Ciclo entre Detalhes e AVA

- **WHEN** o usuário entra no AVA por Matricule-se, retorna aos Detalhes e entra novamente
- **THEN** o mesmo artigo e os mesmos estados demonstrativos são apresentados
- **AND** não aparece indicação de matrícula realizada ou progresso atualizado
