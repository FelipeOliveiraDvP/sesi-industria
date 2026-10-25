# Spec Delta

## MODIFIED Requirements

### Requirement: Documentação operacional reproduzível

Os READMEs SHALL documentar comandos reais, diretórios de execução, configuração de conexão e URLs das aplicações. A raiz cobrirá o ambiente integrado; o backend cobrirá desenvolvimento, migrations, testes e contratos. Os exemplos MUST distinguir Bash e PowerShell, explicar `FRONTEND_URL` e delimitar a recriação do banco local sem remoção automática de dados. MUST distinguir o preparo manual padrão da inicialização opt-in da demonstração e apontar para o roteiro Railway.

#### Scenario: Execução seguindo o README da raiz

- **WHEN** um desenvolvedor segue as instruções desde o clone e a entrada na raiz
- **THEN** consegue iniciar e verificar o PostgreSQL, configurar a conexão e a origem do frontend, restaurar ferramentas e dependências, aplicar migrations, iniciar a API e acessar o Swagger
- **AND** consegue iniciar o frontend com o endereço público da API configurado e verificar imagens e consultas no navegador
- **AND** encontra exemplos de consulta aos endpoints e de parada dos contêineres preservando os dados

#### Scenario: Trabalho diretamente no backend

- **WHEN** um desenvolvedor utiliza o README de `apps/backend`
- **THEN** encontra tecnologias, diretórios e comandos com contexto correto para compilar, criar e aplicar migrations, executar a API e os testes
- **AND** identifica os requisitos de Docker dos testes e a configuração de `FRONTEND_URL` para desenvolvimento e preview
- **AND** sabe que os arquivos de exemplo do backend não são carregados automaticamente no ambiente do processo

#### Scenario: Preparação da demonstração

- **WHEN** o operador consulta os READMEs para executar a imagem no Railway
- **THEN** identifica a opção que habilita migrations e seed no startup e o fato de ela não estar habilitada por padrão
- **AND** encontra o roteiro de configuração, as limitações dessa decisão demonstrativa e a distinção entre imagens de runtime e comandos locais do SDK

### Requirement: Recriação local explícita e delimitada

A transição para o novo seed SHALL permitir recriar explicitamente somente o banco local de desenvolvimento confirmado, preservando migrations versionadas e recursos não relacionados. Startup HTTP, consultas e testes MUST NOT apagar ou recriar o banco de desenvolvimento. A preparação manual SHALL continuar como padrão; uma opção demonstrativa explícita permitirá aplicar migrations e seed no startup sem recriação.

#### Scenario: Preparação do novo catálogo local

- **WHEN** o operador confirma a conexão local de desenvolvimento e executa a recriação documentada
- **THEN** apenas esse banco é recriado e recebe todas as migrations e o novo seed
- **AND** nenhum volume, outro banco ou dado de outro projeto é removido

#### Scenario: Execução cotidiana após a transição

- **WHEN** a API é reiniciada ou as migrations já aplicadas são verificadas novamente
- **THEN** não há recriação automática, duplicação ou sobrescrita do catálogo
- **AND** os testes relacionais continuam usando bancos descartáveis próprios

## ADDED Requirements

### Requirement: Inicialização demonstrativa opt-in do banco

A API SHALL aceitar `Database__InitializeOnStartup`, desabilitada quando ausente ou `false`. Quando `true`, MUST aplicar as migrations versionadas e o seed idempotente no banco configurado antes de atender HTTP. A inicialização MUST reutilizar o fluxo de migrations, sem criar um esquema paralelo ou depender do frontend em execução.

#### Scenario: Padrão local sem inicialização automática

- **WHEN** a API inicia com a opção ausente ou `false` contra um banco existente vazio
- **THEN** o startup não cria tabelas, aplica migrations nem executa seed
- **AND** a preparação continua disponível pelo comando manual documentado

#### Scenario: Primeiro startup demonstrativo

- **WHEN** a API inicia com `Database__InitializeOnStartup=true`, configuração válida e um PostgreSQL vazio disponível
- **THEN** aplica todas as migrations existentes e prepara os seis cursos e suas classificações, módulos, aulas e apresentações
- **AND** somente começa a atender HTTP após a conclusão bem-sucedida dessa preparação
- **AND** não exige instalação do SDK, execução manual de ferramentas EF ou servidor frontend ativo

#### Scenario: Reinício de catálogo preparado

- **WHEN** a opção está habilitada e a API é reiniciada contra um banco atualizado e já inicializado
- **THEN** verifica e aplica somente migrations pendentes e executa novamente o seed idempotente
- **AND** o seed não duplica registros, altera identificadores nem sobrescreve valores existentes
- **AND** registros adicionais permanecem preservados

### Requirement: Falha de preparação impede atendimento HTTP

Com inicialização automática habilitada, a API SHALL falhar no startup quando a configuração, a conexão, uma migration ou o seed impedir a preparação. MUST NOT começar a atender requisições, mascarar a falha ou recriar o banco para contorná-la. Diagnósticos MUST indicar a etapa ou configuração envolvida sem revelar senha ou string de conexão completa.

#### Scenario: Banco indisponível ou sem permissão

- **WHEN** a conexão não pode ser estabelecida ou a identidade não pode aplicar as migrations
- **THEN** a inicialização termina com falha sem disponibilizar o serviço HTTP
- **AND** não existe fallback para banco em memória, esquema alternativo ou catálogo fictício em runtime

#### Scenario: Erro de migration ou seed

- **WHEN** a preparação encontra uma migration incompatível ou falha ao executar o seed
- **THEN** a API não é considerada disponível e não continua com inicialização parcial tratada como sucesso
- **AND** não tenta remover banco, tabelas ou dados como recuperação automática

#### Scenario: Configuração inválida antes da preparação

- **WHEN** a opção de inicialização não é booleana ou `FRONTEND_URL` está ausente ou inválida
- **THEN** o startup rejeita a configuração antes de aplicar migrations ou executar seed
- **AND** o diagnóstico identifica a configuração necessária sem incluir credenciais
