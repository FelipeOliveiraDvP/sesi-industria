# Tasks

## 1. Modelo e evolução do esquema

- [x] 1.1 Acrescentar `ShortDescription` obrigatório até 1000 caracteres e `PresentationJson` obrigatório como `jsonb` ao curso, preservando `Description` e as seis entidades atuais; verificar obrigatoriedade, tipos e limites em `ModelConfigurationTests`.
- [x] 1.2 Definir tipos internos do documento editorial e validação/serialização de apresentação, sem HTML ou dados de aluno; verificar round-trip de Detalhes e AVA e rejeição de conteúdo obrigatório ausente em testes unitários.
- [x] 1.3 Restaurar o `dotnet-ef` local e gerar uma migration incremental em `Data/Migrations`, atualizando o snapshot corrente sem modificar `InitialCreate` ou seu designer; revisar o diff e testar a sequência completa em PostgreSQL vazio, sem default de string vazia para JSONB.
- [x] 1.4 Documentar em `apps/backend/README.md` a nova migration e o caminho de transição por recriação explícita do banco local, sem upsert de IDs antigos; verificar que os comandos usam o diretório e projetos reais e não removem volumes.

## 2. Seed industrial e conteúdo das três telas

- [x] 2.1 Substituir o conjunto por seis cursos com IDs, nomes breves, classificações, cargas horárias, públicos e datas da tabela do design; copiar integralmente títulos e descrições breves da Landing e verificar os seis valores esperados em `SeedDataTests`.
- [x] 2.2 Manter Matemática e Ciências da Natureza nos seis cursos e cadastrar Linguagens sem associação inicial; verificar três categorias, duas modalidades, três status e referências válidas sem acrescentar um sétimo curso.
- [x] 2.3 Transpor os dez módulos e trinta tópicos de Matemática para módulos/aulas relacionais e escrever as estruturas dos outros cursos conforme o roteiro do design; verificar contagens, diversidade, IDs, descrições e ordens em testes de seed e persistência.
- [x] 2.4 Preparar apresentações de Detalhes/AVA para os seis cursos, preservando integralmente a referência de Matemática e criando textos próprios para os outros cinco; verificar dez FAQs por curso, artigos com três mais quatro parágrafos, coerência de modalidade e progresso demonstrativo fixo.
- [x] 2.5 Referenciar os PNGs existentes pelos caminhos `/assets/` definidos no design, sem hosts, cópias ou downloads; verificar existência dos arquivos no monorepo e preservação byte a byte dos assets originais.
- [x] 2.6 Atualizar `SeedDataTests`, `SeedIntegrityTests` e `SeedInitializationTests` para novos campos e conteúdo, preservando testes de banco vazio, repetição, preenchimento de registros ausentes e não sobrescrita; executar os testes de seed com PostgreSQL real quando aplicável e registrar limitações.
- [x] 2.7 Atualizar a seção de dados iniciais do README do backend com o novo conjunto, terceira categoria sem cursos e distinção entre estrutura relacional e prévia do AVA; conferir a documentação contra o seed e as transcrições.

## 3. Contratos HTTP, imagens e origem do frontend

- [x] 3.1 Adicionar configuração de `FRONTEND_URL` validada no startup HTTP, sem dependência dela no DbContext/seed; testar valores ausentes, inválidos, com barra final e preparação do banco com frontend desligado e sem resolver essa origem.
- [x] 3.2 Implementar resolução central das imagens do card, hero e AVA usando a base validada; verificar URLs absolutas corretas com duas bases sobre os mesmos dados, caminhos sem `public` e ausência de requisições externas durante o seed.
- [x] 3.3 Ampliar o resumo com `description`/`shortDescription` e o detalhe com `shortDescription`/`presentation` tipado; adaptar projeções e montagem dos DTOs e verificar consistência dos campos comuns, JSON tipado e ausência de navegações internas.
- [x] 3.4 Configurar CORS para qualquer origem somente nos dois endpoints GET, sem credenciais e com cabeçalhos necessários; testar leituras e erros de várias origens, além da indisponibilidade dos métodos de escrita.
- [x] 3.5 Atualizar `CatalogApiFactory`, testes de contratos, consultas, filtros, pesquisa e detalhes para o novo seed; executar a suíte mantendo pesquisa exclusiva pelo nome breve, critérios cumulativos, contagem, vazio, data nula e ordem de módulos/aulas.
- [x] 3.6 Adaptar os testes de imagens e OpenAPI para assets do frontend, DTOs editoriais e apenas duas operações de negócio; verificar os esquemas publicados, URLs e existência dos arquivos sem depender de servidor Node nos testes .NET.
- [x] 3.7 Acrescentar `FRONTEND_URL` ao exemplo de ambiente do backend e atualizar READMEs de endpoints/origens com Bash e PowerShell; verificar que o backend continua sem carregar `.env` automaticamente e que desenvolvimento/preview usam hosts e portas coerentes.

## 4. Client e estado remoto do frontend

- [x] 4.1 Adicionar Axios `1.20.0` e `@tanstack/react-query` `5.104.5`, mantendo versões existentes e apenas o lockfile da raiz; verificar instalação reproduzível, peers compatíveis e `pnpm frontend:typecheck`.
- [x] 4.2 Criar a instância Axios com `VITE_API_BASE_URL`, padrão local documentado, timeout de 10 segundos e consultas tipadas com `AbortSignal` e guards mínimos antes do cache; verificar URLs, identidade e rejeição de HTTP 200 sem `presentation`, sem biblioteca de schema ou autenticação/interceptors desnecessários.
- [x] 4.3 Configurar provider com uma instância estável de cache e hooks `useCourses`/`useCourse`, chaves normalizadas, stale time de 60 segundos e retenção em memória de cinco minutos; verificar tipos e que Detalhes/AVA compartilham a chave do mesmo ID sem persistência local ou dados do ID anterior.
- [x] 4.4 Definir classificação de falhas e política de no máximo uma repetição transitória, sem repetição de 4xx, configuração inválida, resposta incompatível ou cancelamento; verificar os casos em sessão controlada, incluindo preservação dos dados válidos após refetch inválido, sem adicionar um runner por convenção.
- [x] 4.5 Criar `apps/frontend/.env.example` e documentar configuração pública, padrão local, timeout/cache e necessidade de rebuild para trocar o endereço do bundle; verificar exemplos de desenvolvimento e preview sem expor credenciais.

## 5. Navegação e renderização dos seis cursos

- [x] 5.1 Parametrizar rotas e geradores de caminhos por ID, validar identificadores e preservar aliases de Matemática com substituição de histórico; verificar entrada direta, refresh, ID inválido, ID inexistente e ambos os aliases sem depender da Landing.
- [x] 5.2 Alimentar o trilho por `useCourses`, usar o ID como identidade e destino de todos os cards e manter a ordem da API; verificar seis links corretos, ausência de consultas de detalhe por card e manutenção das ações de catálogo indisponíveis.
- [x] 5.3 Alimentar hero, Sobre o curso, módulos, informações de aulas/avaliações, FAQs e barra pelos dados de `useCourse`; verificar textos integrais de Matemática, conteúdo próprio dos demais, data sem deslocamento de fuso e A definir para data nula.
- [x] 5.4 Alimentar identificação, banner, sidebar, artigo e recomendações do AVA pela apresentação do mesmo detalhe; verificar retorno desktop/mobile ao mesmo ID e progresso de 10% sem matrícula, navegação de lições ou gravação de conclusão.
- [x] 5.5 Atualizar títulos assíncronos da aba, props e imports para retirar a dependência de mocks de curso, preservando conteúdo institucional; verificar títulos dos seis cursos e ausência de saltos adicionais de foco/rolagem ao terminar a consulta.
- [x] 5.6 Adaptar observers da barra e limites do carrossel à chegada de dados e troca de identidade; verificar barra após hero atrasado, reinício dos acordeões por curso, avanço/retorno do trilho e cleanup em desenvolvimento.
- [x] 5.7 Verificar os percursos com respostas controladas e troca rápida A/B antes da integração final; comprovar que títulos, textos e retorno não se misturam e atualizar a tabela de rotas/limites no README do frontend.

## 6. Carregamento e recuperação de falhas

- [x] 6.1 Criar estados de consulta vazia, 404, resposta incompatível e erro transitório com nova tentativa, mantendo dados utilizáveis no refetch; verificar vazio/404/5xx, timeout, API indisponível e 200 sem `presentation`, com recuperação por nova requisição e sem fallback para fixtures.
- [x] 6.2 Adicionar Error Boundary nativo nas regiões de curso e proteção final independente de providers; provocar exceção de renderização em ambiente isolado e verificar recuperação que reinicia a consulta atual, retorno e reset ao mudar de contexto, sem flags de erro no produto.
- [x] 6.3 Implementar Skeletons Mantine do trilho da Landing, mantendo o conteúdo institucional visível; atrasar a resposta e verificar seis posições, mesmas dimensões e limites recalculados após os dados.
- [x] 6.4 Implementar Skeletons Mantine de hero e conteúdo dos Detalhes, sem matrícula/barra acionável antes dos dados; verificar preservação de duas colunas, área de acordeões e transição correta para os conteúdos carregados.
- [x] 6.5 Implementar Skeletons Mantine da sidebar, banner e leitura do AVA, preservando retorno acessível pelo ID da rota; verificar estado inicial lento e falha sem retornar indevidamente a Matemática.
- [x] 6.6 Verificar `aria-busy`, leitura assistiva dos estados, skeletons decorativos, teclado e movimento reduzido nas três telas; confirmar que refetch com dados não troca a página por placeholders nem desloca o foco.
- [x] 6.7 Documentar no README do frontend os estados técnicos, a diferença entre falha HTTP e renderização e o caráter demonstrativo do AVA; conferir que o texto não promete operação offline completa, matrícula ou progresso real.

## 7. Preparação integrada e aceite transversal

- [x] 7.1 Confirmar a conexão do banco local de desenvolvimento autorizado, registrar somente host/nome sem segredos e recriar exclusivamente esse banco, se ainda contiver o seed antigo; verificar que nenhum volume, outro banco ou processo alheio foi removido.
- [x] 7.2 Em `apps/backend`, executar `dotnet tool restore`, `dotnet restore`, atualização das migrations e nova atualização para idempotência; verificar o novo catálogo sem frontend em execução e executar `dotnet build`/`dotnet test`, distinguindo qualquer impedimento de Docker dos resultados aprovados.
- [x] 7.3 Na raiz, executar `pnpm frontend:typecheck` e `pnpm frontend:build`; verificar que o build não exige a API ativa e que o bundle usa a configuração pública documentada, sem lockfile adicional.
- [x] 7.4 Executar os seis ciclos completos com API real em desenvolvimento e preview, incluindo entrada direta, refresh, aliases e histórico; verificar título, descrição, conteúdo do AVA, retorno correto e ausência de chamadas de escrita ou detalhe por card.
- [x] 7.5 Executar a matriz transversal do design em 1440, 1024, 768 e 375 px, incluindo atraso, falha e dados em cache; verificar entrega HTTP das imagens do frontend, CORS, console/rede, acordeões, footer e ausência de overflow do documento.
- [x] 7.6 Conferir o tutorial integrado do README da raiz contra os comandos efetivamente executados e registrar resultados/limitações; verificar que não foram adicionados filtros, busca, catálogo extra, ranking, player, autenticação, matrícula real ou progresso persistido e que os assets e trabalhos não relacionados permanecem preservados.
