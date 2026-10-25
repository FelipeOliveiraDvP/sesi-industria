# Spec Delta

## MODIFIED Requirements

### Requirement: Documentação HTTP navegável em desenvolvimento

A API SHALL disponibilizar documento OpenAPI e Swagger UI em Development, descrevendo os dois endpoints GET, parâmetros, DTOs ampliados e códigos HTTP. Os contratos MUST documentar descrições, apresentação de Detalhes/AVA e URLs de imagens, sem acrescentar endpoints de conteúdo, exemplos alheios ao catálogo ou operações de escrita.

#### Scenario: Exploração dos contratos no Swagger

- **WHEN** a API é iniciada em desenvolvimento e o consumidor acessa o endereço Swagger documentado
- **THEN** a interface é carregada e permite consultar listagem e detalhes
- **AND** o documento descreve `search`, `categoryId`, `modalityId` e `statusId`
- **AND** documenta `description`, `shortDescription` e a apresentação tipada com a prévia do AVA
- **AND** não apresenta operações de escrita do catálogo ou endpoints de exemplo

### Requirement: Documentação operacional reproduzível

Os READMEs SHALL documentar comandos reais, diretórios de execução, configuração de conexão e URLs das aplicações. A raiz cobrirá o ambiente integrado; o backend cobrirá desenvolvimento, migrations, testes e contratos. Os exemplos MUST distinguir Bash e PowerShell, explicar `FRONTEND_URL` e delimitar a recriação do banco local sem remoção automática de dados.

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

## ADDED Requirements

### Requirement: Origem do frontend recebida pelo ambiente

O backend SHALL aceitar `FRONTEND_URL` como uma origem absoluta HTTP ou HTTPS, sem credenciais, caminho além de `/`, query ou fragmento. A configuração MUST ser validada ao iniciar o serviço HTTP, aceitar a barra final equivalente e falhar claramente quando ausente ou inválida. A API não consultará o frontend para validar essa configuração.

#### Scenario: Configuração válida com ou sem barra final

- **WHEN** a API é iniciada com `FRONTEND_URL=http://127.0.0.1:5173` ou a mesma origem com `/` final
- **THEN** utiliza uma base equivalente para imagens e uma origem normalizada para as permissões do navegador
- **AND** inicia sem exigir que o servidor do frontend já esteja em execução

#### Scenario: Configuração ausente

- **WHEN** o serviço HTTP é iniciado sem `FRONTEND_URL` ou com valor em branco
- **THEN** a inicialização falha indicando o nome da configuração necessária
- **AND** não revela conexão do banco ou outros detalhes sensíveis

#### Scenario: Configuração inválida

- **WHEN** `FRONTEND_URL` é relativa, utiliza outro esquema ou contém credenciais, subcaminho, query ou fragmento
- **THEN** o serviço HTTP rejeita a configuração antes de atender consultas
- **AND** não produz URLs de imagens malformadas ou permissões de origem excessivas

### Requirement: CORS público para consultas do catálogo

A API SHALL permitir a leitura, por navegador, dos dois endpoints GET existentes a partir de qualquer origem. A política MUST restringir métodos a GET e cabeçalhos aos necessários, sem cookies ou credenciais; `FRONTEND_URL` continuará sendo usada apenas para compor URLs de imagem. CORS MUST NOT ser apresentado como autenticação nem como bloqueio de clientes que não são navegadores.

#### Scenario: Consulta e erro a partir de qualquer origem

- **WHEN** um navegador de qualquer origem solicita a coleção ou os detalhes, inclusive com resultado 400, 404 ou erro 500 tratado
- **THEN** pode ler a resposta correspondente com `Access-Control-Allow-Origin: *`
- **AND** não precisa enviar cookies, tokens ou credenciais

#### Scenario: Métodos além da leitura

- **WHEN** um navegador tenta cadastrar, editar ou excluir cursos
- **THEN** a API continua sem operações de escrita e retorna método não permitido na rota do catálogo
- **AND** o preflight não autoriza métodos além de GET

### Requirement: Recriação local explícita e delimitada

A transição para o novo seed SHALL permitir recriar explicitamente somente o banco local de desenvolvimento confirmado, preservando migrations versionadas e recursos não relacionados. Startup HTTP, consultas e testes MUST NOT apagar ou recriar o banco de desenvolvimento; a preparação do banco continuará sendo uma operação explícita.

#### Scenario: Preparação do novo catálogo local

- **WHEN** o operador confirma a conexão local de desenvolvimento e executa a recriação documentada
- **THEN** apenas esse banco é recriado e recebe todas as migrations e o novo seed
- **AND** nenhum volume, outro banco ou dado de outro projeto é removido

#### Scenario: Execução cotidiana após a transição

- **WHEN** a API é reiniciada ou as migrations já aplicadas são verificadas novamente
- **THEN** não há recriação automática, duplicação ou sobrescrita do catálogo
- **AND** os testes relacionais continuam usando bancos descartáveis próprios
