# Proposal

## Why

Landing, Detalhes e AVA usam fixtures desconectadas do catálogo, e somente Matemática permite completar a navegação. A API precisa fornecer o conteúdo específico dos seis cursos industriais para que cada card leve aos respectivos Detalhes e AVA, sem redesenhar as telas nem acrescentar filtros.

## What Changes

- **BREAKING — dados iniciais:** substituir os seis cursos educacionais do seed pelos seis cursos da Landing, com IDs estáveis, descrições, estruturas educacionais e conteúdo demonstrativo próprios. A transição poderá recriar somente o banco local de desenvolvimento; migrations e seed subsequentes continuarão idempotentes.
- Preservar a descrição completa já existente no banco, acrescentar uma descrição breve para manter os textos dos cards e persistir a apresentação específica de Detalhes e AVA. Expor descrições na listagem e conteúdo completo em DTOs tipados de detalhes, mantendo apenas os dois endpoints GET existentes.
- Manter os badges Matemática e Ciências da Natureza dos seis cursos e uma terceira categoria cadastrada, inicialmente sem cursos, para cumprir o PRD sem alterar os badges documentados. Inferir dados fictícios e determinísticos de modalidade, status, carga horária, público-alvo e datas.
- **BREAKING — imagens/configuração:** referenciar no seed os assets existentes em `apps/frontend/public/assets`; compor suas URLs nas respostas com `FRONTEND_URL`, sem gravar o host no banco, copiar imagens ou baixar recursos. Configurar o acesso da origem do frontend à API.
- Carregar Landing, Detalhes e AVA com Axios e hooks de `@tanstack/react-query`, incluindo cache por curso, cancelamento, estados vazios, erros recuperáveis, Error Boundaries e Skeletons Mantine que conservem a estrutura das páginas.
- Habilitar `/cursos/:courseId` e `/ava/:courseId` para todos os cursos retornados, preservando as duas URLs antigas de Matemática como aliases. Manter o curso selecionado entre Detalhes, AVA e retorno.
- **BREAKING — execução da interface:** dados de curso passam a depender da API; instalação e build permanecem independentes. Não haverá fallback silencioso para mocks quando a API estiver indisponível.
- Preservar conteúdo institucional compartilhado, componentes visuais e interações locais. Não criar busca, filtros, página de catálogo, ranking de populares/destaques, login, matrícula real, player ou progresso de usuário.

## Capabilities

### New Capabilities

- `frontend-api-consumption`: consumo tipado e compartilhado de consultas, cache por identidade, cancelamento, carregamento estrutural e recuperação de falhas na interface.

### Modified Capabilities

- `catalog-persistence`: conjunto industrial alinhado às telas, descrições de card/detalhe, apresentação persistida de Detalhes/AVA e referências aos assets do frontend.
- `course-catalog-query`: descrições na listagem, apresentação completa por curso e URLs de imagens resolvidas por configuração, preservando pesquisa, filtros, contagens e somente leitura.
- `backend-local-runtime`: `FRONTEND_URL`, CORS público somente para leitura e sem credenciais, contratos OpenAPI e documentação do ambiente integrado e da recriação local.
- `frontend-local-runtime`: configuração de API, rotas de todos os cursos, aliases e funcionamento reconhecível na ausência do backend.
- `frontend-visual-foundation`: imagens provenientes dos contratos da API e estados assíncronos acessíveis, mantendo assets e identidade existentes.
- `course-discovery-page`: cards obtidos da API, sem ranking nem ordem editorial, com acesso aos detalhes de todos os cursos e demais destinos preservados.
- `course-details-page`: mesma composição para qualquer curso do seed, alimentada pela API, com CTAs para seu próprio AVA.
- `learning-environment-preview`: apresentação demonstrativa específica de cada curso recebida da API e retorno determinístico aos respectivos detalhes.

## Impact

- Backend: entidades/configurações, nova migration incremental, seed, DTOs, consultas, configuração HTTP, OpenAPI e testes em `apps/backend`.
- Frontend: dependências Axios/TanStack Query, provider e client, hooks/tipos, rotas, páginas/componentes e tratamento de estados em `apps/frontend`; lockfile compartilhado na raiz.
- Documentação: READMEs da raiz e aplicações e exemplos de ambiente. A engenharia reversa permanece evidência histórica; esta change documenta as ampliações autorizadas, sem reescrever o que foi observado.
- Banco: nenhum dado será apagado durante o planejamento. A futura recriação fica limitada ao banco local configurado e não autoriza apagar volumes ou bancos externos.
