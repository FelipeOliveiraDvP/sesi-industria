# Proposal

## Why

O código ainda não está publicado no repositório GitHub informado, e a documentação mantém um placeholder de clone e afirma que não há remoto. Publicar a branch `main` como principal torna o GitHub a referência compartilhável do projeto e permite validar a CI já existente no primeiro push, deixando a publicação Railway para uma etapa posterior.

## What Changes

- Publicar o conteúdo versionado atual no repositório `git@github.com:FelipeOliveiraDvP/sesi-industria.git`, usando `main` como branch principal/padrão.
- Atualizar a documentação de entrada e o roteiro Railway para apontar ao repositório real, substituir o placeholder de clone e indicar `main` como branch prevista para a futura integração.
- Incluir `opencode.json` como configuração do projeto, após verificar que o arquivo não contém credenciais embutidas.
- Preservar a workflow GitHub Actions existente e verificar sua execução para `main`; não adicionar job de deploy nem credenciais Railway.
- Registrar que a publicação Railway, os serviços, domínios, variáveis e a opção Wait for CI permanecem para uma etapa operacional futura.

## Capabilities

### New Capabilities

- `github-repository`: estabelecer o repositório GitHub canônico, a branch `main` como principal e referências de clone/publicação coerentes na documentação.

### Modified Capabilities

Nenhuma. Os contratos da aplicação e o comportamento da CI não mudam; a CI existente será mantida.

## Impact

- Repositório GitHub `FelipeOliveiraDvP/sesi-industria`, configuração local do remoto `origin` e branch principal `main`.
- Documentação de entrada em `README.md` e roteiro em `docs/deploy-railway.md`.
- Configuração OpenCode/MCP do projeto em `opencode.json`, sem segredo ou credencial literal.
- `.github/workflows/ci.yml` será preservado; sua execução no GitHub será verificada após a publicação.
- Nenhuma alteração em APIs, aplicação, dependências ou serviços Railway. A visibilidade e permissões atuais do repositório não serão alteradas.
