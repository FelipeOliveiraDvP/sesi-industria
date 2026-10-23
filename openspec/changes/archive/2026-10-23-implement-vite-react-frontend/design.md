# Design

## Context

A motivação está em `proposal.md`. A inspeção de 2026-10-23 encontrou:

- `apps/frontend` contém somente `public/assets`, com 65 arquivos: três PNGs, 22 SVGs, 38 TTFs e dois TXT. Não existe código, teste ou configuração de frontend a reaproveitar.
- A raiz não possui `package.json`, `pnpm-workspace.yaml`, lockfile JavaScript, tsconfig ou configuração de lint. O backend .NET e seu Compose já são independentes e não serão alterados.
- Foram lidos os seis documentos de `docs/figma-reverse-engineering/`: README, inventário, design system e as três páginas. As evidências locais cobrem desktop e três frames: `6017:342`, `6017:707` e `6017:1042`.
- O MCP oficial do Figma recusou a criação de sessão por ausência de acesso de edição. Não há leitura nativa de tokens, árvore ou componentes disponível; nenhuma edição do design foi tentada.
- O usuário aprovou as três URLs propostas e o uso de recortes dos PNGs existentes nos cards sem fotos próprias. Essas decisões resolvem as lacunas materiais para este planejamento.

Os contratos de comportamento estão nas cinco specs desta change. O PRD e a change anterior descrevem a API, não autorizam acrescentar pesquisa, filtros, classificações ou fluxos remotos ao frontend visual.

## Goals / Non-Goals

**Goals:**

- Separar a fidelidade visual das dependências de dados: a mesma fixture determinística alimentará acesso direto, navegação e conferência por screenshots.
- Manter roteamento, providers e identidade centralizados, mas permitir diferenças reais entre marketing, Detalhes e AVA.
- Usar Mantine como base sem impor sua aparência padrão aos layouts documentados.
- Tornar explícitas as aproximações de fontes, medidas, responsividade e fotografias, sem ampliar o produto para compensar lacunas do protótipo.

**Non-Goals:**

- Não criar camadas de API/repositório, backend falso, contrato compartilhado com DTOs .NET, estado global complexo ou infraestrutura de autenticação.
- Não transformar os textos sobre aulas, fóruns, antifraude e certificados em funcionalidades.
- Não buscar conteúdo remoto, exportar assets do Figma por meios alternativos ou alterar os recursos fornecidos para otimização.
- Não adicionar SSR, modo framework do React Router, bibliotecas de carrossel, outros kits de UI, ícones em pacote ou lint apenas por convenção.

## Decisions

### 1. Evidências locais e decisões aprovadas como referências

Usar os textos dos documentos de página para conteúdo integral e suas capturas para aparência e estados. O design system fornece cores amostradas e estimativas, não medidas absolutas. As coordenadas das screenshots incluem escala e moldura do Figma: não devem virar posicionamento fixo da aplicação.

Divergências identificadas e tratamento:

| Observação | Decisão |
| --- | --- |
| O texto da Landing enfatiza header vermelho; capturas de cursos, métricas, depoimentos e footer mostram header branco fixo. | Implementar variante integrada ao hero no topo e branca na rolagem, com as duas versões locais da marca. |
| O texto do AVA resume a sidebar como escura; `assets/003-ava/viewport.png` e `section-03-bottom.png` mostram busca e módulos em branco. | Manter somente retorno, identificação e progresso escuros; corpo da navegação claro. |
| A barra de matrícula encobre o final do footer nas capturas. | Preservar a barra flutuante, mas reservar espaço inferior e margem de rolagem/foco para não reproduzir a obstrução de conteúdo. |
| Detalhes apresenta dez módulos; o AVA mostra Introdução e seis módulos genéricos. | Preservar cada conjunto como fixture visual própria, sem inventar uma correspondência entre as listas. |
| Alguns textos citam segurança do trabalho, NRs, reconhecimento facial e prazos. | Transcrever o conteúdo sem corrigir editorialmente a referência nem implementar as funcionalidades mencionadas. |

**Alternativa rejeitada:** extrair requisitos novos do PRD do backend, tratar estimativas de raster como tokens exatos ou reconstruir o chrome do Figma. Isso ampliaria o escopo ou reduziria a fidelidade ao produto.

### 2. Stack estável e compatibilidade declarada verificada

Context7 foi consultado para Vite, React, TypeScript, React Router, Mantine e pnpm. Os patches estáveis abaixo foram conferidos adicionalmente nos metadados públicos do npm em 2026-10-23; não houve instalação ou teste de compatibilidade executável nesta fase.

| Dependência | Base para a implementação | Justificativa e compatibilidade |
| --- | --- | --- |
| Node.js | Linha 24 LTS; ambiente atual `24.15.0` | Satisfaz Vite 8 e pnpm 11.9. O mínimo conjunto é Node `22.13`, devido ao pnpm. |
| pnpm | `11.9.0`, já disponível | Fixar em `packageManager`; não atualizar o ambiente para a linha 12 sem necessidade. |
| `react` / `react-dom` | `19.3.1`, mesma versão | React DOM exige React compatível; satisfaz o peer `^19.2.0` do Mantine 9. |
| `react-router-dom` | `7.18.4` | Modo declarativo; React Router 7 continua publicando este pacote como reexport, compatível com React 19 e Node 20+. |
| `@mantine/core` / `@mantine/hooks` | `9.7.3`, versões idênticas | Única biblioteca de componentes e seus hooks; o core exige a mesma versão de hooks. |
| `vite` / `@vitejs/plugin-react` | `8.3.5` / `6.1.2` | O plugin declara peer Vite `^8.0.0`; seus peers de React Compiler/Babel são opcionais e não serão instalados sem uso. |
| `typescript` | `7.0.2` estável | Compilador do comando `tsc`, com configuração estrita e resolução para bundler. Não utilizar pacote preview. |
| Tipos | `@types/react` / `@types/react-dom` `19.3.0`; `@types/node` linha 24 | Alinhar tipos ao React e ao runtime adotados, resolvendo os patches no lockfile. |
| PostCSS | `postcss` `8.5.31`, `postcss-preset-mantine` `1.18.0`, `postcss-simple-vars` `7.0.1` | Setup oficial para estilos Mantine, mixins responsivos e variáveis de breakpoints; somente dependências de desenvolvimento. |

Referências consultadas via Context7: [Vite 8 e requisitos Node](https://github.com/vitejs/vite/blob/v8.0.10/docs/blog/announcing-vite8.md), [assets públicos do Vite](https://github.com/vitejs/vite/blob/v8.0.10/docs/guide/assets.md), [React StrictMode](https://react.dev/reference/react/StrictMode), [TypeScript para bundlers](https://www.typescriptlang.org/docs/handbook/modules/guides/choosing-compiler-options.html), [roteamento declarativo](https://reactrouter.com/start/declarative/routing), [Mantine com Vite](https://mantine.dev/guides/vite/), [tema Mantine](https://mantine.dev/theming/theme-object/), [workspace pnpm](https://pnpm.io/workspaces).

**Alternativas rejeitadas:** templates com Tailwind/shadcn, atualização indiscriminada de pnpm, React Router framework/SSR, clientes HTTP e pacotes de carrossel. Não resolvem necessidades deste escopo e acrescentam dependências ou convenções concorrentes. Na aplicação, revalidar os peers ao instalar e registrar qualquer ajuste de patch necessário, sem mudar silenciosamente as linhas escolhidas.

### 3. Workspace mínimo e configuração estrita

- Criar na raiz um `package.json` privado com `packageManager: pnpm@11.9.0`, indicação de Node suportado e comandos de conveniência delegados ao pacote `@sesi/frontend`.
- Criar `pnpm-workspace.yaml` incluindo explicitamente `apps/frontend`. Não transformar `apps/backend` em pacote JavaScript.
- Gerar somente `pnpm-lock.yaml` na raiz durante a aplicação. Ignorar `node_modules`, `apps/frontend/dist` e temporários de ferramentas, preservando as regras existentes de .NET e os assets.
- Criar arquivos de entrada e configuração manualmente, sem executar um scaffold sobre `public/` que possa sobrescrever arquivos ou acrescentar assets de exemplo do Vite/React.
- Usar Vite como SPA cliente com plugin React, sem proxy de API, plugins de framework, configuração de backend ou variáveis de endpoint. Manter `base` na raiz, coerente com as URLs aprovadas.
- Separar `tsconfig.app.json` e `tsconfig.node.json`, ambos estritos, com `noEmit`; o primeiro cobre DOM/JSX e o segundo a configuração Vite. Usar resolução `bundler`, módulos compatíveis, imports de tipos explícitos e verificações de variáveis não usadas e acesso indexado seguro.
- `typecheck` verifica os dois projetos sem emitir JavaScript. `build` executa a verificação de tipos antes de `vite build`. `dev` inicia Vite e `preview` serve o build local; preview não é um servidor de produção.
- Não usar `any`, `@ts-ignore`, coerções para mascarar erros ou relaxamento de verificações para concluir a entrega. Como não há lint configurado, não instalar um linter; registrar como não aplicável, não como aprovado.

**Alternativa rejeitada:** pacote isolado com lockfile dentro do frontend ou ampla configuração compartilhada de TypeScript. O primeiro fragmenta o monorepo; a segunda não tem outro consumidor atual.

### 4. Composição modular sem camadas de dados remotos

Estrutura orientativa para a aplicação futura:

```text
apps/frontend/
  public/                         # preservado integralmente
  src/
    app/                          # App, router, providers, routes, theme, assets
    components/
      layout/                     # MarketingLayout, SiteHeader, SiteFooter
      shared/                     # BenefitList, HorizontalRail, Testimonials, VideoPreview
    features/
      home/                       # cards de curso, impacto e blocos específicos
      course-details/             # informações expansíveis e EnrollmentBar
      learning-environment/       # layout do AVA, sidebar e artigo
    pages/                        # LandingPage, CourseDetailsPage, AvaPage, NotFoundPage
    mocks/                        # fixtures tipadas de conteúdo, sem servidor simulado
    styles/                       # fontes e estilos globais; CSS Modules junto aos componentes
    types/                        # apenas tipos realmente compartilhados
    main.tsx
    vite-env.d.ts
  index.html
  package.json
  tsconfig.json
  tsconfig.app.json
  tsconfig.node.json
  vite.config.ts
  postcss.config.cjs
  README.md
```

`main.tsx` cria uma única raiz React, com StrictMode e providers centrais. Importar os estilos oficiais de `@mantine/core` antes dos overrides da aplicação. O provider aplica um único tema e usa `forceColorScheme="light"`, sem controle ou persistência de preferência de tema; essa opção tem precedência sobre o gerenciador de esquema padrão, conforme a documentação consultada. O roteador é centralizado, sem loaders, actions remotas ou rotas geradas por fixtures.

As fixtures contêm somente o necessário à apresentação: seis cards, quatro métricas, benefícios, cinco depoimentos compartilhados, detalhes com dez módulos e dez FAQs, conteúdo de aulas/avaliações e o artigo do AVA. Usar tipos explícitos para esses dados e props; estados visuais de lição serão uniões tipadas, não dados de matrícula. Não importar imagens do `wwwroot` do backend, cujos SVGs são ilustrativos e não correspondem às fotos do protótipo.

Criar compartilhamento apenas onde há repetição: mesmos depoimentos com duas variantes visuais, benefícios em três/quatro colunas, preview de vídeo sem player e trilhos horizontais finitos. Não criar componentes vazios, repositórios, factories de telas, contextos globais de curso ou um LMS genérico.

**Alternativa rejeitada:** uma página monolítica, ou uma camada de domínio idêntica ao backend. A primeira dificulta ajuste visual e reutilização; a segunda introduz conceitos sem utilidade em uma demonstração estática.

### 5. Rotas fixas, acesso direto e política explícita de ações

As URLs foram aprovadas pelo usuário, pois os documentos fornecem somente nós do Figma. Não são rotas extraídas do protótipo.

| URL | Composição | Estado inicial e saída principal |
| --- | --- | --- |
| `/` | MarketingLayout na variante Landing + LandingPage | Hero, trilhos no início; primeiro card e CTA final abrem Detalhes. |
| `/cursos/matematica-para-a-precisao` | MarketingLayout com header não fixo + CourseDetailsPage | Três acordeões fechados, barra oculta no topo; Matricule-se abre AVA. |
| `/ava/matematica-para-a-precisao` | Layout próprio do AVA + AvaPage | Boas práticas, progresso de 10%; seta aponta explicitamente para Detalhes. |
| `*` | NotFoundPage com identidade compartilhada | Mensagem de página não encontrada e link à Landing. |

Usar BrowserRouter, Routes/Route, Outlet quando útil e Link/NavLink de `react-router-dom`. O slug é fixo: não há necessidade de `:courseId`, rotas de outros cursos ou roteamento de lições. Centralizar as URLs para evitar strings divergentes entre CTAs.

- `Cursos` e `Conhecer cursos` na Landing usam a seção `cursos`, com âncora local e `scroll-margin-top` para o header. Não criar `/cursos` como página separada.
- `Início` no header aponta à Landing. O comportamento de Cursos nos Detalhes não foi confirmado; não estender automaticamente a ação da Landing a esse contexto.
- Os demais cards, Todos os cursos, Ver todos os cursos, Sobre, Área do aluno, links do footer, contato e plays sem mídia seguem indisponíveis ou não interativos, sem `href` fictício, novas abas, alertas ou toasts.
- Busca e itens da sidebar do AVA não filtram nem alternam conteúdo. O campo é somente leitura/indisponível com nome acessível; não adicionar placeholder, resultados ou mensagens inexistentes.
- Explicar a natureza demonstrativa no README, no título do documento do AVA e em descrições acessíveis junto às CTAs/progresso, preservando a copy visual dos botões. Não emitir confirmação de matrícula.
- Definir título de documento por rota, foco no conteúdo após navegação e política simples de rolagem: novas entradas mostram o topo, a âncora de cursos tem precedência e o histórico mantém URLs e conteúdo corretos. Não prometer persistência do estado de acordeões ou da posição anterior.

Os servidores de desenvolvimento e preview devem atender o fallback da SPA, a ser verificado abrindo cada URL diretamente. Para hospedagem estática futura, documentar rewrite de caminhos de aplicação para `index.html`, sem redirecionar arquivos estáticos ausentes para HTML. Não implementar infraestrutura de deploy nesta change.

**Alternativas rejeitadas:** HashRouter, `navigate(-1)` na seta do AVA ou destinos inventados para dar ação a todo elemento. As duas primeiras prejudicam URLs ou entrada direta; a última viola os limites observados.

### 6. Tema Mantine derivado das referências, sem identidade alternativa

Manter `createTheme` e overrides em `src/app/theme.ts`, variáveis de superfície/layout em estilos globais e CSS Modules para a composição das seções. Usar o tema nos componentes Mantine e variáveis compartilhadas nos estilos, sem recriar um segundo design system.

| Token de referência | Aplicação |
| --- | --- |
| `#FFFFFF` | Superfícies principais e cards claros. |
| `#0C0C0D` | Texto, botões escuros e topo da sidebar. |
| `#890000` / `#C3222C` | Fundos institucionais e cards de depoimento na Landing. |
| `#990212` | Base de gradientes dos cards de curso. |
| `#FCD19C` / `#E4CCFF` | Pêssego e badge de Ciências da Natureza. |
| `#1D0300` / `#9C908F` | Rodapé e texto secundário, sujeito a ajuste de contraste quando necessário. |
| Próximo de `#E5E5E5` | Bordas/divisores estimados, ajustados pela conferência visual. |

Derivar a paleta de dez tons exigida pelo Mantine preservando as cores de referência nos papéis relevantes; os tons derivados são decisões de implementação, não tokens extraídos. Configurar Open Sans como família de corpo e títulos, pesos apropriados, escala de espaçamento, raios e sombras discretas. Tamanhos de títulos, container por volta de 1200–1220 px e raios de aproximadamente 8–10 px são pontos de partida visuais, não restrições rígidas de raster.

O hero da Landing tem H1 grande, texto à esquerda e fotografia recortada à direita; Detalhes usa título menor e duas colunas; o AVA usa largura de leitura e respiro sem cards envolvendo os parágrafos. Gradientes e overlays pertencem às imagens/seções correspondentes, não a todos os componentes.

**Alternativa rejeitada:** utilizar tema Mantine padrão, assumir Inter a partir do chrome do Figma ou escolher outra identidade por preferência estética.

### 7. Inventário e aplicação dos assets preservados

Todos os caminhos abaixo são relativos a `apps/frontend/public/assets`; no browser começam por `/assets/`, sem o segmento `public`. Centralizar as referências usadas pela aplicação, respeitando caixa e nomes, inclusive `icon-linkedIn-24px.svg` e as vírgulas dos nomes TTF.

| Grupo | Arquivos e aplicação |
| --- | --- |
| Fotografias | `hero-image.png` 1920×1080: capa da Landing; `video-thumb.png` 1920×1080: CTA da Landing e hero de Detalhes; `banner-ava.png` 1940×1080: banner panorâmico do AVA. |
| Fotografias dos seis cards | Não existem arquivos individuais. Usar `banner-ava.png` nos cards 1 e 6, `hero-image.png` no card 2 e `video-thumb.png` nos cards 3, 4 e 5, com recortes e overlays próprios. É reutilização aprovada, não equivalência temática/fotográfica prometida. |
| Marca | `logo-sesi-industria-primary.svg`, `logo-sesi-industria-white.svg` e `logo-sesi.svg`: variantes do header/footer, marca no banner e ícone da página quando adequado. |
| Benefícios | `01-certificacao-red.svg`, `02-professores-red.svg`, `03-suporte-red.svg` na Landing; variantes `-bege.svg` equivalentes e `04-flexibilidade-bege.svg` nos Detalhes. |
| Categorias | `icon-matematica-20px.svg` e `icon-ciencias-natureza-20px.svg` nos badges. `icon-linguagens-20px.svg` e `icon-sustentabilidade-20px.svg` permanecem preservados, sem criar categorias para utilizá-los. |
| Navegação e estados | `arrow-navigation.svg`, `arrow-navigation-1.svg`, `add_2.svg`, `remove.svg` e `star.svg`: setas, acordeões e depoimentos, com tamanho/cor apropriados sem alterar os originais. |
| Redes sociais | `icon-linkedIn-24px.svg`, `icon-instagram-24px.svg`, `icon-facebook-24px.svg` no footer como conteúdo visual sem inventar URLs de perfis. |
| Fontes | `_Font/OpenSans-VariableFont_wdth,wght.ttf` e sua variante itálica: declarar `@font-face` local, `font-display: swap` e pesos suportados. Carregar apenas faces necessárias; preservar as 36 variantes de `_Font/static/`, `README.txt` e `OFL.txt`. |

Aplicar `object-fit`/`object-position`, proporções reservadas e lazy loading fora da dobra. O banner do AVA é uma foto 16:9 recortada para faixa panorâmica; a marca branca é sobreposta a partir do SVG, não presumida como parte do PNG. Não duplicar PNGs para realizar recortes que CSS resolve.

Para os poucos símbolos ausentes, como lupa, calendário, play e estados de lição, usar CSS ou SVG inline mínimo e sem dependência adicional. Isso não substitui ícones já fornecidos nem introduz outro kit de componentes. Preservar os 65 arquivos byte a byte; não remover variantes de fontes só porque não serão requisitadas.

**Alternativas rejeitadas:** usar screenshots de páginas como interface, recortar fotos já degradadas das evidências, consumir imagens remotas ou copiar os SVGs genéricos do backend.

### 8. Interações simples, locais e verificáveis

| Comportamento | Implementação prevista |
| --- | --- |
| Carrosséis de cursos/depoimentos | Um trilho compartilhado com lista semântica, CSS scroll-snap, rolagem nativa e ActionIcons Mantine. Avançar/retornar pela largura visível ou item; calcular limites após scroll e resize, sem autoplay, loop ou dependências Embla. |
| Acordeões | Accordion Mantine em modo múltiplo, inicialmente vazio, com os três grupos e ícones locais de mais/menos. O conteúdo extenso permanece no fluxo normal. |
| Header da Landing | Marcador/observação do hero controla a variante clara após a capa. O comportamento fixo não será aplicado ao header de Detalhes. |
| Barra de matrícula | Observar um marcador ao final do hero; mostrar somente quando esse limite tiver passado acima do viewport, não quando estiver abaixo da dobra inicial. Reservar espaço e scroll padding para o conteúdo/foco. |
| AVA | Layout desktop de altura de viewport com coluna principal rolável e sidebar fixa; fixtures constantes para progresso e lição. No mobile, a sidebar será acessível por Drawer Mantine. |
| Navegação móvel de marketing | Burger e Drawer Mantine somente para acomodar os mesmos itens, sem acrescentar funções de negócio. Fechar após navegação, com Escape e retorno de foco. |

Efeitos e observers precisam de cleanup para funcionar corretamente sob StrictMode. O estado fica no componente correspondente; não usar stores externos, localStorage, sessionStorage, IndexedDB, cookies de usuário ou mocks de transporte HTTP.

**Alternativa rejeitada:** uma arquitetura de eventos/progresso ou um servidor mock para conteúdo conhecido. Estados locais e APIs nativas do navegador bastam.

### 9. Responsividade e acessibilidade como adaptações declaradas

Os documentos não confirmam layouts mobile. Adotar inicialmente breakpoints Mantine `36em`, `48em`, `62em`, `75em` e `88em`, mantendo os valores consistentes entre tema e PostCSS. Abaixo de `62em`, empilhar hero de Detalhes e colunas extensas conforme a legibilidade; até `48em` inclusive, usar navegação recolhível e o Drawer do AVA. Ajustes de encaixe podem ser refinados sem alterar conteúdo, rotas ou funções.

- Usar grids flexíveis para métricas, benefícios e footer; não fixar a largura do frame nas coordenadas da captura.
- Tratar trilhos como a única rolagem horizontal intencional. Cards devem permitir leitura completa de títulos/descrições em viewport pequeno.
- Usar headings em sequência, landmarks, texto selecionável, nomes de botões de ícone, `aria-expanded` nos controles reais e indicação correta de indisponibilidade.
- Preservar aparência das ações ilustrativas sem usar links falsos: texto não interativo quando possível; quando a affordance de botão precisar permanecer, bloquear sua ativação e comunicar `aria-disabled`/descrição. Nunca usar `href="#"` como substituto de destino.
- Tratar a busca do AVA como campo somente leitura/indisponível e rotulado, não como formulário que promete resultados.
- Manter foco visível, foco de rota e fechamento de Drawer corretos. Barra de matrícula e sidebar não podem cobrir elementos focados.
- Conferir contraste; se um tom secundário estimado não atender à legibilidade, ajustar minimamente e registrar a divergência, preservando a identidade.
- Respeitar `prefers-reduced-motion`; desabilitar rolagem suave/animação não essencial nessa condição.

**Alternativa rejeitada:** inventar um design mobile independente, manter colunas espremidas ou preservar uma obstrução do protótipo em detrimento da acessibilidade.

### 10. Validação técnica e visual na fase de aplicação

Não há comandos de frontend executáveis nesta fase de planejamento. Na implementação, usar os comandos da raiz:

| Verificação | Comando ou procedimento e resultado esperado |
| --- | --- |
| Dependências | `pnpm install`; um lockfile na raiz, peers compatíveis e nenhum pacote UI concorrente. Conferir também instalação com `--frozen-lockfile` após a geração do lock. |
| Tipos | `pnpm --filter @sesi/frontend typecheck`; código zero, cobrindo src e configuração Vite sem emissão. |
| Produção | `pnpm --filter @sesi/frontend build`; distribuição produzida sem erros de tipos/empacotamento. |
| Desenvolvimento | `pnpm --filter @sesi/frontend dev --host 127.0.0.1`; rotas e interações disponíveis sem API/banco. |
| Preview | `pnpm --filter @sesi/frontend preview --host 127.0.0.1`; abrir e atualizar as três URLs e uma desconhecida sobre o build. |
| Lint | Não aplicável no estado inspecionado; só executar se uma ferramenta for legitimamente incorporada ao projeto antes dessa etapa. |
| Preservação | Comparar lista e hashes dos assets antes/depois; conferir nomes públicos, imagens renderizadas, SVGs e fontes efetivamente carregadas. |

Usar o navegador automatizado disponível, sem instalar uma suíte adicional apenas para estas verificações. Capturar evidências locais e registrar o resultado real desta matriz:

| Área | Casos mínimos |
| --- | --- |
| Landing | Hero e header inicial/rolado; seis cursos e card final; avanço/retorno/limites; métricas e benefícios; cinco depoimentos; CTA e footer; âncora cursos; ambas as entradas em Detalhes. |
| Detalhes | Hero, data/certificação e quatro benefícios; três acordeões fechados/abertos simultaneamente/recolhidos; dez módulos, aulas/avaliações e dez FAQs completos; barra oculta/visível; depoimentos e footer alcançável; duas entradas no AVA; Início. |
| AVA | Entrada direta, banner, sidebar bicolor, estados das lições, progresso de 10%, sete parágrafos, rolagem independente e retorno determinístico; controles ilustrativos sem novos efeitos. |
| Navegação | Três rotas diretas, refresh em desenvolvimento e preview, histórico, URL inexistente e slug desconhecido. |
| Responsividade | Larguras de 1440, 1024, 768 e 375 px; incluir estados abertos e texto longo, não somente a dobra inicial. |
| Acessibilidade | Percurso de teclado, foco, rótulos, expansão, Drawer/Escape/retorno de foco, contraste e movimento reduzido. |
| Console e rede | Nenhum erro de página, recurso quebrado ou aviso relevante introduzido; nenhuma chamada de dados ao backend/terceiros ou recurso de outro origin. Tráfego local de HMR em desenvolvimento é permitido. |
| Simulação | Nenhuma autenticação, matrícula, envio, progresso persistido, resultado de busca ou aula adicional fictícios. |

A comparação visual deve recortar/normalizar a área do produto das evidências de `docs/figma-reverse-engineering/assets`, sem exigir igualdade de pixels com o visualizador escalado. O desvio das fotos dos cards é aceito; conteúdo incompleto, layout genérico ou ausência de estados não são compensados por essa exceção. O relatório final da implementação deve cobrir páginas/rotas, componentes, decisões, assets, dependências, configurações, comandos, resultados executados e limitações.

**Alternativa rejeitada:** considerar o scaffold ou apenas o build como aceite completo, ou declarar sucesso de inspeções que não puderam ser realizadas.

## Risks / Trade-offs

- [Tokens e medidas são inferidos de raster] → Ajustar contra as evidências por seção e distinguir estimativa de valor confirmado, sem alegar extração nativa.
- [Fotos individuais dos cursos ausentes] → Reutilização local autorizada e mapeada; registrar a divergência no README e no relatório, sem recorrer à rede.
- [Fonte original não identificada] → Usar Open Sans fornecida, comparar quebras de linha e refinar pesos/tamanhos sem baixar outra fonte.
- [Documentação mobile inexistente] → Tratar breakpoints e Drawer como adaptações técnicas; validar a matriz de larguras com o mesmo conteúdo.
- [Barra fixa e sidebar podem encobrir conteúdo/foco] → Espaço reservado, scroll padding e testes com acordeões longos, footer e viewport pequeno.
- [PNGs e conjunto de fontes são grandes] → Não alterar originais; requisitar apenas faces usadas, adiar imagens fora da dobra e reutilizar URLs para aproveitar cache.
- [Versões verificadas por documentação/metadados, ainda não instaladas] → Validar peers, tipos e build durante a aplicação; não confundir compatibilidade declarada com teste executado.
- [Controles visualmente presentes mas indisponíveis podem surpreender] → Comunicar limites acessivelmente e na documentação, sem fabricar destinos ou sucesso remoto.
- [Permissões do Figma indisponíveis] → Usar somente os documentos e capturas já existentes; o aceite não depende de conseguir permissão futura.

## Migration Plan

1. Após autorização explícita para aplicar, criar o workspace e os arquivos de frontend ausentes, preservando os assets e sem tocar no backend, Compose ou specs anteriores.
2. Implementar fundação visual e fixtures, depois Landing, Detalhes e AVA completos, com navegação e estados previstos nas specs.
3. Executar a matriz de validação, corrigir defeitos e atualizar somente documentação/configurações necessárias ao frontend.
4. Distribuir o build como SPA estática quando houver uma hospedagem definida; documentar seu fallback sem introduzir deploy nesta entrega.

Não há migração de banco, transformação de dados ou rollout coordenado com a API. Um eventual rollback deve retirar apenas os arquivos e ajustes introduzidos pela implementação do frontend, preservando os assets originais e qualquer trabalho do usuário. A conclusão destes artefatos não autoriza iniciar a implementação: a fase de aplicação depende de uma nova solicitação do usuário.
