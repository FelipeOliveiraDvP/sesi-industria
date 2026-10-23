# Design system inferido

## Como ler os valores

- **Confirmado — raster:** valor RGB/HEX obtido por amostragem de pixels de uma área plana das screenshots PNG. Confirma a cor exibida naquela captura, não um token CSS do arquivo Figma.
- **Estimado — visual:** posição, dimensão, escala, tipografia, raio, sombra ou função inferida olhando screenshots do canvas.
- **Não identificado:** não foi possível determinar sem leitura do arquivo Figma ou do DOM interno.

O conteúdo do protótipo é desenhado em canvas e não expõe os estilos internos via DOM/CSS. O MCP do Figma não concedeu sessão de leitura por falta de acesso de edição; por isso, os valores abaixo são evidência visual, não uma extração de tokens do arquivo.

## Cores observadas

| Papel visual | HEX / RGB amostrado | Evidência e classificação |
|---|---|---|
| Superfície principal | `#FFFFFF` / `255, 255, 255` | **Confirmado — raster** em áreas grandes da Landing, Detalhes e AVA. |
| Texto principal / botão escuro | `#0C0C0D` / `12, 12, 13` | **Confirmado — raster** em texto escuro, botão de matrícula e sidebar do AVA. O papel de token é **estimado**. |
| Vermelho profundo | `#890000` / `137, 0, 0` | **Confirmado — raster** em fundo de depoimentos e CTA de contato. O papel de cor institucional primária é **estimado**. |
| Superfície de card vermelho | `#C3222C` / `195, 34, 44` | **Confirmado — raster** nos cards de depoimentos da Landing. |
| Base do gradiente dos cards de curso | `#990212` / `153, 2, 18` | **Confirmado — raster** em região inferior dos cards de curso; o restante do gradiente varia com a imagem. |
| Superfície quente clara | `#FCD19C` / `252, 209, 156` | **Confirmado — raster** no fundo de depoimentos de Detalhes e em badge `MATEMÁTICA`. |
| Badge de Ciências da Natureza | `#E4CCFF` / `228, 204, 255` | **Confirmado — raster** no badge dos cards dessa categoria. |
| Rodapé escuro | `#1D0300` / `29, 3, 0` | **Confirmado — raster** na área de rodapé. |
| Texto secundário do rodapé | `#9C908F` / `156, 144, 143` | **Confirmado — raster** em texto visível no rodapé; o uso como token é **estimado**. |
| Cinza intermediário | `#858586` / `133, 133, 134` | Amostra raster recorrente em texto de apoio/antialiasing; valor de token e papel exato **estimados**. |
| Divisores/bordas claras | próximo de `#E5E5E5` | **Estimado — visual** nas linhas de navegação, acordeões e header. Não foi extraído do CSS. |
| Fundo externo do visualizador | `#000000` / `0, 0, 0` | **Confirmado — raster**, mas pertence à moldura do visualizador Figma, não à interface SESI. |

O hero da Landing e imagens dos cards têm sobreposição/gradiente vermelho sobre fotografia; a cor final depende do pixel da foto e não foi tratada como um único token sólido.

## Tipografia

| Uso | Observação | Classificação |
|---|---|---|
| Família tipográfica interna | Sans-serif de aparência neutra, com formas simples e boa legibilidade. O nome da fonte não pode ser confirmado a partir do canvas. | **Não identificada** |
| Chrome externo do Figma | O DOM externo retorna `Inter, sans-serif` em alguns controles do visualizador, em torno de 14 px. Não é evidência da fonte da interface SESI. | **Confirmado para o visualizador**, não aplicável ao produto |
| H1 da Landing | Muito grande, bold, em três linhas; aproximadamente 68–72 px no frame estimado. | **Estimado** |
| H1 de Detalhes curso | Bold, aproximadamente 38–42 px no frame estimado. | **Estimado** |
| H2 de seção | Bold, aproximadamente 36–42 px. | **Estimado** |
| Título dos cards de curso | Bold/semibold, aproximadamente 22–26 px. | **Estimado** |
| Corpo de texto | Regular, aproximadamente 16–18 px; linhas relativamente abertas. | **Estimado** |
| Navegação e botões | Regular/medium, aproximadamente 14–16 px. | **Estimado** |
| Labels e categorias | Aproximadamente 10–12 px, frequentemente em caixa alta. | **Estimado** |
| AVA | Cabeçalhos em bold e corpo regular; corpo aproximadamente 17–19 px na escala do frame. | **Estimado** |

Altura de linha, tracking e nomes exatos de pesos não puderam ser lidos. Visualmente, títulos usam peso forte e corpo/descrições usam peso regular; alinhamento predominantemente à esquerda, exceto CTAs centrais.

## Layout, espaçamento e superfícies

- **Frame desktop:** proporção visual próxima de 16:9. Na captura CSS de 1908 × 844, Landing e Detalhes aparecem aproximadamente em `x=309, y=60, 1288 × 724 px`; o AVA aparece aproximadamente em `x=300, y=60, 1306 × 724 px`. São limites estimados pelo raster, não bounding boxes internas do Figma.
- **Canvas do visualizador:** bounding box confirmado pelo DOM em `x=48, y=60, 1812 × 724 px`; escala de apresentação perto de 0,9× para os frames mostrados.
- **Landing:** navegação horizontal no topo; conteúdo central com margens largas; hero de largura total; cards em trilho horizontal; bloco de métricas e benefícios em colunas; depoimentos em carrossel; CTA e rodapé em blocos de largura total. O header permaneceu visível durante a rolagem observada.
- **Detalhes curso:** hero em duas colunas (texto à esquerda, imagem 16:9 à direita); benefícios em quatro colunas; descrição e acordeões em duas colunas; depoimentos e rodapé no fluxo vertical. O header não permaneceu visível depois da rolagem inicial.
- **AVA:** sidebar escura fixa à esquerda e coluna de conteúdo branca à direita; a coluna principal rola sem mover a sidebar.
- **Container central:** largura visual estimada em torno de 1.200–1.220 px no frame de produto, com margens internas amplas.
- **Espaçamentos:** ritmo amplo entre seções, cards separados por gutters regulares, ícone/label/descrição agrupados verticalmente. Valores exatos em tokens não identificados.
- **Responsividade:** não avaliada; somente a apresentação desktop da URL foi inspecionada.

## Componentes recorrentes

| Componente | Padrão observado | Exemplos |
|---|---|---|
| Marca | Logotipo tipográfico `SESI + INDÚSTRIA`, vermelho sobre branco; branco em fundos vermelhos/escuros. | Header e footer. |
| Botão claro primário | Fundo branco, texto quase preto, seta para a direita, cantos arredondados estimados em 7–9 px. | `Conhecer cursos`, `Matricule-se`, `Mandar uma mensagem`. |
| Botão escuro | Fundo `#0C0C0D` amostrado, texto branco, cantos arredondados. | `Matricule-se` em Detalhes curso e barra fixa. |
| Botão secundário/contorno | Fundo branco, contorno cinza fino e texto escuro. | `Ver todos os cursos`, `Área do aluno`. |
| Card de curso | Fotografia como fundo/cover, overlay vermelho, badge colorido, título e descrição brancos, cantos arredondados e sombra discreta. | Carrossel `Principais cursos`. |
| Card de depoimento | Texto de citação, autor, cargo e cinco estrelas; fundo vermelho claro sobre vermelho escuro na Landing; branco sobre laranja claro em Detalhes. | `Quem fez recomenda`. |
| Métrica | Número grande à esquerda e rótulo alinhado à direita, separados por linhas finas. | `25.000+`, `500+`, `4.8/5`, `2.5M+`. |
| Ícone de benefício | Ícone outline dentro de quadrado colorido de cantos arredondados. | Certificação, professores, suporte e flexibilidade. |
| Carrossel | Trilho horizontal com setas esquerda/direita sem texto; o botão sob o ponteiro recebeu uma superfície cinza clara. | Cursos e depoimentos. |
| Acordeão | Linhas divisórias horizontais, título em negrito e sinal `+`/`−`; conteúdo textual expande inline. | `Módulos`, `Aulas online e avaliações`, `Dúvidas frequentes`. |
| Barra fixa de matrícula | Faixa preta elevada com nome do curso à esquerda e botão branco à direita; surge após a rolagem em Detalhes. | `Matemática para a Precisão: Medidas e Escalas na Metalurgia`. |
| Sidebar do AVA | Cabeçalho do curso, progresso, busca visual e seções/itens numerados. | Fluxo `03. AVA`. |
| Rodapé | Fundo quase preto-vinho, quatro colunas de marca/contato, navegação, cursos populares e suporte; ícones sociais no final. | Landing; também aparece em Detalhes. |

Raios, espessuras e sombras não foram medidos no CSS; os valores são estimativas pela imagem. Não foram encontrados estados de validação, erro ou sucesso do produto durante a navegação. O clique no play de vídeo de Detalhes não produziu mudança visual observável.
