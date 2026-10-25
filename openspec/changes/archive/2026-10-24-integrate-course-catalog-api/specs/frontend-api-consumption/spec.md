# Spec Delta

## Purpose

Garantir que as telas consumam os dados públicos de cursos com identidade consistente, carregamento acessível e recuperação de falhas, sem substituir uma indisponibilidade por conteúdo fictício local.

## ADDED Requirements

### Requirement: Consultas de curso provenientes da API configurada

A interface SHALL obter a coleção e os detalhes dos cursos da API configurada. Os dados específicos de curso exibidos na Landing, nos Detalhes e no AVA MUST corresponder às respostas recebidas, sem fallback silencioso para fixtures locais.

#### Scenario: Conteúdo alterado na origem

- **WHEN** uma consulta bem-sucedida retorna um título, descrição ou artigo diferente do conteúdo anterior
- **THEN** a tela apresenta os novos dados do curso consultado
- **AND** não restaura os valores dos mocks usados pela demonstração antiga

#### Scenario: API indisponível

- **WHEN** a consulta falha sem dados anteriores utilizáveis
- **THEN** a interface apresenta um estado de erro recuperável
- **AND** não apresenta os seis mocks como se a consulta tivesse sido concluída

### Requirement: Identidade e cancelamento das consultas

As consultas SHALL isolar os dados de cada identificador de curso. Uma resposta de um curso anterior MUST NOT substituir os dados do curso da rota atual. Requisições sem consumidores SHALL poder ser canceladas sem apresentar o cancelamento como erro ao usuário.

#### Scenario: Troca rápida de curso

- **WHEN** o usuário navega do curso A ao B antes da resposta de A
- **THEN** somente os dados de B podem ocupar os Detalhes ou o AVA de B
- **AND** nenhum título, artigo ou destino de retorno de A aparece como conteúdo de B

#### Scenario: Reutilização entre Detalhes e AVA

- **WHEN** o usuário abre o AVA imediatamente após carregar os detalhes do mesmo curso
- **THEN** os dados já disponíveis desse curso podem ser reutilizados sem apresentar conteúdo de outro identificador
- **AND** a rota continua funcionando quando aberta diretamente, sem cache anterior

### Requirement: Carregamento estrutural acessível

Enquanto uma consulta inicial não possui dados, a região dependente SHALL apresentar placeholders de carregamento com a mesma composição, proporções e responsividade do conteúdo final. O estado MUST ser comunicado acessivelmente, sem expor títulos fictícios ou controles de curso acionáveis antes do carregamento.

#### Scenario: Primeira visita com resposta lenta

- **WHEN** a Landing, os Detalhes ou o AVA aguardam sua primeira resposta
- **THEN** cards, hero, sidebar ou artigo conservam seus espaços e organização conforme a tela
- **AND** a região indica carregamento e os placeholders decorativos não poluem a leitura assistiva
- **AND** as seções institucionais não dependentes da consulta permanecem disponíveis

#### Scenario: Preferência por movimento reduzido

- **WHEN** o usuário solicita redução de movimento no sistema
- **THEN** os placeholders continuam reconhecíveis sem animação não essencial

### Requirement: Atualização sem apagar dados utilizáveis

Uma atualização em segundo plano SHALL preservar o conteúdo utilizável do mesmo curso, sem substituir toda a página por placeholders. Falhas nessa atualização MUST oferecer recuperação sem tratar o conteúdo anterior como dados de outro curso.

#### Scenario: Atualização após retorno à aba

- **WHEN** uma consulta com dados anteriores do mesmo identificador é atualizada
- **THEN** o conteúdo permanece visível enquanto a nova resposta é aguardada
- **AND** uma falha apresenta indicação discreta e opção de nova tentativa

### Requirement: Falhas de consulta distinguíveis e recuperáveis

A interface SHALL distinguir consulta vazia, recurso inexistente e falha de comunicação. Erros de formato e recurso inexistente MUST NOT gerar repetição automática; falhas transitórias terão no máximo uma repetição automática por ciclo de consulta e permitirão nova tentativa explícita.

#### Scenario: Listagem vazia

- **WHEN** a API retorna HTTP 200 com `items: []` e `totalCount: 0`
- **THEN** a região informa que não há cursos disponíveis no catálogo consultado
- **AND** não apresenta erro de conexão nem cards fictícios

#### Scenario: Falha transitória e recuperação

- **WHEN** a rede, o tempo limite ou uma resposta 5xx impede uma consulta inicial
- **THEN** a região apresenta uma mensagem genérica e a ação Tentar novamente após as tentativas limitadas
- **AND** a ação executa uma nova consulta e permite recuperar o conteúdo sem recarregar a aplicação inteira

#### Scenario: Recurso inexistente

- **WHEN** a API retorna 404 para o curso solicitado
- **THEN** a interface apresenta curso não encontrado com retorno à Landing
- **AND** não repete automaticamente a mesma consulta nem mostra uma tela em branco

### Requirement: Respostas incompatíveis com o contrato

A interface SHALL recusar respostas HTTP bem-sucedidas que não contenham a estrutura necessária da coleção ou do detalhe solicitado. Dados incompatíveis MUST NOT ser apresentados ou armazenados como consulta bem-sucedida. Essa falha não terá repetição automática; a recuperação explícita deverá obter uma nova resposta.

#### Scenario: Detalhe sem apresentação

- **WHEN** uma consulta de detalhe recebe HTTP 200 sem o objeto `presentation` necessário
- **THEN** a região apresenta falha reconhecível sem conteúdo de outro curso ou tela branca
- **AND** a resposta incompatível não é armazenada como dado de sucesso nem provoca repetição automática
- **AND** Tentar novamente realiza outra consulta e recupera a tela quando a nova resposta é válida

#### Scenario: Atualização incompatível após resposta válida

- **WHEN** uma atualização recebe conteúdo incompatível para uma consulta que já possui dados válidos
- **THEN** a resposta inválida não substitui os dados válidos do mesmo curso
- **AND** a região oferece indicação de falha e nova tentativa explícita

### Requirement: Contenção de falhas de renderização

Uma exceção inesperada ao renderizar uma região SHALL produzir uma alternativa acessível com mensagem genérica e recuperação ou retorno, em vez de apagar a aplicação. A navegação para outro contexto MUST permitir sair do estado de falha sem expor detalhes internos.

#### Scenario: Exceção em uma região de curso

- **WHEN** um componente da região de curso lança uma exceção durante a renderização
- **THEN** essa região apresenta uma alternativa reconhecível com ação de recuperação ou retorno
- **AND** as regiões não afetadas permanecem utilizáveis quando tecnicamente isoladas
- **AND** não são exibidos stack traces, configurações ou credenciais

#### Scenario: Recuperação de região dependente de dados

- **WHEN** o usuário aciona a recuperação de uma região de curso que falhou durante a renderização
- **THEN** a região reinicia também a consulta correspondente e obtém uma nova resposta
- **AND** não repete indefinidamente a renderização dos mesmos dados em cache sem permitir retorno

#### Scenario: Saída do contexto com falha

- **WHEN** o usuário retorna à Landing ou navega para outro curso após uma falha de renderização
- **THEN** a interface reinicia a contenção correspondente ao novo contexto
- **AND** a falha anterior não bloqueia permanentemente a navegação
