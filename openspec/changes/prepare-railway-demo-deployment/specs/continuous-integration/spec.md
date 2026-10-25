# Spec Delta

## Purpose

Verificar automaticamente a compatibilidade do frontend, do backend e das imagens de demonstração a cada contribuição, sem depender do banco de desenvolvimento ou de acesso ao ambiente publicado.

## ADDED Requirements

### Requirement: Verificações automáticas no mesmo commit

O repositório SHALL definir GitHub Actions acionadas por push em branches, pull requests e execução manual. O resultado MUST representar o commit verificado e falhar quando uma verificação obrigatória falhar, sem filtros de caminhos que deixem um push da branch de deploy sem execução de CI.

#### Scenario: Commit recebido pela CI

- **WHEN** ocorre push em uma branch, atualização de pull request ou execução manual do workflow
- **THEN** o checkout correspondente é verificado pelos jobs obrigatórios de frontend, backend e contêineres
- **AND** o workflow fornece um resultado utilizável pela opção de aguardar CI no Railway
- **AND** o workflow não altera a branch, o remoto ou as regras do repositório

#### Scenario: Verificação obrigatória com erro

- **WHEN** tipos, build, testes ou smoke tests obrigatórios falham
- **THEN** o job e a execução não são apresentados como aprovados
- **AND** não há tolerância silenciosa com `continue-on-error` para essas verificações

### Requirement: Validação do workspace frontend

A CI SHALL instalar o frontend pela raiz com a versão de pnpm declarada no projeto e o lockfile congelado. MUST executar verificação de tipos e build com uma origem de API fictícia válida, sem exigir API ou banco em execução e sem inventar comandos de lint ou testes inexistentes.

#### Scenario: Checkout com dependências consistentes

- **WHEN** o job frontend utiliza os manifestos e o lockfile compatíveis
- **THEN** a instalação, a verificação de tipos e o build são executados
- **AND** a distribuição contém os assets locais necessários
- **AND** nenhuma credencial ou origem real de hospedagem é necessária

#### Scenario: Lockfile incompatível

- **WHEN** o manifesto exige uma resolução incompatível com o lockfile versionado
- **THEN** a instalação falha em vez de modificar ou regenerar silenciosamente o lockfile

### Requirement: Testes backend com PostgreSQL real

A CI SHALL restaurar as ferramentas locais e dependências .NET, compilar o backend e executar a suíte completa, incluindo integração com PostgreSQL real descartável. MUST usar o mecanismo de testes existente, sem acessar o banco de desenvolvimento ou de demonstração e sem substituir falhas de infraestrutura por testes ignorados.

#### Scenario: Suíte completa no runner

- **WHEN** o job backend executa em um runner com SDK .NET 10 e Docker disponíveis
- **THEN** executa os testes unitários e de integração, incluindo startup, migrations, seed e consultas
- **AND** os bancos utilizados são criados e liberados pela infraestrutura de testes
- **AND** o job não precisa iniciar o Compose local ou o frontend

#### Scenario: Docker ou banco de teste indisponível

- **WHEN** a infraestrutura relacional necessária não pode ser iniciada
- **THEN** o job falha com indicação do bloqueio
- **AND** não apresenta a suíte de integração como aprovada ou dispensável

### Requirement: Verificação executável das imagens

A CI SHALL construir e iniciar as duas imagens com PostgreSQL descartável e executar verificações HTTP. MUST comprovar inicialização automática do catálogo, preservação no reinício, portas configuráveis, entrega da SPA e respostas corretas para assets presentes e ausentes.

#### Scenario: Primeiro startup e reinício da API empacotada

- **WHEN** a imagem da API inicia com a opção automática habilitada contra um banco vazio e depois é reiniciada contra o mesmo banco
- **THEN** o primeiro atendimento ocorre com seis cursos e um detalhe utilizável sem preparação manual do esquema
- **AND** os dados, identificadores e quantidades permanecem estáveis após o reinício
- **AND** a execução usa a porta de runtime fornecida ao teste, diferente da porta padrão da imagem

#### Scenario: Frontend empacotado responde como aplicação estática

- **WHEN** a imagem frontend inicia com sua porta de runtime configurada
- **THEN** `/`, `/cursos/1`, `/ava/1` e os aliases retornam o documento da aplicação
- **AND** imagens e bundles existentes retornam arquivos com tipos de conteúdo apropriados
- **AND** um asset inexistente retorna 404, não o HTML da SPA

#### Scenario: Encerramento do smoke test

- **WHEN** as verificações terminam ou falham
- **THEN** apenas os contêineres, a rede e o armazenamento descartáveis criados por aquela execução são removidos
- **AND** não há remoção ampla de recursos Docker ou alteração do Compose de desenvolvimento

### Requirement: CI sem publicação ou segredos de ambiente

Os workflows SHALL operar com permissões mínimas de leitura do repositório e sem secrets externos. MUST NOT executar o CLI do GitHub, publicar imagens, provisionar recursos, aplicar migrations em bancos externos ou iniciar deploys; a publicação futura será responsabilidade da integração configurada pelo operador no Railway.

#### Scenario: Pull request sem acesso privilegiado

- **WHEN** a CI executa para uma contribuição sem credenciais externas, inclusive de fork
- **THEN** os jobs de validação podem executar com valores fictícios e infraestrutura local do runner
- **AND** não dependem de token Railway, login de registry ou permissão de escrita no repositório
- **AND** usam o evento normal de pull request, sem executar código não confiável em contexto privilegiado
