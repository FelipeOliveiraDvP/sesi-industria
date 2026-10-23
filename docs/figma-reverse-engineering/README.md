# Engenharia reversa visual — protótipo SESI Indústria

## Objetivo

Registrar, com base apenas no que é efetivamente exibido no protótipo navegável, as páginas, os estados, o conteúdo textual, os padrões visuais e as evidências de tela do protótipo SESI Indústria. Este material não implementa a aplicação nem presume acesso de edição ao arquivo Figma.

## Origem e inspeção

- **URL de origem:** [protótipo SESI Indústria](https://www.figma.com/proto/YB0o91nnOEB5bXXCtmy5s6/-TEST-DEV--SESI-Ind%C3%BAstria?node-id=6017-342&p=f&viewport=-1570%2C411%2C0.09&t=tvoKbn8KIvr7vLak-1&scaling=contain&content-scaling=fixed&starting-point-node-id=6017%3A342&page-id=3%3A2)
- **Data da inspeção:** 2026-10-23
- **Ferramenta de navegação:** MCP do Playwright.
- **Viewport do navegador:** 1908 × 844 px CSS, DPR 1.
- **Viewport do protótipo:** elemento canvas medido em `x=48, y=60, largura=1812, altura=724 px`; o conteúdo do protótipo é desenhado dentro dele.
- **Escala:** mantida a configuração original da URL (`scaling=contain`, `content-scaling=fixed`); não foi ajustado o zoom.
- **Páginas identificadas:** 3. **Páginas documentadas:** 3.

## Índice

- [Inventário de páginas](page-inventory.md)
- [Design system inferido](design-system.md)
- [001 — Landing page](pages/001-landing-page.md)
- [002 — Detalhes curso](pages/002-detalhes-curso.md)
- [003 — AVA](pages/003-ava.md)

## Evidências visuais

As capturas foram salvas em PNG, em escala CSS, agrupadas por página:

- [Landing page — assets](assets/001-inicio/)
- [Detalhes curso — assets](assets/002-detalhes-curso/)
- [AVA — assets](assets/003-ava/)

As páginas individuais contêm links diretos para as capturas usadas como evidência. Os nomes iniciados por `viewport` registram a primeira tela; `section-*` registra rolagem; `state-*` ou `*-carousel-*` registra variação de interação.

## Limitações conhecidas

1. O MCP oficial do Figma recusou a abertura de sessão de leitura com a mensagem de que não há acesso de edição ao arquivo. Nenhuma edição foi tentada. A inspeção do protótipo foi feita com o Playwright, conforme a URL pública fornecida.
2. A renderização é uma combinação de DOM externo (controles do visualizador/wrappers), um `<canvas>` visível de 1812 × 724 px e um `iframe` auxiliar de 0 × 0 px, sem conteúdo interno acessível. A interface do protótipo em si é desenhada no canvas. O snapshot de acessibilidade e o DOM externo não expõem seus textos, imagens, bounding boxes ou estilos; o conteúdo interno foi transcrito visualmente das screenshots.
3. O canvas traz a mensagem do visualizador: `Screenreader support is currently disabled. To enable it, use the Accessibility settings from the Options menu.` Portanto, não foi possível obter uma árvore acessível dos elementos internos.
4. Cores listadas como valores HEX foram amostradas do raster PNG quando havia uma área plana; elas **não são tokens CSS extraídos**. Posições, dimensões internas, tipografia e propriedades de imagem são estimativas visuais. O `design-system.md` separa essas categorias.
5. O tamanho do frame e o conteúdo total não são expostos pelo DOM. As dimensões totais foram estimadas a partir da escala visual e da rolagem interna. O conteúdo foi percorrido até o fim; as screenshots de confirmação no rodapé do Landing e de Detalhes são idênticas às capturas anteriores, e o final do AVA foi confirmado por captura idêntica após rolagem adicional.
6. Só foi inspecionada a apresentação desktop da URL fornecida; responsividade em outros viewports não foi determinada.
7. Em alguns controles visuais não houve transição perceptível no protótipo. Isso está anotado como comportamento não confirmado, sem inferir destino ou ação.
