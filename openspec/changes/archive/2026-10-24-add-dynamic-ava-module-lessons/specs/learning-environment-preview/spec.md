# Spec Delta

## MODIFIED Requirements

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
