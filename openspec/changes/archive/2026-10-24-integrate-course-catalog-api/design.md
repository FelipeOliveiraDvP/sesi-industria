# Design

## Context

Ver `proposal.md`, seção Why, para a motivação. A exploração identificou:

- `Course.Description`, sua configuração obrigatória e a coluna na migration `20261023101851_InitialCreate` já existem. O detalhe já a expõe; a listagem não. O card e Sobre o curso possuem textos distintos na referência.
- `CourseQueryService` pesquisa e filtra no PostgreSQL, projeta DTOs, ordena a coleção por nome completo/ID e os módulos/aulas por ordem/ID. Esse comportamento não precisa de uma nova arquitetura de backend.
- Os seis cursos do seed não são os da Landing. O seed só insere IDs ausentes: editar seus valores não atualiza o banco existente. As verificações de categorias contam as classificações cadastradas, sem exigir curso em cada uma.
- O frontend possui três telas, rotas fixas, fixtures e componentes reutilizáveis; não possui client HTTP, cache remoto ou suíte de testes frontend. As dependências existentes incluem React `19.3.1`, Mantine `9.7.3`, React Router `7.18.4` e Vite `8.3.5`.
- A engenharia reversa confirma somente o percurso de Matemática, dez módulos em Detalhes e uma sidebar demonstrativa diferente no AVA. O usuário autorizou expandir o percurso para todos os cursos, não criar filtros nem navegação de lições.
- A API em execução respondeu às consultas de coleção, combinação de critérios, detalhe, vazio, 400 e 404 durante a exploração. Isso não constitui execução da suíte nem validação da integração futura.

Este design é necessário pela alteração transversal, novas dependências, evolução do modelo, CORS e transição do banco. Os contratos observáveis estão nas nove specs desta change.

## Goals / Non-Goals

**Goals:**

- Usar uma identidade de curso em todo o percurso, sem estado de navegação obrigatório ou duplicação de cache entre Detalhes e AVA.
- Separar dados relacionais do catálogo, conteúdo editorial de apresentação e configuração de ambiente.
- Reutilizar layouts e componentes com props tipadas, preservando os assets e a interação visual, inclusive sob carregamento ou falha.
- Manter a API somente leitura, a preparação do banco explícita e os testes relacionais independentes do servidor frontend.

**Non-Goals:**

- Não criar CMS, repositórios genéricos, CQRS, novos projetos .NET, endpoints de conteúdo ou um sistema de ensino funcional.
- Não transformar a sidebar em árvore navegável de aulas, o progresso em estado de aluno nem o botão de matrícula em operação de inscrição.
- Não acrescentar campos visuais de modalidade, carga horária ou status às telas só porque o DTO os possui. Eles continuam disponíveis à consulta do catálogo.
- Não alterar ordenação por popularidade, criar destaque editorial, nova página de catálogo, filtro, busca, contador visual ou destinos de footer.
- Não instalar dependências, gerar migrations, modificar código ou apagar dados durante este planejamento.

## Decisions

### 1. Limite entre conteúdo remoto e institucional

A API será a fonte dos dados específicos de curso nas três páginas: identificação, descrições, imagens, data, certificação, módulos/aulas, informações de aulas/avaliações, FAQs e apresentação do AVA. Hero institucional da Landing, métricas, benefícios compartilhados, depoimentos, marcas, textos de navegação e footer permanecem locais e com a mesma apresentação.

Os mocks de curso deixam de alimentar a aplicação; seu conteúdo será transposto para o seed ou usado apenas como referência de conferência. `marketing.ts` pode continuar contendo métricas, benefícios e depoimentos. Não apagar fixtures ou assets não relacionados apenas para limpar diretórios.

**Alternativas:** tornar todo o site um CMS ampliaria o produto; manter artigo/FAQs de Matemática locais e reutilizá-los para os outros cursos produziria conteúdo incorreto. O limite escolhido atende ao pedido de dados em todas as páginas sem criar serviços institucionais.

### 2. Seed com IDs conhecidos, dois badges e terceira categoria sem curso

Usar IDs `1` a `6` na ordem de identificação abaixo; isso é identificação estável, não prioridade de exibição. Nomes completos e descrições breves devem ser copiados integralmente de `docs/figma-reverse-engineering/pages/001-landing-page.md`. A coleção continuará ordenada tecnicamente por nome completo e ID; o frontend não reordena nem infere popularidade.

Categorias: `1 = Matemática`, `2 = Ciências da Natureza`, `3 = Linguagens`. A terceira é uma escolha de seed, inicialmente sem cursos: mantém o mínimo de três classificações do PRD sem alterar os badges ou acrescentar um sétimo card. Não se cria filtro nem badge novo para utilizá-la.

Modalidades: `1 = Online`, `2 = Presencial`. Status: `1 = Disponível`, `2 = Em breve`, `3 = Encerrado`. Os valores abaixo são decisões fictícias e constantes de preparação, não informações reais de turmas.

| ID | Curso / nome breve | Categoria | Modalidade | Status | Horas | Disponibilização | Público-alvo |
| --- | --- | --- | --- | --- | --- | --- | --- |
| 1 | Matemática para a Precisão: Medidas e Escalas na Metalurgia / Metrologia | 1 | 1 | 1 | 40 | 2026-01-05 | Operadores metalúrgicos e técnicos em formação |
| 2 | Geometria e Modelagem no Design Têxtil / GeoTêxtil | 1 | 2 | 1 | 32 | null | Modelistas e profissionais de produção têxtil |
| 3 | Estatística e Controle de Qualidade na Produção / QualiDados | 1 | 1 | 2 | 24 | 2027-02-10 | Inspetores de qualidade e líderes de produção |
| 4 | Física do Movimento e da Energia na Metalurgia / Física Industrial | 2 | 2 | 3 | 36 | 2025-08-20 | Operadores e técnicos de manutenção industrial |
| 5 | Química e Sustentabilidade / Química Têxtil | 2 | 1 | 2 | 20 | 2027-03-05 | Profissionais de tinturaria e gestão ambiental |
| 6 | Ciência dos Materiais na Metalurgia / Materiais Industriais | 2 | 2 | 3 | 30 | null | Técnicos metalúrgicos e inspetores de materiais |

O alias Metrologia também fornece um caso de pesquisa que corresponde ao nome breve sem ocorrer no nome completo. Não associar visibilidade, possibilidade de consultar o AVA ou cálculo de datas ao status.

Para Matemática, preservar a descrição completa de Sobre o curso, os dez módulos e os trinta tópicos da referência de Detalhes. Cada tópico será a descrição de uma aula, com título curto próprio; o frontend apresenta a descrição como o tópico já documentado. Os módulos continuam tendo descrição própria obrigatória, ainda que ela não introduza um novo bloco visual.

Para os cursos sem detalhes documentados, escrever conteúdo fictício determinístico no mesmo formato, em vez de reutilizar textos de Matemática. Roteiro concreto para o seed:

| ID | Módulos previstos | Quantidade de aulas por módulo |
| --- | --- | --- |
| 2 | Formas e simetrias; Medidas e proporções; Modelagem e planificação; Aplicação em estampas | 2, 3, 2, 2 |
| 3 | Leitura de dados; Variação e amostragem; Controle de processos | 2, 3, 2 |
| 4 | Movimento; Forças e atrito; Trabalho e potência; Calor e transferência; Eficiência energética | 2, 2, 3, 2, 2 |
| 5 | Reações e processos têxteis; Uso de recursos e tratamento de resíduos | 3, 3 |
| 6 | Estrutura dos metais; Propriedades mecânicas; Tratamentos e seleção; Inspeção de materiais | 2, 3, 2, 2 |

Manter a geração determinística de IDs de módulos/aulas já utilizada, com ordem explícita. As cinco apresentações novas terão descrição completa própria, dez FAQs adaptadas, textos de aulas/avaliações compatíveis com a modalidade e artigo de três parágrafos mais quatro de recomendações. Para Presencial, o bloco Aulas online descreve materiais e apoio digital, sem afirmar que toda a formação é online. Os sete parágrafos de Matemática permanecem integrais.

**Alternativas:** reclassificar Química como uma terceira categoria mudaria o badge documentado; adicionar um curso só para ocupar a terceira categoria mudaria a Landing; reduzir o mínimo de categorias alteraria o PRD sem solicitação.

### 3. Descrição completa existente, descrição breve e conteúdo editorial persistido

Manter as seis entidades relacionais atuais. Acrescentar ao curso:

| Campo persistido | Escolha |
| --- | --- |
| `Description` | Já existente, obrigatório, até 4000 caracteres; descrição completa, sem nova coluna duplicada |
| `ShortDescription` | Novo texto obrigatório, até 1000 caracteres; cópia integral do texto do card |
| `PresentationJson` | Novo texto obrigatório mapeado como `jsonb`; documento editorial de Detalhes/AVA |
| `ImageUrl` | Manter propriedade/coluna existente; armazenar caminho público relativo do card |

O documento terá contrato estável representado por tipos internos próprios, serializados/deserializados com System.Text.Json. A coluna permanece um `string` no modelo EF com tipo `jsonb`, evitando novas entidades de apresentação ou mapeamento POCO legado. A serialização ocorre ao preparar o seed e a leitura tipada ao montar o detalhe; o documento bruto não é o DTO HTTP.

O documento inclui `certification`, `detailsImagePath`, `onlineLessons`, `assessments`, `faqs` e `learningPreview`. Esta última contém `category`, `progressPercent`, `searchLabel`, `bannerImagePath`, `introduction`, `modules`, `article` e `recommendations`, nos formatos já usados pela interface. Arrays têm ordem estável; estados de lição são limitados a `completed`, `current` e `pending`. Não armazenar HTML, JavaScript, dados de aluno ou nomes de componentes.

A sidebar é uma apresentação demonstrativa separada dos módulos relacionais: preserva Introdução, três itens e seis linhas genéricas recolhidas, inclusive para Matemática com dez módulos nos Detalhes. Os valores de 10% e conclusão são exemplos de apresentação do curso, não registros de progresso de pessoa. Cada curso possui seu próprio documento e artigo contextualizado.

Validar a presença dos dados e dos caminhos ao construir o seed, com testes de round-trip. O PostgreSQL garante JSON válido; a aplicação e os testes garantem o contrato do conteúdo. Não criar índices JSON, operações de edição ou consultas dentro desse documento: busca/filtros continuam relacionais.

**Alternativas:** um único texto para card e Sobre o curso perderia uma das transcrições; várias tabelas editoriais aumentariam o modelo sem consultas que as justifiquem; mapear diretamente POCO por mecanismos legados do Npgsql foi descartado. Complex types com `ToJson()` são uma opção oficial do EF 10, mas não são necessários para um documento pequeno lido integralmente e sem atualização parcial.

### 4. Dois endpoints existentes, DTOs ampliados e tipados

Não criar endpoints de aulas, AVA, classificações, métricas ou conteúdo institucional.

| Resposta | Campos e consumo |
| --- | --- |
| `GET /api/courses` | Preserva `items` e `totalCount`; cada resumo mantém os campos atuais e acrescenta `description` e `shortDescription`. A Landing usa `fullName`, `shortDescription`, `category` e `imageUrl` |
| `GET /api/courses/{id}` | Mantém os campos atuais, acrescenta `shortDescription` e `presentation` tipado; alimenta tanto Detalhes quanto AVA |
| `presentation` | `certification`, `detailsImageUrl`, `onlineLessons`, `assessments`, `faqs`, `learningPreview` |
| `onlineLessons` | `introduction`, `topics` e `accessibility` |
| `assessments` | `introduction`, `topics` e `conclusion` |
| `faqs` | Lista de `question` e `answer` |
| `learningPreview` | Campos editoriais descritos acima, expondo `bannerImageUrl` em vez do caminho persistido |

Os campos de módulo/aula permanecem `id`, `title`, `description`, `order` e, para módulo, `lessons`. Coleções vazias continuam `[]`; `availableOn` continua data de calendário ISO ou `null`. O frontend formata a data sem conversão de fuso; mantém o rótulo visual da referência e usa A definir quando nula, sem criar uma entidade de turma.

A listagem mantém projeção SQL sem carregar documentos de apresentação. Para detalhes, materializar uma projeção intermediária dos campos necessários, módulos/aulas ordenados e documento editorial, depois desserializar e montar os DTOs em memória. Não tentar traduzir desserialização arbitrária para SQL, usar lazy loading ou retornar entidades.

Descrição na listagem é a completa; `shortDescription` é o texto editorial breve. A distinção explícita evita dar significados diferentes ao mesmo campo nos dois endpoints. Não há problema de volume relevante no conjunto inicial de seis cursos e não há consultas individuais por card.

OpenAPI e testes de campos exatos precisam acompanhar a ampliação. Preservar Problem Details, pesquisa literal sem distinção de caixa, escape de curingas, filtros cumulativos e ausência de regras implícitas de status/data. A busca não passa a considerar descrições.

### 5. Imagens resolvidas na resposta, não fixadas no seed

Referências persistidas:

| Uso | Caminho público |
| --- | --- |
| Cards 1 e 6 | `/assets/banner-ava.png` |
| Card 2 | `/assets/hero-image.png` |
| Cards 3, 4 e 5 | `/assets/video-thumb.png` |
| Hero de Detalhes dos seis cursos | `/assets/video-thumb.png` |
| Banner do AVA dos seis cursos | `/assets/banner-ava.png` |

As imagens compartilhadas são a reutilização já aceita, não fotografias exclusivas prometidas. Alt text descreve a imagem efetiva, sem atribuir a ela uma cena inexistente do curso.

Um resolvedor pequeno compõe todas as URLs de imagem expostas a partir da base configurada e caminhos `/assets/` válidos, sem `..`, esquemas externos ou segmentos de filesystem. Exemplo: base `http://127.0.0.1:5173` e caminho `/assets/banner-ava.png` resultam em `http://127.0.0.1:5173/assets/banner-ava.png`. Normalizar barras sem perder a porta.

Não salvar o host no banco, copiar PNGs para `wwwroot`, fazer downloads no seed ou validar a URL por HTTP. A API responde JSON mesmo com o servidor estático desligado; somente a imagem fica indisponível. SVGs antigos do backend deixam de ser referências do seed; não é necessário apagar esses arquivos para concluir a integração.

**Alternativa:** gravar URLs absolutas durante o seed acoplaria seus valores ao ambiente e exigiria recriação para trocar de porta ou host.

### 6. Configuração de origens e execução integrada

Backend: ler a chave exata `FRONTEND_URL` pela configuração de ambiente, validada no início do host HTTP e usada como base das URLs de imagem. Aceitar origem HTTP/HTTPS com barra final opcional; rejeitar branco, URL relativa, credenciais, query, fragmento e subcaminho. O frontend atual é servido na raiz. Não consultar disponibilidade externa para validar a origem.

Usar options com validação de startup e consumo pelo resolvedor, sem tornar o `CatalogDbContext` ou o seed dependentes dessas options. Ferramentas de migrations e testes de persistência precisam continuar operando sem frontend em execução e sem precisar resolver a origem de imagens. Testar essa separação explicitamente.

Uma política CORS permite qualquer origem para os endpoints de leitura, usando `AllowAnyOrigin`, somente GET e os cabeçalhos necessários, sem credenciais. Como os dados do catálogo são públicos e a API não possui operações de escrita, o wildcard é intencional; CORS não é autenticação. `FRONTEND_URL` não participa da decisão CORS e continua definindo o host das imagens. Preflight é infraestrutura do middleware, não um novo endpoint de negócio. Posicionar o middleware na ordem documentada entre roteamento e execução de endpoints, preservando cabeçalhos nas respostas 200/400/404/500.

Frontend: usar `VITE_API_BASE_URL`, com padrão local `http://localhost:5080`, como base de uma única instância Axios. A base é uma origem HTTP/HTTPS válida, sem credenciais, query ou fragmento; as funções do catálogo acrescentam `/api/courses`. Configuração inválida produz erro reconhecível sem loops de tentativa. Não enviar credenciais, tokens ou cabeçalhos de autenticação.

| Ambiente | Backend `FRONTEND_URL` | Frontend `VITE_API_BASE_URL` |
| --- | --- | --- |
| Desenvolvimento documentado | `http://127.0.0.1:5173` | `http://localhost:5080` |
| Preview documentado | `http://127.0.0.1:4173` | O endereço da API usado no build |

Padronizar comandos com host e portas fixos nos exemplos, sem troca silenciosa de porta pelo servidor. `localhost` e `127.0.0.1` são hosts diferentes: mudar host/porta do frontend requer atualizar `FRONTEND_URL` e reiniciar a API para que as imagens apontem ao servidor estático correto; isso não altera o CORS wildcard. Alterar o endereço da API em um build exige recompilar o frontend; iniciar o preview não substitui variáveis já embutidas no bundle.

Criar somente exemplos versionados, não credenciais: incluir `FRONTEND_URL` em `apps/backend/.env.example` e `VITE_API_BASE_URL` em `apps/frontend/.env.example`. O backend continua exigindo exportação no ambiente do processo, sem carregador dotenv. Explicar que variáveis `VITE_` são públicas e não devem carregar segredos.

**Alternativa:** proxy de desenvolvimento foi descartado para esta change; um client com base explícita e CORS público somente para GET funciona igualmente no desenvolvimento e no build/preview, sem pressupor proxy que só exista no Vite dev.

### 7. Axios e TanStack Query com hooks pequenos

Em 2026-10-23, os metadados públicos do npm indicaram Axios `1.20.0` e `@tanstack/react-query` `5.104.5`. O segundo declara peers de React e tipos `^18 || ^19`, compatíveis com as versões existentes. Adotar essas versões estáveis, pinadas e registradas no lockfile na implementação; não atualizar as bibliotecas já presentes. A consulta de metadados não equivale a instalação ou build verificados.

Usar o pacote atual `@tanstack/react-query`, não o pacote legado `react-query`. Estrutura pequena compatível com o projeto:

- `src/app/query-client.ts`: uma instância estável de cache por sessão da aplicação.
- `src/lib/api-client.ts`: instância Axios com base configurável, timeout de 10 segundos e tratamento tipado de falhas.
- `src/features/courses/`: tipos de contratos, funções de consulta, chaves e hooks `useCourses`/`useCourse`.
- Componentes de página recebem o resultado do hook e repassam dados por props às regiões visuais. Não duplicar respostas em outro store global.

Chaves: coleção `['courses', 'list']`; detalhe `['courses', 'detail', id]`, com `id` normalizado como inteiro. Detalhes e AVA usam a mesma consulta de detalhe, pois o DTO já contém ambas as apresentações. Nenhum hook faz consulta de detalhe por item da coleção.

As funções específicas do catálogo validam minimamente as respostas antes de resolver a consulta: coleção com `items`/`totalCount` compatíveis, identidade e campos utilizados pelos cards, e detalhe com ID correspondente, módulos/aulas e apresentação completa nos formatos utilizados pelas telas. Tipos TypeScript não substituem essa validação de runtime. Usar guards pequenos, sem nova biblioteca de schema e sem rejeitar campos adicionais compatíveis. Uma resposta 200 sem `presentation`, por exemplo, gera erro de formato antes de entrar no cache de sucesso; dados válidos anteriores permanecem disponíveis quando houver falha de refetch.

Passar o `signal` da função de consulta à requisição Axios. Cancelamento não vira mensagem de erro. Usar `staleTime` de 60 segundos e retenção de cache em memória de cinco minutos; não persistir cache no navegador. Não preencher uma chave nova com dados do curso anterior.

No máximo uma repetição automática para falha transitória de rede, timeout ou 5xx; nenhuma para 4xx, configuração inválida, resposta incompatível ou cancelamento. A ação Tentar novamente permite novo ciclo explícito com nova requisição. Manter refetch em segundo plano sem trocar conteúdo utilizável por skeleton.

**Alternativas:** `fetch`, outro store de dados, múltiplas instâncias de cache, interceptors de autenticação e uma camada genérica de repositório não atendem ao pedido ou acrescentam complexidade sem benefício.

### 8. Rotas por identificador e aliases compatíveis

Manter o roteamento declarativo atual. Centralizar geradores de caminhos para `/cursos/{id}` e `/ava/{id}` e validar IDs inteiros positivos dentro do intervalo aceito pela API. Parâmetro inválido apresenta Not Found antes de iniciar um hook desabilitado; parâmetro válido inexistente trata o 404 retornado.

Aliases fixos de Matemática redirecionam com substituição de histórico para `/cursos/1` e `/ava/1`. Não criar slugs persistidos, usar títulos para resolver identidade ou transformar slugs desconhecidos em curso 1. A CTA inferior da Landing mantém o destino documentado de Matemática por seu ID de referência, nunca pelo primeiro item da coleção alfabética.

```text
Landing
  |
  +--> useCourses --> GET /api/courses
  |       |
  |       +--> card.id --> /cursos/{id}
  |
  +--> CTA de referencia --> /cursos/1

/cursos/{id} --> useCourse(id) --> GET /api/courses/{id}
       |                                  |
       +--> /ava/{id} --> mesmo cache <----+
                 |
                 +--> retorno explicito --> /cursos/{id}
```

Passar a identidade por props para hero, conteúdo, barra, sidebar e artigo. Remover imports de fixtures de curso desses consumidores e do cálculo de título da aba. `RouteEffects` continua responsável por foco/rolagem na navegação; títulos assíncronos são definidos onde os dados estão disponíveis, com estados genéricos de carregamento e erro. Atualização de dados não repete o salto de foco.

Reiniciar estado local de acordeões/barra por identidade de curso. Montar a observação do fim do hero quando o conteúdo real estiver presente; o efeito atual com montagem única não deve perder o marcador ao haver skeleton inicial. Recalcular limites do carrossel após a resposta. Manter retorno no AVA e navegação móvel acessíveis mesmo durante falha/carregamento, usando o ID válido da rota.

### 9. Erros de consulta e Error Boundaries com responsabilidades distintas

Usar consultas normais, sem Suspense nem propagação automática de todo erro HTTP para a renderização. Erros previstos de consulta são tratados por `isError`/dados disponíveis: vazio é sucesso, 404 é não encontrado, falha inicial é painel genérico com nova tentativa, falha de refetch mantém dados do mesmo curso com aviso discreto.

Adicionar um Error Boundary reutilizável nativo do React, com alternativa genérica e reinicialização por contexto de rota, nas regiões de curso. Adicionar proteção final na raiz para falhas que escapem das regiões; sua alternativa não depende de providers que possam ter falhado. Não é necessário outro pacote de boundary.

O boundary captura exceções de renderização de descendentes; não captura automaticamente rejeições de Axios, eventos ou erros em seu próprio fallback. Por isso a consulta precisa do tratamento explícito acima. A ação de nova tentativa da consulta chama o refetch. Na recuperação de uma região de curso, reiniciar o boundary e a consulta correspondente, obtendo outra resposta em vez de apenas renderizar novamente o mesmo cache. O retorno à Landing ou outra rota não conserva a falha anterior. Uma falha recorrente mantém alternativa de saída e não deve criar loop de reload.

Não mostrar stack trace, configuração, conteúdo bruto de Problem Details ou segredos ao usuário. Registro técnico, se utilizado, é local e não introduz analytics ou um serviço externo de monitoramento.

**Alternativas:** confiar apenas no boundary deixaria falhas assíncronas sem tratamento; lançar todo erro de refetch apagaria conteúdo útil; adicionar Suspense e uma biblioteca de boundaries não é necessário para este escopo.

### 10. Skeletons por região, sem dados fictícios utilizáveis

Usar `Skeleton` do Mantine existente. `isPending` representa ausência inicial de dados; `isLoading` em Query v5 também exige que a consulta esteja buscando. Tratar identificador/configuração inválidos antes desses estados para não mostrar skeleton infinito. `isFetching` sozinho não substitui conteúdo em cache por placeholders.

| Tela | Placeholder |
| --- | --- |
| Landing | Trilho com seis posições de card e mesmos recortes/dimensões; restante institucional permanece renderizado |
| Detalhes | Hero em duas colunas com espaços de badge, título, metadados e imagem; área de descrição/acordeões com as mesmas larguras; sem barra ou matrícula acionável antes dos dados |
| AVA | Sidebar bicolor, espaço de identificação e progresso, banner e blocos de artigo/recomendações; retorno segue acessível pelo ID da rota |

Usar regiões com `aria-busy`, mensagem de status breve e skeletons decorativos ocultos da árvore assistiva. Não usar títulos reais de mocks como conteúdo encoberto porém legível por tecnologia assistiva. Desativar animação não essencial em movimento reduzido. Reservar proporções de imagens e manter a adaptação em 1440, 1024, 768 e 375 px.

Quando carregados, os componentes preservam os mesmos headings, colunas, trilhos e acordeões. Não adicionar controles para todos os campos disponíveis na API. Status em breve/encerrado não bloqueia uma prévia que não realiza matrícula.

### 11. Migrations, seed e verificação sem ampliar ferramentas

Manter `InitialCreate` e seu arquivo `.Designer.cs` intactos; gerar uma migration incremental para `ShortDescription` e `PresentationJson` e atualizar o snapshot corrente `CatalogDbContextModelSnapshot` com o `dotnet-ef` local na fase de implementação. Revisar o mapeamento `jsonb` e a ausência de default de string vazia, que não é JSON válido. Para o fluxo de banco vazio adotado, as novas colunas obrigatórias podem ser adicionadas sem valores default; o seed roda depois de todas as migrations.

Não tentar reinterpretar IDs antigos como cursos novos por upsert. O caminho de transição autorizado é recriar o banco local confirmado e aplicar toda a sequência. Em banco já populado e sem transição, a migration pode exigir a preparação explícita; não mascarar dados editoriais ausentes com documentos vazios. Após a transição, o seed volta a inserir somente IDs ausentes, sem sobrescrever conteúdo existente.

Atualizar testes atuais em vez de enfraquecer invariantes para acomodar o seed: obrigatoriedade e limites de campos, diversidade, round-trip editorial, idempotência e estrutura vazia continuam cobertos. Adaptar expectativas de nomes, categorias e aliases; testar pesquisa exclusiva no nome breve e todos os filtros existentes. A terceira categoria agora retorna coleção vazia quando filtrada, não dois cursos.

`CatalogApiFactory` deve receber `FRONTEND_URL` válida, sem servidor Node. Testar composição de URLs com duas bases sobre o mesmo banco e CORS wildcard em mais de uma origem, inclusive erro, sem credenciais e sem método de escrita no preflight. Testes de imagens deixam de exigir SVGs servidos pelo backend: verificam caminhos/URLs e existência dos assets no monorepo; o navegador confirma a entrega HTTP pelo frontend.

Não há runner unitário frontend atualmente. Usar a automação de navegador disponível para verificar os fluxos e falhas, sem instalar uma suíte adicional apenas por convenção. Interceptar respostas em uma sessão de teste para atraso, vazio, 404, 5xx e HTTP 200 sem `presentation`; neste último caso, comprovar rejeição sem repetição automática, ausência de dado inválido no cache e nova requisição ao recuperar. Provocar também uma exceção de renderização em ambiente isolado para comprovar o boundary e a recuperação com consulta reiniciada, sem adicionar rota ou flag de falha ao produto. Build e tipos não provam esses comportamentos.

Matriz mínima de aceite na implementação:

| Área | Verificação |
| --- | --- |
| Seed e banco | Banco vazio, novo esquema, seis cursos, três classificações de categoria, dois modos, três status, apresentações completas e repetição sem alteração |
| API | Descrições na coleção, apresentação dos seis detalhes, pesquisa/filtros/contagem, ordenação, vazio, 400/404/405/500 e OpenAPI |
| Imagens e origem | Caminhos do seed sem host, URLs com duas bases, CORS wildcard somente para GET, imagens 200 no frontend e nenhum download durante o seed |
| Navegação | Seis ciclos Landing/Detalhes/AVA/retorno, entradas diretas, refresh, aliases e histórico |
| Carregamento | Resposta atrasada nas três telas, layout preservado, sem controles de curso prematuros, movimento reduzido e sem skeleton ao refetch com dados |
| Erros | API indisponível, timeout, vazio, ID inválido, 404, 5xx, 200 incompatível, recuperação com nova consulta, exceção de renderização e saída de contexto |
| Identidade/cache | Detalhes e AVA do mesmo ID compartilham dados; A para B com atraso não mistura títulos/artigos/retorno; nenhuma consulta de detalhe por card |
| Regressão visual | Seções completas, acordeões, barra após hero assíncrono, limites do trilho, footer alcançável e quatro larguras de referência |
| Limites | Sem filtros, busca, catálogo adicional, ranking, novas ações de footer, login, escrita, player ou progresso real |

### Referências técnicas consultadas

- [TanStack Query v5: estados de consulta](https://tanstack.com/query/latest/docs/framework/react/guides/queries), cancelamento e tratamento de erros, consultados via Context7.
- [Axios: instâncias](https://axios-http.com/docs/instance) e [cancelamento por AbortSignal](https://axios-http.com/docs/cancellation), consultados via Context7.
- [Mantine Skeleton](https://mantine.dev/core/skeleton/), consultado via Context7; manter a versão já instalada.
- [React: Error Boundaries](https://react.dev/reference/react/Component#catching-rendering-errors-with-an-error-boundary).
- [Npgsql: mapeamento JSON](https://www.npgsql.org/efcore/mapping/json.html), incluindo o suporte a string `jsonb` e a alternativa EF 10 `ToJson()`.
- [ASP.NET Core 10: CORS](https://learn.microsoft.com/aspnet/core/security/cors?view=aspnetcore-10.0) e [validação de options](https://learn.microsoft.com/aspnet/core/fundamentals/configuration/options?view=aspnetcore-10.0), consultados no Microsoft Learn.
- Metadados de versões e peers do registro npm de Axios e `@tanstack/react-query`, conferidos em 2026-10-23 sem instalação.

## Risks / Trade-offs

- [PRD pede três categorias, mas a Landing usa duas] → Manter Linguagens cadastrada e sem cursos; não alterar badges nem quantidades de cards.
- [Conteúdo dos cinco novos detalhes não existe na referência] → Usar o roteiro fictício explícito acima, mantendo estrutura e dados consistentes; não alegar transcrição desses cursos.
- [Documento editorial JSONB não possui toda a forma imposta por constraints relacionais] → Tipos internos, validação de seed e testes de round-trip, sem JSON livre no contrato HTTP.
- [Estado do AVA pode parecer real] → Manter indicação acessível de prévia, progresso fixo e ausência de escritas, com payload separado da estrutura educacional real.
- [Troca de origem afeta URLs de imagem] → Normalizar `FRONTEND_URL` e manter exemplos coerentes de host/porta; CORS de leitura não depende dessa configuração.
- [API cai após conteúdo em cache] → Preservar apenas dados do mesmo curso com aviso; não mascarar falha inicial com mocks.
- [Observers atuais pressupõem DOM síncrono] → Montagem por estado/identidade, cleanup e teste com carregamento atrasado e navegação rápida.
- [Recriação apaga dados do alvo errado] → Conferir host, banco e conexão local antes do comando; não remover volume, outros bancos ou executar reset no startup.
- [Desvio de imagem entre API e tela] → Resolver também as imagens de Detalhes e AVA, não apenas a do card; preservar os arquivos originais.

## Migration Plan

1. Na aplicação futura, adicionar modelo/contratos/seed e uma migration incremental, sem reescrever a inicial. Implementar configuração de origem e testes antes de depender das telas.
2. Confirmar que a conexão aponta apenas ao banco local de desenvolvimento autorizado. Parar o processo da API desse projeto, se necessário para liberar conexões; recriar somente esse banco, sem apagar o volume. Nenhum desses passos é executado no planejamento.
3. A partir de `apps/backend`, restaurar a ferramenta local e dependências, aplicar a sequência versionada e o seed; repetir a atualização para verificar idempotência. Não exigir frontend em execução para preparar dados.
4. Configurar `FRONTEND_URL`, iniciar a API e validar contratos e CORS; integrar client, hooks, rotas e estados no frontend, instalar as duas dependências pinadas e atualizar apenas o lockfile compartilhado.
5. Executar `dotnet build` e `dotnet test` no backend e `pnpm frontend:typecheck`/`pnpm frontend:build` na raiz. Testes relacionais usam Testcontainers, não o banco local recriado.
6. Executar a matriz de navegador em desenvolvimento e preview com endereços coerentes. Atualizar READMEs e exemplos, registrando verificações efetivamente executadas e impedimentos.
7. Não há migração de dados antigos preservados nesta transição local. Rollback exige código/contratos compatíveis com a versão escolhida e uma nova preparação explícita do banco local, se necessária; não executar downgrade destrutivo ou limpeza de volume automaticamente. Preservar todo trabalho não pertencente à change.
