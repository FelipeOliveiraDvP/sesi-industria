# Spec Delta

## Purpose

Permitir executar e preparar a publicação demonstrativa do catálogo no Railway com frontend estático e API independentes, configuração reproduzível e banco persistente sem exposição pública necessária.

## ADDED Requirements

### Requirement: Imagens independentes do monorepo

O projeto SHALL oferecer imagens executáveis independentes para frontend e API, construídas a partir de um checkout limpo do monorepo. Os builds MUST usar as versões documentadas e os arquivos de dependências existentes, sem exigir API, banco ou frontend em execução.

#### Scenario: Build sem infraestrutura de execução

- **WHEN** as duas imagens são construídas da raiz com acesso aos registros de dependências e a configuração pública necessária
- **THEN** ambas são produzidas sem conectar a PostgreSQL nem consultar serviços da aplicação
- **AND** o frontend utiliza o lockfile compartilhado, sem criar outro lockfile
- **AND** a execução das imagens não exige SDK .NET, Node.js, pnpm ou fontes da aplicação instalados no host

### Requirement: Porta HTTP configurável no contêiner

Os dois serviços SHALL aceitar a variável `PORT` em runtime e atender HTTP na porta indicada em todas as interfaces do contêiner. Quando ausente, MUST usar a porta documentada para execução local da imagem, sem alterar os perfis de desenvolvimento da aplicação.

#### Scenario: Porta fornecida pela plataforma

- **WHEN** cada imagem é iniciada com uma porta válida diferente do padrão em `PORT`
- **THEN** o respectivo serviço responde pela porta informada a partir de fora do contêiner
- **AND** não depende de bind exclusivo a localhost nem de uma porta de desenvolvimento fixa

#### Scenario: Execução sem porta explícita

- **WHEN** uma imagem é iniciada sem `PORT`
- **THEN** o serviço atende na porta padrão de contêiner documentada

### Requirement: Hospedagem estática com navegação SPA

O frontend empacotado SHALL servir a distribuição compilada com Nginx, sem servidor de desenvolvimento ou preview. Entrada direta e refresh das rotas da aplicação MUST receber a SPA; recursos estáticos inexistentes MUST retornar 404, não HTML de fallback.

#### Scenario: Rotas e aliases publicados

- **WHEN** o navegador abre ou atualiza `/`, `/cursos/1`, `/ava/1` ou um dos aliases legados
- **THEN** recebe o documento da aplicação e seus recursos estáticos
- **AND** o roteamento existente resolve a tela, o curso ou o redirecionamento apropriado
- **AND** uma rota de aplicação desconhecida continua sendo tratada pela página de não encontrado da SPA

#### Scenario: Recursos estáticos válidos e ausentes

- **WHEN** o cliente solicita imagens, fontes ou bundles presentes na distribuição
- **THEN** recebe o arquivo correspondente com tipo de conteúdo apropriado
- **AND** ao solicitar `/assets/inexistente.png` ou um bundle inexistente recebe HTTP 404 sem o documento `index.html`

### Requirement: Endereços públicos e configuração por fase

O build da imagem frontend SHALL exigir `VITE_API_BASE_URL` como origem HTTP ou HTTPS válida, sem credenciais, subcaminho, query ou fragmento. A API SHALL receber `FRONTEND_URL` em runtime para as imagens. As URLs de navegador MUST ser públicas; a conexão de banco MUST permanecer exclusiva do backend.

#### Scenario: URLs corretas no ambiente publicado

- **WHEN** o frontend é construído com a origem pública da API e a API inicia com a origem pública do frontend
- **THEN** o navegador consulta os dois endpoints GET existentes pela origem configurada
- **AND** as URLs de imagem retornadas apontam para os assets servidos pelo frontend
- **AND** nenhuma senha, conexão de banco ou credencial Railway é necessária no bundle

#### Scenario: Configuração de build ausente ou inválida

- **WHEN** a imagem frontend é construída sem `VITE_API_BASE_URL` ou com uma origem inválida
- **THEN** o build falha com indicação da variável necessária
- **AND** não publica silenciosamente um bundle apontando para a API local padrão

#### Scenario: Alteração de endereço após o build

- **WHEN** apenas a variável de runtime do frontend é alterada após a imagem ter sido construída
- **THEN** o bundle não troca automaticamente a origem da API
- **AND** a documentação orienta reconstruir a imagem para incorporar o novo endereço

### Requirement: Configuração Railway por serviço

O projeto SHALL versionar configurações Railway distintas para os serviços frontend e API, com seleção explícita das imagens, caminhos relevantes de rebuild e verificação HTTP de disponibilidade. A configuração da demonstração MUST manter uma réplica da API e reinícios limitados em caso de falha.

#### Scenario: Associação dos serviços pelo painel

- **WHEN** o operador associa cada serviço ao arquivo Railway correspondente e mantém o contexto de build na raiz
- **THEN** cada serviço usa seu Dockerfile e verifica a rota HTTP documentada antes de ser considerado disponível
- **AND** o serviço de frontend inclui os manifestos e o lockfile da raiz entre seus caminhos relevantes
- **AND** uma alteração exclusivamente de documentação não exige rebuild das aplicações

#### Scenario: API aguardando inicialização do catálogo

- **WHEN** a API demonstrativa ainda não concluiu migrations e seed ou encerrou com falha no startup
- **THEN** a verificação de disponibilidade não apresenta o novo deploy como pronto
- **AND** a configuração concede o tempo de inicialização documentado e não produz reinícios ilimitados

### Requirement: Segregação de banco e dados locais

O deploy documentado SHALL utilizar PostgreSQL persistente, acessado pela rede privada pela API, sem depender de acesso público ao banco. Contextos de build e imagens MUST excluir configurações locais secretas e artefatos gerados, preservando os assets necessários e as migrations versionadas.

#### Scenario: Primeiro catálogo e reinício

- **WHEN** a API demonstrativa inicia com a inicialização automática habilitada contra o banco novo provisionado pelo operador
- **THEN** o catálogo fica disponível sem copiar o banco local nem executar comandos de migrations no host do operador
- **AND** reiniciar ou substituir os contêineres da aplicação preserva o armazenamento do PostgreSQL

#### Scenario: Arquivos locais presentes no checkout de trabalho

- **WHEN** o build é executado em uma máquina que possui `.env.local`, dependências instaladas e saídas de builds anteriores
- **THEN** esses arquivos não são enviados como entradas locais secretas ou reutilizados como distribuição da imagem
- **AND** as imagens são construídas a partir dos fontes e configurações versionáveis necessários

### Requirement: Preparação local e operação externa explícita

A entrega SHALL preparar arquivos e um roteiro de publicação sem criar repositórios, executar o CLI do GitHub ou operar contas externas. O roteiro MUST separar decisões versionadas de configurações manuais no painel, explicar os limites demonstrativos do startup automático e não apresentar deploy não executado como validado.

#### Scenario: Entrega sem contas de hospedagem

- **WHEN** a preparação é concluída sem acesso a GitHub ou Railway
- **THEN** os arquivos podem ser revisados e os contêineres podem ser verificados localmente
- **AND** o roteiro lista a ligação futura do repositório, a branch de deploy, os domínios, as variáveis, o banco e a opção de aguardar a CI como ações do operador
- **AND** não há criação de remoto, commit, push, publicação de imagem ou provisionamento remoto implícitos

#### Scenario: Explicação da decisão demonstrativa

- **WHEN** o responsável consulta o roteiro para a apresentação
- **THEN** identifica que migrations no startup exigem permissões de esquema, aumentam o tempo de inicialização e não oferecem rollout coordenado ou rollback automático do banco
- **AND** encontra a alternativa de separar migrations do processo HTTP em uma implantação de produção
