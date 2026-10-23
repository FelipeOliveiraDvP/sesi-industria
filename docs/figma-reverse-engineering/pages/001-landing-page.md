# Landing page

## 1. Identificação

- **ID da página:** 001
- **Identificador do frame:** `page-id=3:2`, `node-id=6017:342` — fluxo `01. Landing page`.
- **URL:** [abrir Landing page no protótipo](https://www.figma.com/proto/YB0o91nnOEB5bXXCtmy5s6/-TEST-DEV--SESI-Ind%C3%BAstria?node-id=6017-342&p=f&viewport=-1570%2C411%2C0.09&t=tvoKbn8KIvr7vLak-1&scaling=contain&content-scaling=fixed&starting-point-node-id=6017%3A342&page-id=3%3A2)
- **Página de origem:** ponto inicial fornecido na tarefa.
- **Forma de navegação até esta página:** abertura direta pela URL; também pode ser escolhida em `Flows` ou acessada pelo item `Início` a partir de Detalhes curso.
- **Estados inspecionados:** viewport inicial; rolagem por cursos, métricas, depoimentos, CTA e rodapé; carrossel de cursos na posição inicial e avançada; carrossel de depoimentos avançado; cliques em navegação e CTAs.
- **Dimensões do viewport do navegador:** 1908 × 844 px CSS, DPR 1 (**confirmado pelo DOM**).
- **Canvas do visualizador:** `x=48, y=60, 1812 × 724 px` (**confirmado pelo DOM**).
- **Frame visível:** aproximadamente `x=309, y=60, 1288 × 724 px` no screenshot (**estimado pelo raster**); proporção próxima de 16:9, escala perto de 0,9×.
- **Dimensões totais estimadas do conteúdo:** cerca de 1440 × 4.000 px no frame (**estimada** a partir da rolagem interna; altura total não exposta pelo DOM).

## 2. Objetivo e descrição geral

Página inicial de uma plataforma de estudos voltada à indústria. O conteúdo observado apresenta a proposta de capacitação, cursos em destaque, métricas de impacto, depoimentos, uma chamada de matrícula e informações institucionais no rodapé.

## 3. Estrutura visual

Leitura de cima para baixo, com posições em **pixels do viewport da screenshot** (origem no canto superior esquerdo da captura; dimensões internas estimadas):

1. **Header** — faixa vermelha no topo do frame, aproximadamente `x=309, y=60, 1288 × 72 px`. Logotipo alinhado à esquerda e navegação alinhada à direita. O header permaneceu visível nas rolagens observadas.
2. **Hero** — aproximadamente `x=309, y=132, 1288 × 495 px`, fotografia de ambiente industrial com forte sobreposição vermelha. Texto e CTA à esquerda; pessoa e máquinas ocupam principalmente a metade direita.
3. **Principais cursos** — superfície branca, heading e trilho horizontal de cards. No estado de cursos capturado, título em torno de `x=411, y=130`; cards em torno de `x=411, y=196`, com cerca de `259 × 373 px` cada no screenshot. Há quatro cards quase completos e parte do próximo; o carrossel revela os demais.
4. **Impacto transformador** — superfície branca em duas regiões: texto/descrição à esquerda; quatro métricas empilhadas à direita, com três benefícios ilustrados abaixo. Evidência em `section-03-impacto.png`.
5. **Depoimentos** — seção vermelha de largura total. Eyebrow e heading no alto à esquerda; cards vermelhos mais claros em carrossel. Na captura, cards iniciam perto de `x=411, y=338` e têm cerca de `259 × 373 px` no screenshot.
6. **CTA de matrícula** — seção vermelha, texto e botão à esquerda, imagem de vídeo à direita. A seção aparece antes do rodapé.
7. **Footer** — superfície vinho quase preta, quatro colunas (marca/contato, navegação, cursos populares, suporte) e linha inferior de direitos/ícones sociais.

As posições acima são **estimadas visualmente** e referem-se a screenshots específicas; não são coordenadas de nodes obtidas do Figma.

## 4. Conteúdo textual

Os textos abaixo são **Confirmados por transcrição visual** das screenshots. Não foi possível extrair strings internas do canvas via DOM.

### Header e hero

- Logotipo: `SESI + INDÚSTRIA`
- Navegação: `Início` · `Cursos` · `Sobre` · `Área do aluno`
- Título: `Impulsione sua carreira na indústria`
- Texto: `Amplie seus conhecimentos com excelência na melhor plataforma de estudos da indústria brasileira`
- Botão: `Conhecer cursos →`

### Principais cursos

1. **Badge:** `MATEMÁTICA`  
   **Título:** `Matemática para a Precisão: Medidas e Escalas na Metalurgia`  
   **Descrição:** `Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.`
2. **Badge:** `MATEMÁTICA`  
   **Título:** `Geometria e Modelagem no Design Têxtil`  
   **Descrição:** `Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.`
3. **Badge:** `MATEMÁTICA`  
   **Título:** `Estatística e Controle de Qualidade na Produção`  
   **Descrição:** `Leitura e interpretação de dados de controle de qualidade e produtividade.`
4. **Badge:** `CIÊNCIAS DA NATUREZA`  
   **Título:** `Física do Movimento e da Energia na Metalurgia`  
   **Descrição:** `Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.`
5. **Badge:** `CIÊNCIAS DA NATUREZA`  
   **Título:** `Química e Sustentabilidade`  
   **Descrição:** `Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.`
6. **Badge:** `CIÊNCIAS DA NATUREZA`  
   **Título:** `Ciência dos Materiais na Metalurgia`  
   **Descrição:** `Conceitos básicos de estrutura, resistência e propriedades dos metais.`

Card/ação final do carrossel: `Todos os cursos →`. Botão sob os cards: `Ver todos os cursos`.

### Impacto transformador

- Título: `Impacto transformador`
- Texto: `Resultados que demonstram o sucesso da nossa metodologia de capacitação contínua para a indústria`
- Métrica: `25.000+` — `Profissionais Capacitados`
- Métrica: `500+` — `Empresas parceiras`
- Métrica: `4.8/5` — `Avaliação média`
- Métrica: `2.5M+` — `Horas de capacitação`
- Benefício: `Certificação SESI` — `Cursos com a melhor certificação do mercado industrial reconhecidos pelo SESI.`
- Benefício: `Professores especialistas` — `Contamos com professores reconhecidos por sua excelência no mercado.`
- Benefício: `Suporte contínuo` — `Você não está sozinho! Acompanhamento online durante todo o curso.`

### Depoimentos

- Eyebrow: `DEPOIMENTOS`
- Título: `Quem fez recomenda`
- `“A experiência do curso foi incrível! As aulas foram muito bem estruturadas e o cronograma foi cuidadosamente planejado, facilitando muito o aprendizado.”` — **Lucas Pereira**, `Analista na Empresa X`, `★★★★★`
- `“Concluir o curso foi uma jornada enriquecedora. As aulas eram dinâmicas e o horário foi organizado para maximizar nosso tempo de estudo.”` — **Mariana Costa**, `Gestora na Empresa X`, `★★★★★`
- `“O curso superou minhas expectativas! As aulas eram bem elaboradas e o planejamento do horário ajudou a manter o foco e a produtividade.”` — **Thiago Almeida**, `Técnico na Empresa X`, `★★★★★`
- `“Finalizar o curso foi uma experiência motivadora. As aulas foram muito bem preparadas e o cronograma foi ideal para o aprendizado.”` — **Fernanda Silva**, `Analista na Empresa X`, `★★★★★`
- `“A conclusão do curso foi uma etapa muito gratificante. As aulas eram bem organizadas e o horário foi pensado para facilitar nosso aprendizado.”` — **Gabriel Santos**, `Analista na Empresa X`, `★★★★★`

### CTA de matrícula

- Título: `Pronto para transformar sua carreira?`
- Texto: `Junte-se a milhares de profissionais que já estão transformando a indústria através dos nossos cursos. Invista no seu futuro hoje mesmo!`
- Botão: `Matricule-se →`
- Ícone sobre a imagem: play triangular em círculo.

### Rodapé

- Logotipo: `SESI + INDÚSTRIA`
- Contato: `contato@sesiindustria.com.br`
- Endereço: `Av. Paulista, 1313`
- Coluna `NAVEGAÇÃO`: `Início`, `Certificação`, `Cursos`, `Sobre`, `Blog`
- Coluna `CURSOS POPULARES`: `Cultura Organizacional`, `Ferramentas Google para Produtividade`, `Excel Básico ao Avançado`, `Trabalho em Equipe e Colaboração`, `Gestão do Tempo e Produtividade`, `Comunicação Assertiva`, `Inteligência Emocional no Trabalho`
- Coluna `SUPORTE`: `Termos de Uso`, `Política de Privacidade`, `Trabalhe Conosco`, `Fale Conosco`, `Central de Ajuda`
- Linha inferior: `SESI` + `Todos os direitos reservados.`
- Ícones sociais visíveis: LinkedIn, Instagram e Facebook (sem texto adjacente legível).

## 5. Componentes e posicionamento

| Componente | Posição/dimensão aproximada | Estilo e relação espacial | Classificação |
|---|---|---|---|
| Header | frame inteiro; `x≈309, y≈60, w≈1288, h≈72` no screenshot | Fundo vermelho; logo à esquerda; links alinhados horizontalmente à direita; linha inferior clara. | **Estimado** |
| Hero | `x≈309, y≈132, w≈1288, h≈495` | H1 branco em três linhas à esquerda; parágrafo e botão abaixo; fotografia cobre o bloco com overlay vermelho. | **Estimado** |
| Cards de curso | `x≈411, y≈196`; cada um `≈259 × 373` no estado capturado | Badges no topo; títulos/descrições no terço inferior; overlay vermelho; cantos arredondados e sombra leve. | **Estimado** |
| Métricas | coluna direita, screenshot `section-03-impacto.png` | Quatro linhas com número grande à esquerda, rótulo à direita e divisores. | **Estimado** |
| Benefícios | abaixo das métricas, três colunas | Ícones outline vermelhos em quadrados rosa-claro; título bold e texto menor. | **Estimado** |
| Cards de depoimento | screenshot `section-04-depoimentos.png`, desde `x≈411, y≈338` | Cards de fundo vermelho claro sobre vermelho escuro; citação no topo, autor/cargo e estrelas na base. | **Estimado** |
| CTA visual | screenshot `section-05-cta.png`; coluna de texto à esquerda e imagem à direita | Botão branco; imagem de vídeo retangular com play central. | **Estimado** |
| Footer | bloco escuro ao final do frame | Quatro colunas e linha inferior; texto secundário cinza; ícones sociais à direita. | **Estimado** |

## 6. Imagens e elementos gráficos

| Elemento | Posição/dimensão aproximada | Descrição visual e ajuste | Evidência / origem |
|---|---|---|---|
| Hero industrial | ocupa a largura do hero (`≈1288 × 495 px` no screenshot) | Homem sorridente, de óculos e roupa de trabalho, diante de máquinas/linha industrial; recorte amplo, com overlay vermelho. Ajuste aparente `cover`. | `viewport.png`; nome/origem do arquivo **não identificados**. |
| Card 1 | `≈259 × 373 px` | Close de medição/régua sobre peça metálica ou bobina; badge `MATEMÁTICA`; overlay vermelho em gradiente. | `section-02-principais-cursos.png`; origem **não identificada**. |
| Card 2 | `≈259 × 373 px` | Materiais/têxteis e padrões geométricos; badge `MATEMÁTICA`. | `section-02-principais-cursos.png`; origem **não identificada**. |
| Card 3 | `≈259 × 373 px` | Tela de dados/controle em ambiente de produção; badge `MATEMÁTICA`. | `section-02-principais-cursos.png`; origem **não identificada**. |
| Card 4 | `≈259 × 373 px` | Trabalho de soldagem/metais com faíscas; badge `CIÊNCIAS DA NATUREZA`. | `courses-carousel-end.png`; origem **não identificada**. |
| Card 5 | `≈259 × 373 px` | Recipientes/produto em contexto de química/tinturaria; badge `CIÊNCIAS DA NATUREZA`. | `courses-carousel-end.png`; origem **não identificada**. |
| Card 6 | `≈259 × 373 px` | Superfície/peça metálica com perfil cilíndrico; badge `CIÊNCIAS DA NATUREZA`. | `courses-carousel-end.png`; origem **não identificada**. |
| Ícones de benefício | pequenos, abaixo das métricas | Medalha, capelo e headset em traço vermelho; finalidade sugerida pelo texto adjacente. | `section-03-impacto.png`; arquivos de ícone **não identificados**. |
| Estrelas | cinco por depoimento | Estrelas sólidas, brancas na Landing. | `section-04-depoimentos.png`. |
| Thumbnail de vídeo | aproximadamente `703 × 397 px` no screenshot | Dois profissionais em ambiente industrial; um usa capacete e demonstra equipamento; overlay escuro e botão de play central. Ajuste aparente `cover`. | `section-05-cta.png`; fonte/nome **não identificados**. |
| Logotipo/ícones sociais | header/footer | Logotipo SESI + INDÚSTRIA; ícones LinkedIn, Instagram e Facebook. | `viewport.png` e `section-06-rodape.png`; vetores/fontes **não identificados**. |

## 7. Estilos observados

- Fundo principal branco (`#FFFFFF`, amostrado do PNG); textos escuros próximos de `#0C0C0D` (**raster**, não CSS).
- Header/hero usam vermelho sobre fotografia; o vermelho do hero varia devido ao overlay e à imagem.
- Cards de curso usam gradiente vermelho; pixels na região inferior foram amostrados próximos de `#990212`.
- Botões do hero: fundo branco, texto escuro, seta à direita, cantos arredondados.
- Cards: raio estimado em 8–10 px e sombra leve; sem valor CSS disponível.
- Cabeçalhos grandes em bold; corpo em sans-serif regular. Família tipográfica **não identificada**.
- Footer: fundo amostrado `#1D0300`, texto de apoio em cinza amostrado próximo de `#9C908F`.

## 8. Interações e navegação

| Controle | Ação/resultado observado | Classificação |
|---|---|---|
| `Cursos` no header | Rola para a seção `Principais cursos`; o `node-id` permanece `6017:342`. | **Confirmada** |
| `Conhecer cursos →` | Também leva/rola para a seção `Principais cursos`. | **Confirmada** |
| Primeiro card (`Matemática para a Precisão...`) | Abre `Detalhes curso`, nó `6017:707`. | **Confirmada** |
| `Matricule-se →` no CTA inferior | Abre `Detalhes curso`, nó `6017:707`. | **Confirmada** |
| Cards 2 e 3 testados | Não houve alteração visível de nó/URL. | **Confirmada para os cliques testados** |
| `Ver todos os cursos` | Não houve mudança visível após o clique observado. | **Confirmada para a sessão** |
| Card final `Todos os cursos →` | Não houve mudança visível após o clique observado. | **Confirmada para a sessão** |
| Seta direita do carrossel de cursos | Avança o trilho; aparecem os cursos seguintes e o card `Todos os cursos →`. | **Confirmada** |
| Seta esquerda do carrossel de cursos | Após avançar, retorna ao grupo inicial quando acionada no estado testado. | **Confirmada** |
| Seta direita do carrossel de depoimentos | Avança o trilho de depoimentos. | **Confirmada** |
| `Sobre` e `Área do aluno` | Não houve mudança visível de nó/tela nos cliques observados. | **Confirmada para a sessão; destino não identificado** |
| `Início` | A partir de Detalhes curso retorna à Landing. Na Landing já no início, nenhum novo estado foi observado. | **Confirmada** |
| Play sobre a imagem da CTA | O clique não iniciou reprodução nem mudou o nó/tela. | **Confirmada para o estado observado** |
| Links representativos do footer | `Início`, `Certificação`, `Cursos`, `Cultura Organizacional` e `Termos de Uso` não mudaram URL ou tela nos cliques testados. Os demais links não foram acionados individualmente. | **Confirmada para os cliques testados** |

## 9. Estados e variações

- Posição inicial da Landing com hero e início da seção de cursos visíveis.
- Posição da seção de cursos com cards 1–5 parcialmente visíveis.
- Carrossel de cursos avançado, com cards 3–6 e card `Todos os cursos →` visíveis.
- Depoimentos em posição inicial e avançada; cinco cards identificados.
- Rolagem até métricas, CTA e rodapé; uma rolagem adicional no final não alterou a screenshot (PNG idêntico à captura anterior).
- Não foram observados modal, dropdown, validação de formulário ou estado de erro/sucesso.
- A cor cinza clara atrás da seta de carrossel corresponde ao hover/click apontado na captura, não a uma seleção persistente confirmada.

## 10. Evidências

- [Viewport inicial](../assets/001-inicio/viewport.png)
- [Seção Principais cursos](../assets/001-inicio/section-02-principais-cursos.png)
- [Carrossel de cursos — avançado](../assets/001-inicio/courses-carousel-end.png)
- [Carrossel de cursos — grupo seguinte](../assets/001-inicio/courses-carousel-next.png)
- [Métricas e benefícios](../assets/001-inicio/section-03-impacto.png)
- [Depoimentos](../assets/001-inicio/section-04-depoimentos.png)
- [Depoimentos — carrossel avançado](../assets/001-inicio/testimonials-carousel-next.png)
- [CTA de matrícula](../assets/001-inicio/section-05-cta.png)
- [Footer](../assets/001-inicio/section-06-rodape.png)
- [Confirmação de fim da rolagem](../assets/001-inicio/section-06-bottom-confirmation.png)
- [CTA `Conhecer cursos`](../assets/001-inicio/state-conhecer-cursos.png)
- [Clique no link de footer `Início` (sem mudança de estado)](../assets/001-inicio/footer-link-inicio-test.png)
- [Destino do CTA inferior `Matricule-se`](../assets/002-detalhes-curso/state-home-cta-to-details.png)
- [Lista de flows do Figma](../assets/001-inicio/flows-panel.png)

## 11. Incertezas e lacunas

- Não foi possível determinar a fonte interna, os tokens CSS, o nome/URL dos arquivos de imagem nem as dimensões originais dos nodes.
- O conteúdo gráfico interno não aparece na árvore de acessibilidade; textos foram conferidos visualmente nas screenshots.
- `Sobre`, `Área do aluno`, `Ver todos os cursos` e o card final não mostraram destinos na sessão inspecionada; não se infere que sejam links funcionais.
- O estado responsivo em telas menores não foi inspecionado.
