# Spec Delta

## Purpose

Define o repositório GitHub canônico do projeto, sua branch principal e referências de clone coerentes, mantendo a CI existente e deixando a publicação no Railway para uma etapa posterior.

## ADDED Requirements

### Requirement: Repositório GitHub canônico e branch principal

O projeto SHALL publicar seu conteúdo versionado em `git@github.com:FelipeOliveiraDvP/sesi-industria.git` e SHALL usar `main` como branch principal e padrão no GitHub. A publicação MUST preservar o conteúdo existente e não alterar a visibilidade ou as permissões atuais do repositório.

#### Scenario: Publicação inicial do projeto

- **WHEN** o conteúdo revisado do projeto é publicado no repositório GitHub
- **THEN** a branch `main` contém o estado versionado publicado
- **AND** o GitHub identifica `main` como branch padrão
- **AND** a publicação não remove nem renomeia outras branches locais

#### Scenario: Acesso pelo remoto canônico

- **WHEN** um mantenedor consulta ou atualiza a origem Git configurada para publicação
- **THEN** a origem canônica é `git@github.com:FelipeOliveiraDvP/sesi-industria.git`
- **AND** nenhuma chave privada ou credencial é armazenada nos arquivos versionados

### Requirement: Documentação identifica o repositório e a branch de referência

A documentação de entrada e o roteiro de publicação SHALL identificar o repositório GitHub canônico e SHALL usar instruções de clone compatíveis com ele. O roteiro Railway MUST indicar `main` como branch prevista sem afirmar que a integração ou o deploy já foram configurados.

#### Scenario: Clone a partir da documentação de entrada

- **WHEN** um desenvolvedor segue as instruções de clone no README da raiz
- **THEN** o comando aponta ao repositório GitHub canônico
- **AND** o desenvolvedor pode continuar os comandos locais a partir da raiz do monorepo

#### Scenario: Railway permanece como etapa futura

- **WHEN** um operador consulta o roteiro Railway após a publicação no GitHub
- **THEN** encontra `main` como branch de referência para uma futura associação
- **AND** o texto distingue a existência dos arquivos locais da configuração externa e do deploy ainda não realizados
