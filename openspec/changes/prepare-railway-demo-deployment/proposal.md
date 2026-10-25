# Proposal

## Why

A demonstração precisa poder ser publicada no Railway a partir do monorepo, mas ainda não há imagens de execução, configuração de hospedagem ou GitHub Actions. Preparar esses arquivos e inicializar o catálogo no startup reduz as etapas manuais da apresentação, assumindo explicitamente os limites de uma aplicação demonstrativa.

## What Changes

- Adicionar Dockerfiles multi-stage independentes para API .NET 10 e frontend React/Vite, com contexto de build na raiz e distribuição estática servida por Nginx, não por `vite preview`.
- Preparar configurações Railway por serviço para selecionar os Dockerfiles, acompanhar caminhos relevantes, definir verificações de disponibilidade e manter uma réplica da API; documentar PostgreSQL 17 persistente e privado.
- Tornar migrations e seed iniciais uma etapa aguardada do startup da API quando `Database__InitializeOnStartup=true`, configuração habilitada no deploy demonstrativo. Manter o padrão desabilitado para desenvolvimento e testes existentes, sem apagar bancos, usar `EnsureCreated` ou recriar migrations.
- Configurar a porta HTTP dos contêineres para o ambiente Railway e distinguir a URL pública da API usada no build do frontend da origem do frontend usada nas imagens retornadas pelo backend.
- Adicionar GitHub Actions para tipos/build do frontend, build/testes .NET com PostgreSQL real e verificações das imagens Docker, sem credenciais Railway, publicação de imagens ou deploy executado pelo workflow.
- Documentar a ligação manual futura dos serviços ao repositório pelo painel Railway, a opção de aguardar a CI e os tradeoffs de migrations no startup para apresentação.

## Capabilities

### New Capabilities

- `railway-deployment`: execução demonstrativa dos dois serviços empacotados, Nginx com navegação SPA e assets corretos, configuração pública/privada e roteiro reproduzível de publicação no Railway.
- `continuous-integration`: validação automática do monorepo e dos contêineres no GitHub Actions, usando infraestrutura descartável e sem acesso ao ambiente publicado.

### Modified Capabilities

- `backend-local-runtime`: explicitar a inicialização opt-in por migrations e seed antes de atender HTTP, preservar o padrão manual local e atualizar os requisitos operacionais que hoje não distinguem essa exceção demonstrativa.

## Impact

- Novos arquivos previstos: `.github/workflows/ci.yml`, `.dockerignore`, `apps/backend/Dockerfile`, `apps/frontend/Dockerfile`, `apps/frontend/nginx/default.conf.template`, `apps/backend/railway.json`, `apps/frontend/railway.json`, `scripts/smoke-deployment.sh` e `docs/deploy-railway.md`.
- Alterações previstas no startup/configuração da API, em testes de inicialização e nos READMEs/exemplos de ambiente. Reutilizar `CatalogDbContext.UseSeeding`/`UseAsyncSeeding`, as migrations e os dados existentes, sem modificar os contratos do catálogo ou o seed.
- Preservar CORS público somente GET, Swagger apenas em Development, assets do frontend e o Compose local que inicia somente PostgreSQL.
- Não criar repositório, executar `gh`, configurar remotos, fazer commits/push, acessar contas, provisionar recursos ou publicar serviços. A entrega prepara arquivos locais; configuração externa e validação de um deploy real dependem de ação posterior do operador.
- Sem novos serviços de negócio, funcionalidades visuais, autenticação, endpoints, registries, migração de dados locais ou estrutura de alta disponibilidade. Nenhuma dependência de aplicação nova é necessária; entram imagens de build/runtime e actions de infraestrutura.
