# Spec Delta

## Purpose

Permitir que desenvolvedores preparem, executem, documentem e testem o backend do catálogo isoladamente no monorepo, usando a API no host e PostgreSQL local em contêiner.

## ADDED Requirements

### Requirement: Backend independente no monorepo

O projeto SHALL disponibilizar o backend em `apps/backend`, compatível com o SDK .NET 10 e executável pelo terminal do host. Restaurar dependências, compilar, executar a API e executar seus testes não deve exigir frontend, IDE ou sistemas externos de negócio.

#### Scenario: Preparação sem outras aplicações

- **WHEN** um desenvolvedor com os pré-requisitos instalados executa os comandos documentados a partir de `apps/backend`
- **THEN** consegue restaurar e compilar a solução do backend e executar seus testes
- **AND** não precisa instalar ou implementar frontend, Moodle ou outro sistema de negócio

#### Scenario: Inicialização da API no host

- **WHEN** o banco está saudável, as migrations foram aplicadas e a conexão está configurada
- **THEN** o comando documentado inicia a API no host e informa seu endereço HTTP

### Requirement: PostgreSQL local configurável e persistente

O `docker-compose.yml` da raiz SHALL iniciar somente o serviço PostgreSQL necessário ao backend. Deve utilizar imagem oficial estável, nome explícito de serviço, banco, usuário e senha configuráveis, porta de host padrão `5432` configurável, volume nomeado, healthcheck e política de reinicialização apropriada ao desenvolvimento.

#### Scenario: Inicialização com valores padrão de desenvolvimento

- **WHEN** o desenvolvedor executa `docker compose up -d` na raiz sem sobrescrever variáveis
- **THEN** somente o PostgreSQL é iniciado, usando os valores locais documentados e a porta `5432`
- **AND** o serviço informa estado saudável quando pode receber conexões

#### Scenario: Sobrescrita de configuração

- **WHEN** o desenvolvedor informa outros valores de banco, usuário, senha e porta antes da criação do serviço
- **THEN** o Compose utiliza esses valores e a conexão da API pode ser ajustada de forma correspondente

#### Scenario: Reinício sem perda de dados

- **WHEN** os contêineres são parados ou removidos e recriados sem remover o volume nomeado
- **THEN** o esquema e os dados previamente persistidos permanecem disponíveis

### Requirement: Conexão recebida pelo ambiente do processo

A API SHALL receber `ConnectionStrings:DefaultConnection` pelo mecanismo padrão de configuração, aceitando a variável `ConnectionStrings__DefaultConnection`. O arquivo `apps/backend/.env.example` deve apresentar uma conexão compatível com os padrões do Compose e servir apenas como referência, sem carregamento automático ou customizado de `.env`.

#### Scenario: Conexão configurada no terminal

- **WHEN** o desenvolvedor define `ConnectionStrings__DefaultConnection` no ambiente e inicia a API
- **THEN** a API utiliza essa conexão para acessar o PostgreSQL

#### Scenario: Arquivo de exemplo sem exportação

- **WHEN** apenas `.env.example` existe, sem disponibilizar a conexão ao processo
- **THEN** esse arquivo não é interpretado automaticamente como configuração da API
- **AND** a documentação explica como exportar a variável em Bash e PowerShell

#### Scenario: Configuração de conexão ausente

- **WHEN** a API é iniciada sem uma conexão configurada
- **THEN** a inicialização falha com uma indicação clara da configuração necessária, sem revelar segredos

### Requirement: Documentação HTTP navegável em desenvolvimento

A API SHALL disponibilizar documento OpenAPI e Swagger UI no ambiente de desenvolvimento, descrevendo os dois endpoints de leitura, seus parâmetros, DTOs de resposta e códigos HTTP. A entrega não deve conservar endpoints de exemplo alheios ao catálogo.

#### Scenario: Exploração dos contratos no Swagger

- **WHEN** a API é iniciada em desenvolvimento e o consumidor acessa o endereço Swagger documentado
- **THEN** a interface é carregada e permite consultar listagem e detalhes
- **AND** o documento descreve `search`, `categoryId`, `modalityId` e `statusId`
- **AND** não apresenta operações de escrita do catálogo ou endpoints de exemplo

### Requirement: Falhas HTTP sem exposição de detalhes internos

A API SHALL tratar exceções HTTP não previstas com resposta consistente em Problem Details e status apropriado. As respostas não devem expor stack traces, strings de conexão, senhas ou outros detalhes internos.

#### Scenario: Falha inesperada ao atender consulta

- **WHEN** uma consulta encontra uma exceção não tratada
- **THEN** a API retorna HTTP 500 com Problem Details e mensagem genérica
- **AND** o corpo da resposta não contém detalhes internos ou segredos

### Requirement: Documentação operacional reproduzível

Os READMEs SHALL documentar comandos reais do projeto. A raiz deve cobrir pré-requisitos e execução completa; `apps/backend/README.md` deve cobrir desenvolvimento, estrutura, compilação, migrations, testes e endpoints, sem duplicação desnecessária. Exemplos Bash e PowerShell devem distinguir sintaxes de variáveis de ambiente.

#### Scenario: Execução seguindo o README da raiz

- **WHEN** um desenvolvedor segue as instruções desde o clone e a entrada na raiz
- **THEN** consegue iniciar e verificar o PostgreSQL, configurar a conexão, entrar em `apps/backend`, restaurar ferramentas e dependências, aplicar migrations, iniciar a API e acessar o Swagger
- **AND** encontra exemplos de consulta aos endpoints e de parada dos contêineres preservando os dados

#### Scenario: Trabalho diretamente no backend

- **WHEN** um desenvolvedor utiliza o README de `apps/backend`
- **THEN** encontra tecnologias, diretórios e comandos com contexto correto para compilar, criar e aplicar migrations, executar a API e os testes
- **AND** consegue identificar os requisitos de Docker dos testes de integração

### Requirement: Testes relacionais reais e verificáveis

A suíte SHALL incluir verificações automatizadas de inicialização, relacionamentos, migrations, seed e contratos de consulta. Testes de persistência e consultas devem executar contra PostgreSQL real, sem substituí-lo definitivamente por banco em memória. Os comandos e resultados de validação devem ser relatados sem apresentar testes não executados como aprovados.

#### Scenario: Execução completa dos testes

- **WHEN** o desenvolvedor executa o comando documentado de testes com Docker disponível
- **THEN** os testes exercitam migrations, integridade, seed repetido, pesquisa, filtros individuais e combinados, contagens, detalhes, curso inexistente e consultas vazias contra PostgreSQL
- **AND** os dados locais de desenvolvimento não são apagados pelos testes

#### Scenario: Infraestrutura de teste indisponível

- **WHEN** a execução não consegue disponibilizar PostgreSQL por falta de Docker ou outro pré-requisito
- **THEN** a limitação é identificada explicitamente e os testes de integração não são reportados como aprovados

### Requirement: Configurações locais não versionadas

O repositório SHALL ignorar credenciais e configurações locais, `.env`, arquivos de IDE, logs e artefatos gerados aplicáveis, preservando `.env.example`, migrations, fontes, testes e documentação necessários. Somente valores fictícios de desenvolvimento podem constar dos exemplos versionados.

#### Scenario: Arquivos gerados e exemplos

- **WHEN** o desenvolvimento produz `bin`, `obj`, logs ou arquivos `.env` locais
- **THEN** esses arquivos são abrangidos pelas regras de exclusão do repositório
- **AND** `.env.example` e os arquivos necessários à compilação, execução e documentação continuam elegíveis para versionamento
