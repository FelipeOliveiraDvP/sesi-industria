# Tasks

Este checklist só será executado após autorização para `/opsx-apply`. Ele prepara e verifica arquivos locais; não inclui uso de `gh`, criação de repositório, configuração de remoto, commit/push, publicação de imagens ou operação de contas externas. Preservar alterações locais preexistentes e as migrations/seed atuais.

## 1. Inicialização demonstrativa do banco

- [x] 1.1 Ler `Database:InitializeOnStartup` como booleano com padrão `false` e permitir configuração isolada por `CatalogApiFactory`; verificar, com testes de startup, os casos ausente, `false` e inválido, sem alterações globais de ambiente.
- [x] 1.2 Aguardar migrations e o callback de seed existente antes de iniciar HTTP quando a opção estiver ativa; verificar com PostgreSQL descartável vazio que o primeiro cliente já encontra seis cursos, classificações, módulos, aulas e apresentações, sem preparação externa do esquema.
- [x] 1.3 Cobrir reinício com a opção ativa sobre o mesmo banco, incluindo valor de curso alterado e registro extra; verificar quantidades/IDs estáveis, valores preservados pelo seed e ausência de migrations pendentes.
- [x] 1.4 Validar origem do frontend e opção antes de qualquer DDL, e impedir startup em falhas de conexão/permissão, migration ou seed; verificar testes com PostgreSQL/interceptação de falha, nenhuma resposta HTTP de sucesso e diagnósticos sem senha/conexão completa.
- [x] 1.5 Atualizar `apps/backend/.env.example` e `apps/backend/README.md` para distinguir preparo manual padrão e opt-in da demo; verificar que os exemplos continuam sem carregamento automático de `.env`, sem reset no startup e sem exigir frontend para o comando EF manual.
- [x] 1.6 Executar os testes focados de `StartupConfigurationTests` e `SeedInitializationTests` em `apps/backend`; verificar que o caso original sem criação de esquema continua aprovado com a opção padrão, sem gerar ou editar migrations e dados do seed.

## 2. Empacotamento da API e contexto Docker

- [x] 2.1 Criar `.dockerignore` na raiz excluindo configurações locais, dependências e saídas geradas sem excluir fontes, migrations, seed ou assets; verificar as regras contra os caminhos reais do monorepo e que o build não depende de `bin`, `obj` ou `dist` locais.
- [x] 2.2 Criar `apps/backend/Dockerfile` multi-stage com SDK/runtime .NET 10 e publish Release a partir do contexto da raiz; verificar o build sem banco ativo e inspecionar que a imagem final não inclui SDK, ferramentas EF, testes ou credenciais.
- [x] 2.3 Configurar runtime Production, usuário não privilegiado e expansão de `PORT` com padrão `8080`, preservando sinais por `exec` quando necessário; verificar a imagem contra PostgreSQL descartável com a flag ativa, na porta padrão e em outra porta, e confirmar indisponibilidade de Swagger em Production.
- [x] 2.4 Documentar no README do backend os comandos de build/run pela raiz e a exigência de conexão/origem/flag em runtime; verificar os exemplos com valores fictícios em recursos descartáveis, sem modificar `launchSettings.json` ou o Compose local.

## 3. Frontend multi-stage servido por Nginx

- [x] 3.1 Criar `apps/frontend/Dockerfile` com Node 24, pnpm declarado e instalação congelada do workspace pela raiz, seguido do build e estágio final Nginx; verificar construção sem API/PostgreSQL e ausência de Node, pnpm e `node_modules` no runtime final.
- [x] 3.2 Exigir e validar `ARG VITE_API_BASE_URL` no estágio de build, sem consulta de disponibilidade; verificar sucesso com origem fictícia válida e falha com valor ausente, relativo, com credenciais, subcaminho, query ou fragmento.
- [x] 3.3 Criar `apps/frontend/nginx/default.conf.template` com substituição restrita de `PORT`, padrão `8080`, SPA fallback e regra de estáticos sem fallback; verificar nas portas padrão e alternativa HTML para rotas/aliases, MIME de arquivos existentes e 404 sem HTML para assets ausentes.
- [x] 3.4 Garantir revalidação de `index.html` e preservação da origem incorporada ao bundle; verificar que alterar `VITE_API_BASE_URL` apenas ao iniciar a imagem não substitui a origem de build e que assets públicos sem hash não recebem cache imutável indiscriminado.
- [x] 3.5 Atualizar `apps/frontend/README.md` e `.env.example` com build/run de produção, contexto da raiz, rebuild por troca de origem e distinção de `vite preview`; verificar os exemplos e a conservação dos comandos locais existentes.

## 4. Arquivos Railway e roteiro do operador

- [x] 4.1 Criar `apps/backend/railway.json` e `apps/frontend/railway.json` com builder Dockerfile, caminhos por serviço, healthchecks, janelas de 300/60 segundos, uma réplica e três reinícios em falha; verificar JSON/schema oficial e ausência de `preDeployCommand`, start redundante ou valores secretos.
- [x] 4.2 Definir `watchPatterns` cobrindo entradas reais de cada imagem e os manifestos/lockfile compartilhados do frontend; verificar a matriz de alterações em fonte, configuração, Dockerfile e `.dockerignore`, e que alterações só em READMEs/`docs` não demandam rebuild.
- [x] 4.3 Criar `docs/deploy-railway.md` com Root Directory `/`, seleção dos arquivos por serviço, domínios públicos obtidos antes do build, PostgreSQL 17 com volume/rede privada, string Npgsql por referências, flag automática e Wait for CI; verificar um walkthrough usando placeholders sem executar ações remotas.
- [x] 4.4 Incluir no roteiro a explicação para apresentação, limitações de privilégios/tempo de startup/rollout e rollback sem reversão automática de banco; verificar que produção com migrations separadas é apresentada como alternativa, não implementada nesta change.
- [x] 4.5 Referenciar o roteiro no README da raiz e nos READMEs das aplicações, corrigindo afirmações incondicionais sobre startup sem migrations; verificar links, exemplos Bash/PowerShell aplicáveis e separação explícita entre preparo local e operação futura do painel.

## 5. Smoke tests automatizados e GitHub Actions

- [x] 5.1 Criar `scripts/smoke-deployment.sh` com build das imagens, PostgreSQL/rede/armazenamento descartáveis exclusivos, esperas limitadas e limpeza por trap; verificar execução bem-sucedida e falha induzida sem sobrar recursos próprios nem remover recursos externos ao teste.
- [x] 5.2 Automatizar no smoke script o primeiro catálogo, detalhe, URLs de imagem e reinício idempotente da API, além de portas alternativas, rotas/aliases, MIME e assets 404 no Nginx; verificar o script a partir de checkout sem distribuições locais e sem usar banco/conexão do usuário.
- [x] 5.3 Incluir no smoke script os builds negativos de origem ausente/inválida e a distinção entre configuração de build e runtime; verificar que as falhas esperadas são reconhecidas e que uma regressão obrigatória produz código de saída não zero.
- [x] 5.4 Criar `.github/workflows/ci.yml` para push em branches, pull requests e execução manual, com permissões `contents: read` e jobs frontend/backend/containers com timeout; verificar sintaxe e caminhos, comandos locais equivalentes, lockfile congelado e execução completa dos testes .NET/Testcontainers, sem `gh`, secrets externos, push de imagens ou deploy.
- [x] 5.5 Documentar os jobs e os pré-requisitos/comando do smoke test no roteiro, inclusive que Wait for CI exige configuração manual posterior; verificar que a documentação não afirma execução real no GitHub/Railway e não inventa lint ou suíte frontend inexistentes.

## 6. Verificação integrada da preparação

- [x] 6.1 Executar em `apps/backend` `dotnet tool restore`, `dotnet restore`, `dotnet build` e `dotnet test`, e na raiz tipos/build frontend com origem fictícia e o smoke script completo; registrar resultados reais, inclusive bloqueios, sem usar o banco de desenvolvimento ou publicar recursos.
- [x] 6.2 Exercitar localmente no navegador as imagens produzidas: Landing, Detalhes e AVA dos seis cursos, acesso direto/refresh, aliases, página desconhecida, imagens e chamadas GET/CORS; verificar ausência de erros de MIME/console e de mudanças nos contratos ou funcionalidades de negócio.
- [x] 6.3 Revisar em conjunto workflow, configurações Railway, imagens e roteiro, e registrar no roteiro o que foi ou não executado; verificar que nenhuma credencial entrou no contexto/imagens, que as diferenças da demo estão explicadas e que a publicação externa permanece uma etapa futura do operador.
