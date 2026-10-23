# Spec Delta

## Purpose

Permitir preparar, executar e validar a interface demonstrativa do SESI Indústria no monorepo, com navegação direta e conteúdo local, independentemente do backend e de serviços externos.

## ADDED Requirements

### Requirement: Execução independente do frontend

A aplicação SHALL ser executável a partir de `apps/frontend` ou dos comandos documentados da raiz, sem iniciar a API, PostgreSQL, Docker ou qualquer serviço de negócio.

#### Scenario: Desenvolvimento sem infraestrutura do backend

- **WHEN** o desenvolvedor instala as dependências e executa o comando documentado de desenvolvimento com backend e banco desligados
- **THEN** a interface disponibiliza as três telas e suas interações locais
- **AND** não solicita conexão, credenciais ou preparação do banco

#### Scenario: Visualização do build produzido

- **WHEN** o desenvolvedor executa o comando de preview após um build bem-sucedido
- **THEN** consegue navegar na versão compilada com os mesmos dados locais e sem iniciar o backend

### Requirement: Workspace com instalação reproduzível

O monorepo SHALL permitir instalar o frontend com pnpm na raiz, mantendo um único `pnpm-lock.yaml` compartilhado e uma versão documentada do gerenciador. A preparação do frontend SHALL preservar os comandos e arquivos de execução independentes do backend.

#### Scenario: Instalação pelo workspace

- **WHEN** o desenvolvedor executa `pnpm install` na raiz com os pré-requisitos documentados
- **THEN** as dependências do frontend são resolvidas no workspace
- **AND** não é criado outro lockfile dentro de `apps/frontend`
- **AND** os procedimentos .NET existentes continuam independentes dessa instalação

### Requirement: Rotas públicas da demonstração

A aplicação SHALL disponibilizar `/` para a Landing, `/cursos/matematica-para-a-precisao` para Detalhes e `/ava/matematica-para-a-precisao` para o AVA. As três URLs aprovadas SHALL funcionar por abertura direta, navegação interna e atualização do navegador.

#### Scenario: Acesso direto às três telas

- **WHEN** o usuário abre diretamente ou atualiza qualquer uma das três URLs no servidor de desenvolvimento ou preview
- **THEN** a tela correspondente é exibida sem depender de uma visita anterior à Landing
- **AND** o AVA abre no estado demonstrativo documentado sem exigir autenticação

#### Scenario: Histórico de navegação local

- **WHEN** o usuário percorre Landing, Detalhes e AVA e utiliza os controles de histórico do navegador
- **THEN** as URLs e as telas permanecem coerentes com a navegação realizada
- **AND** nenhuma tela exige estado recebido de outra rota para renderizar

### Requirement: Caminhos inexistentes reconhecíveis

A aplicação SHALL exibir uma página de não encontrado coerente com a identidade SESI para caminhos não cadastrados, com acesso de retorno à Landing. Caminhos de cursos ou aulas não documentados SHALL NOT gerar conteúdo fictício.

#### Scenario: URL desconhecida

- **WHEN** o usuário abre `/pagina-inexistente` ou um endereço de curso diferente do único curso detalhado
- **THEN** visualiza a página de não encontrado
- **AND** consegue retornar à Landing por um link acessível

### Requirement: Conteúdo e operações limitados ao navegador

A interface SHALL usar dados estáticos locais e estado transitório, sem chamadas de dados HTTP ao backend ou a terceiros, persistência de progresso, autenticação, matrícula real ou serviços remotos. Recursos de execução SHALL ser servidos pela própria aplicação, sem CDN, fontes remotas, analytics ou embeds externos.

#### Scenario: Fluxo completo sem chamadas de dados

- **WHEN** o usuário visita todas as rotas e aciona carrosséis, acordeões, links de matrícula e retorno
- **THEN** não há chamadas a APIs próprias ou de terceiros
- **AND** os únicos recursos HTTP necessários são documentos e arquivos estáticos da aplicação no mesmo origin
- **AND** conexões locais da ferramenta de desenvolvimento, quando presentes, não são integrações de negócio

#### Scenario: Reinício da demonstração

- **WHEN** o usuário atualiza a página após interações locais
- **THEN** a tela pode retornar ao estado inicial documentado
- **AND** não é recuperada ou gravada matrícula, conclusão de aula ou progresso de usuário em armazenamento persistente

### Requirement: Comandos de qualidade e build

O frontend SHALL oferecer `dev`, `build`, `typecheck` e `preview`. A verificação de tipos SHALL falhar diante de erros de tipagem da aplicação ou configuração e não emitir código compilado; o build SHALL gerar uma distribuição local de produção com os assets necessários.

#### Scenario: Verificação de tipos bem-sucedida

- **WHEN** o desenvolvedor executa `pnpm --filter @sesi/frontend typecheck` sobre a implementação válida
- **THEN** o comando termina com código zero sem gerar JavaScript da aplicação

#### Scenario: Produção com assets locais

- **WHEN** o desenvolvedor executa `pnpm --filter @sesi/frontend build`
- **THEN** o comando conclui sem erros de tipagem ou de empacotamento
- **AND** produz uma distribuição que inclui os caminhos públicos de imagens, vetores e fontes referenciados

### Requirement: Documentação e resultados verificáveis

A documentação SHALL informar pré-requisitos, instalação, comandos, URLs, limites da simulação e fallback de rotas para hospedagem estática. A entrega SHALL distinguir verificações executadas com sucesso, falhas e verificações não realizadas, incluindo lint apenas quando existir ferramenta configurada.

#### Scenario: Execução a partir da documentação

- **WHEN** um desenvolvedor segue o README do frontend e a orientação resumida da raiz
- **THEN** consegue instalar, iniciar, verificar tipos, compilar e visualizar a aplicação sem seguir os passos do banco
- **AND** identifica as três rotas, a natureza local da demonstração e a configuração de fallback exigida fora dos servidores locais

#### Scenario: Relato de validação real

- **WHEN** a implementação é apresentada para aceite
- **THEN** o relatório registra resultados reais de tipos, build, rotas, assets, interações, console e rede
- **AND** não apresenta lint ausente ou uma verificação bloqueada pelo ambiente como aprovação
