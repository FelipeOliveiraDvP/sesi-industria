# Spec Delta

## MODIFIED Requirements

### Requirement: Execução independente do frontend

A instalação, verificação de tipos, build e inicialização do servidor frontend SHALL continuar independentes da execução da API, PostgreSQL e Docker. A obtenção dos dados de cursos dependerá da API configurada; sem ela, a estrutura e o conteúdo institucional MUST permanecer disponíveis com estados de falha recuperáveis, não com fixtures substitutas.

#### Scenario: Desenvolvimento sem infraestrutura do backend

- **WHEN** o desenvolvedor instala as dependências e inicia o frontend com backend e banco desligados
- **THEN** o servidor disponibiliza a interface e seus recursos estáticos
- **AND** as regiões dependentes de cursos apresentam falha recuperável após as tentativas limitadas
- **AND** o navegador não solicita credenciais ou conexão direta ao banco

#### Scenario: Visualização do build produzido

- **WHEN** o desenvolvedor executa o preview após um build bem-sucedido
- **THEN** consegue abrir a interface compilada sem iniciar o backend no mesmo processo
- **AND** os dados são obtidos da API configurada quando disponível, ou substituídos por estados explícitos de falha quando indisponível

### Requirement: Rotas públicas da demonstração

A aplicação SHALL disponibilizar `/`, `/cursos/:courseId` e `/ava/:courseId`, com identificador inteiro de curso. As URLs antigas `/cursos/matematica-para-a-precisao` e `/ava/matematica-para-a-precisao` MUST continuar acessíveis como aliases do curso de referência. As rotas funcionarão por entrada direta, navegação interna e atualização, sem depender de estado recebido de outra página.

#### Scenario: Acesso direto às três telas

- **WHEN** o usuário abre ou atualiza a Landing, os Detalhes ou o AVA de qualquer curso existente no servidor de desenvolvimento ou preview
- **THEN** a tela correspondente obtém seus dados pela API e apresenta o curso da URL
- **AND** o AVA abre como prévia demonstrativa sem exigir autenticação

#### Scenario: Histórico de navegação local

- **WHEN** o usuário percorre Landing, Detalhes e AVA e utiliza os controles de histórico do navegador
- **THEN** as URLs, os cursos e as telas permanecem coerentes com a navegação realizada
- **AND** nenhuma tela exige estado recebido de outra rota para renderizar

#### Scenario: Compatibilidade com as URLs antigas

- **WHEN** uma das duas URLs antigas de Matemática é aberta diretamente
- **THEN** a aplicação resolve o curso de referência e encaminha à rota equivalente com seu identificador
- **AND** o encaminhamento não exige uma visita anterior à Landing nem cria uma entrada redundante de redirecionamento no histórico

### Requirement: Caminhos inexistentes reconhecíveis

A aplicação SHALL apresentar estado acessível de não encontrado para caminhos não cadastrados, identificadores de curso inválidos ou cursos inexistentes. Esse estado MUST oferecer retorno à Landing e não gerar conteúdo de curso ou aula fictício para preencher a rota.

#### Scenario: URL desconhecida

- **WHEN** o usuário abre `/pagina-inexistente` ou um identificador de curso incompatível com o formato aceito
- **THEN** visualiza a página de não encontrado
- **AND** consegue retornar à Landing por um link acessível
- **AND** não fica preso a um carregamento de consulta desabilitada

#### Scenario: Identificador válido mas inexistente

- **WHEN** uma URL contém identificador válido e a API informa que o curso não existe
- **THEN** os Detalhes ou o AVA apresentam curso não encontrado
- **AND** não mostram Matemática ou outro curso como substituto

### Requirement: Conteúdo e operações limitados ao navegador

A interface SHALL consumir somente consultas públicas da API configurada para dados de curso, mantendo interações visuais transitórias e conteúdo institucional local. MUST NOT criar matrícula, autenticação, progresso persistido, chamadas de negócio de escrita ou dependências de CDN, fontes remotas, analytics e embeds externos.

#### Scenario: Fluxo completo sem chamadas de dados

- **WHEN** os dados necessários já estão carregados e o usuário aciona carrosséis, acordeões e abertura ou fechamento da navegação móvel
- **THEN** essas interações locais não disparam novas consultas de negócio
- **AND** não gravam dados em servidor ou armazenamento persistente

#### Scenario: Fluxo completo com consultas de leitura

- **WHEN** o usuário visita as rotas e aciona carrosséis, acordeões, links de matrícula e retorno
- **THEN** as chamadas de dados de negócio se limitam aos dois endpoints GET do catálogo
- **AND** imagens e demais recursos estáticos são servidos pelo frontend, inclusive quando suas URLs vierem da API
- **AND** não são enviados dados de usuário, matrícula ou progresso
- **AND** conexões locais de desenvolvimento continuam sendo apenas tráfego da ferramenta

#### Scenario: Reinício da demonstração

- **WHEN** o usuário atualiza a página após interações locais
- **THEN** a tela recupera os dados do curso ou um estado explícito de falha
- **AND** não recupera nem grava matrícula, conclusão de aula ou progresso de usuário em armazenamento persistente

### Requirement: Documentação e resultados verificáveis

A documentação SHALL informar preparação independente do frontend, configuração pública da API, dependência dos dados de curso, rotas por ID, aliases, limites da prévia e fallback de hospedagem. A entrega MUST distinguir verificações aprovadas, falhas e não executadas, incluindo lint apenas quando houver ferramenta configurada.

#### Scenario: Execução a partir da documentação

- **WHEN** um desenvolvedor segue o README do frontend e a orientação resumida da raiz
- **THEN** consegue instalar, iniciar, verificar tipos, compilar e visualizar a aplicação sem acoplar esses comandos ao processo da API
- **AND** identifica como preparar o backend para carregar dados e como configurar endereços no desenvolvimento, build e preview
- **AND** reconhece os estados esperados sem API, as rotas e os limites da prévia

#### Scenario: Relato de validação real

- **WHEN** a implementação é apresentada para aceite
- **THEN** o relatório registra resultados reais de tipos, build, rotas, assets, interações, console, rede e integração dos seis cursos
- **AND** inclui carregamento lento, API indisponível, recuperação e curso inexistente
- **AND** não apresenta lint ausente ou uma verificação bloqueada pelo ambiente como aprovação

## ADDED Requirements

### Requirement: Endereço público configurável da API

O frontend SHALL aceitar configuração pública do endereço HTTP ou HTTPS da API, com padrão local documentado e sem credenciais. A configuração usada no build MUST ser explicada como configuração pública, não como segredo ou valor alterável automaticamente pelo backend.

#### Scenario: Endereço local padrão ou sobrescrito

- **WHEN** o frontend é iniciado com a configuração padrão documentada ou com outra URL pública válida
- **THEN** suas consultas utilizam o endereço correspondente
- **AND** uma configuração inválida resulta em falha reconhecível, não em tela branca ou requisições para endereços derivados incorretamente
