# learning-environment-preview Specification

## Purpose

Apresentar o ambiente de aprendizagem demonstrativo do curso documentado, permitindo ler integralmente Boas práticas e Recomendações e retornar aos Detalhes sem simular progresso ou aulas adicionais.

## Requirements

### Requirement: Layout próprio do ambiente de aprendizagem

O AVA SHALL preservar sidebar e coluna de leitura sem header ou footer de marketing, para qualquer curso existente em `/ava/{id}`. Em desktop, a sidebar ficará fixa com identificação escura e navegação clara, enquanto o artigo rola. A obtenção assíncrona dos dados MUST preservar essa composição em todos os estados de consulta.

#### Scenario: Rolagem independente do artigo

- **WHEN** o usuário abre o AVA de um curso existente e rola o conteúdo principal até o fim
- **THEN** a posição da sidebar permanece estável em desktop
- **AND** a coluna de leitura exibe o final das recomendações daquele curso sem rolagem horizontal
- **AND** não são exibidas as seções comerciais da Landing ou dos Detalhes

#### Scenario: Navegação lateral em tela pequena

- **WHEN** o AVA é aberto em uma largura incompatível com sidebar e artigo lado a lado
- **THEN** o conteúdo lateral permanece acessível pela adaptação de abertura e fechamento local
- **AND** a leitura do artigo e o retorno aos Detalhes continuam disponíveis sem conteúdo encoberto

### Requirement: Identificação e progresso demonstrativos

A sidebar SHALL usar a identificação do curso consultado e os valores demonstrativos recebidos da API, com progresso fixo de 10% no seed. A indicação MUST ser acessível como prévia e não representar matrícula, conclusão ou sincronização de usuário real. Dados de outro identificador não poderão preencher a tela durante uma transição.

#### Scenario: Estado inicial de progresso

- **WHEN** o usuário entra no AVA diretamente ou por qualquer CTA de matrícula de um curso do seed
- **THEN** visualiza o título integral daquele curso e seu indicador demonstrativo de 10% após a consulta
- **AND** não precisa de login ou inscrição prévia

#### Scenario: Ausência de avanço fictício

- **WHEN** o usuário percorre o artigo, encontra os itens laterais ou atualiza a página
- **THEN** o progresso permanece em 10%
- **AND** nenhum registro de conclusão ou progresso de aluno é criado em servidor ou armazenamento persistente

#### Scenario: Identidade após trocar de AVA

- **WHEN** o usuário abre o AVA de outro curso
- **THEN** título, imagens, artigo e retorno passam a corresponder ao novo identificador
- **AND** o conteúdo anterior não aparece como se pertencesse ao novo curso

### Requirement: Estrutura lateral correspondente ao estado documentado

A sidebar SHALL apresentar Introdução expandida com 1.1 Introdução concluída, 1.2 Boas práticas como lição atual e 1.3 Extras pendente. Também SHALL exibir os seis módulos e suas aulas genéricas recebidos da apresentação do curso pela API. Os módulos começam recolhidos e podem ser expandidos localmente; as aulas permanecem somente leitura, sem navegação, player ou registro de conclusão. Busca permanece ilustrativa e acessivelmente indisponível.

#### Scenario: Conferência dos itens e estados

- **WHEN** o usuário examina a sidebar de Matemática
- **THEN** encontra Introdução expandida com 1.1 Introdução concluída, 1.2 Boas práticas atual e 1.3 Extras pendente
- **AND** encontra os seis módulos genéricos numerados de `2. Módulo 1` a `7. Módulo 6`, inicialmente recolhidos
- **AND** os controles de módulo comunicam seu estado expandido ou recolhido
- **AND** a busca conserva a lupa e comunica sua indisponibilidade

#### Scenario: Abertura de um módulo

- **WHEN** o usuário aciona o controle de um módulo por teclado ou ponteiro
- **THEN** o controle comunica semanticamente o estado expandido e exibe as aulas e descrições fornecidas pela API
- **AND** o acionamento seguinte recolhe somente aquele módulo
- **AND** as aulas não abrem rota, conteúdo de player ou fluxo de conclusão

#### Scenario: Ausência de conteúdo de lições não documentadas

- **WHEN** o usuário encontra Introdução, Extras, um módulo recolhido, uma aula ou o campo de busca
- **THEN** somente um módulo habilitado pode ser expandido para revelar suas aulas genéricas em texto
- **AND** aulas, Introdução, Extras e busca não iniciam rotas, player, busca de resultados ou estados de conclusão
- **AND** nenhuma interação altera o progresso demonstrativo

#### Scenario: Estrutura genérica para todos os cursos

- **WHEN** o usuário abre o AVA de diferentes cursos do seed
- **THEN** cada detalhe fornece os mesmos seis módulos e as mesmas aulas genéricas, na ordem definida pelo seed
- **AND** essa prévia não substitui os módulos e aulas relacionais específicos apresentados em Detalhes
- **AND** nenhum item lateral altera o progresso demonstrativo fixo de 10%

#### Scenario: Estrutura proveniente da API para outro curso

- **WHEN** o AVA de outro curso do seed é carregado
- **THEN** a sidebar utiliza a apresentação retornada para aquele curso
- **AND** mantém a mesma hierarquia e natureza demonstrativa, sem inferir estados de conclusão a partir dos módulos relacionais

### Requirement: Artigo integral de Boas práticas e Recomendações

A coluna principal SHALL usar o banner, artigo e recomendações do curso recebidos da API, como texto selecionável e sem truncamento. Matemática MUST manter os três parágrafos de Boas práticas e quatro de Recomendações transcritos; os demais cursos terão sete parágrafos próprios, coerentes com seus temas e a mesma estrutura visual.

#### Scenario: Leitura até o último parágrafo

- **WHEN** o usuário percorre o AVA de Matemática do começo ao fim
- **THEN** visualiza os sete parágrafos documentados, incluindo a recomendação final sobre conexão com outros alunos e profissionais
- **AND** o banner recebido preserva o recorte panorâmico e a marca sobreposta utilizando os assets do frontend
- **AND** o texto não é substituído por imagem da tela ou conteúdo genérico não recebido da API

#### Scenario: Leitura específica dos demais cursos

- **WHEN** qualquer um dos outros cinco cursos é aberto diretamente no AVA
- **THEN** seu artigo e recomendações são obtidos pelo identificador e tratam do assunto daquele curso
- **AND** não exibem o artigo específico de medidas e escalas como substituto
- **AND** os mesmos títulos de seção e proporções da página são preservados

### Requirement: Retorno determinístico aos Detalhes

As setas de retorno do AVA SHALL abrir `/cursos/{id}` com o mesmo identificador consultado, inclusive após entrada direta. O retorno MUST dispensar histórico prévio e não apresentar confirmação de salvamento, matrícula ou progresso. As URLs antigas de Matemática continuarão resolvendo o curso de referência.

#### Scenario: Voltar após entrada direta no AVA

- **WHEN** o usuário abre o AVA de um curso em nova aba e aciona a seta de retorno em desktop ou mobile
- **THEN** a aplicação exibe os Detalhes daquele mesmo curso
- **AND** não depende de um endereço anterior no histórico do navegador

#### Scenario: Ciclo entre Detalhes e AVA

- **WHEN** o usuário entra no AVA por Matricule-se, retorna aos Detalhes e entra novamente
- **THEN** o artigo e os estados demonstrativos continuam correspondendo ao mesmo curso
- **AND** não aparece indicação de matrícula realizada ou progresso atualizado

#### Scenario: Retorno enquanto dados estão indisponíveis

- **WHEN** a consulta do AVA está carregando ou falhou e a URL contém identificador válido
- **THEN** permanece disponível uma ação de retorno aos detalhes daquele identificador ou à Landing
- **AND** a aplicação não usa o retorno fixo para Matemática como alternativa

