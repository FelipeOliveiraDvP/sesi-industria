# Proposal

## Why

O repositório possui backend executável, documentação visual de três telas SESI Indústria e assets locais, mas `apps/frontend` contém apenas `public/`. Falta uma interface navegável que permita conferir integralmente o design e suas interações confirmadas, sem depender da API ou de serviços remotos.

## What Changes

- Criar uma aplicação cliente em `apps/frontend` com Vite, React, TypeScript estrito, React Router DOM e Mantine como única biblioteca de componentes; usar CSS/CSS Modules para reproduzir o visual documentado.
- Estabelecer o workspace pnpm mínimo na raiz, atualmente sem `package.json`, configuração de workspace ou lockfile JavaScript, com um único `pnpm-lock.yaml` e scripts de desenvolvimento, tipos, build e preview.
- Implementar integralmente Landing, Detalhes do curso e AVA conforme os seis documentos de `docs/figma-reverse-engineering/` e suas capturas, incluindo conteúdo abaixo da dobra e estados confirmados.
- Adotar as URLs aprovadas pelo usuário: `/`, `/cursos/matematica-para-a-precisao` e `/ava/matematica-para-a-precisao`, com acesso direto, atualização do navegador e tratamento visual de caminhos inexistentes.
- Centralizar providers, roteamento, tema Mantine e fontes Open Sans locais; compartilhar header, footer, benefícios, depoimentos e outros componentes realmente repetidos, preservando as diferenças de layout entre as telas.
- Implementar carrosséis locais, navegação confirmada, acordeões com expansão simultânea, barra de matrícula após rolagem e conteúdo do AVA com progresso demonstrativo de 10%, usando fixtures tipadas e estado apenas em memória.
- Preservar os 65 arquivos atuais de `public/assets` e seus caminhos. Como as fotografias individuais dos cursos não foram fornecidas, reutilizar recortes dos três PNGs existentes nos cards, conforme autorização do usuário, registrando essa divergência em vez de prometer equivalência fotográfica.
- Adaptar a interface a desktop, tablet e mobile, com semântica, teclado, foco e contraste adequados; as adaptações menores serão decisões de implementação, pois as referências documentam somente desktop.
- Documentar execução independente e validar tipos, build, rotas, assets, interações, console e ausência de chamadas de dados/requisições externas com inspeção de navegador.
- Manter fora do escopo consumo de APIs, alterações no backend/banco/Compose, autenticação, persistência, matrícula real, pagamentos, envio de mensagens, reprodução de vídeos sem mídia fornecida e páginas/funcionalidades não documentadas. Controles sem ação confirmada permanecem visuais e explicitamente indisponíveis, sem destinos ou confirmações fictícias.

## Capabilities

### New Capabilities

- `frontend-local-runtime`: preparação, execução, build e navegação direta da SPA no workspace pnpm, com dados locais e independência do backend.
- `frontend-visual-foundation`: identidade visual compartilhada, assets e fontes locais, layouts reutilizáveis, acessibilidade e adaptação responsiva.
- `course-discovery-page`: Landing completa, apresentação dos seis cursos, métricas, benefícios, depoimentos, CTAs e carrosséis locais.
- `course-details-page`: apresentação do único curso detalhado, conteúdo integral dos acordeões, depoimentos, contato visual e acesso demonstrativo ao AVA.
- `learning-environment-preview`: AVA demonstrativo com estrutura lateral, progresso estático, artigo integral e retorno aos detalhes, sem simular um LMS funcional.

### Modified Capabilities

Nenhuma. `openspec list --specs --json` não apresenta capacidades publicadas. As capacidades da change concluída `scaffold-dotnet-catalog-backend` e seus contratos de leitura permanecem inalterados; a exclusão de frontend daquela entrega não impede esta aplicação independente.

## Impact

- Código e configurações novos ficarão em `apps/frontend`, sem sobrescrever os assets existentes. A aplicação futura acrescentará `package.json`, `pnpm-workspace.yaml` e lockfile na raiz, além de ajustes mínimos no `README.md` e `.gitignore` e um README do frontend.
- Dependências de execução: React/React DOM, React Router DOM e os pacotes compatíveis `@mantine/core`/`@mantine/hooks`; desenvolvimento: Vite/plugin React, TypeScript, tipos e configuração PostCSS recomendada pelo Mantine. Não serão adicionados outros kits UI, clientes HTTP, bibliotecas de estado ou de carrossel.
- Context7 foi consultado para Vite, React, TypeScript, React Router, Mantine e pnpm; o design registra as versões estáveis e compatibilidades verificadas, sem instalar dependências nesta fase.
- A sessão de leitura do MCP oficial do Figma foi recusada por falta de acesso de edição. A referência permanece a documentação e suas evidências locais, sem alegar extração de tokens nativos do Figma.
- Esta change cria somente planejamento. Nenhum arquivo de implementação, dependência instalada ou resultado de validação do frontend faz parte desta etapa.
