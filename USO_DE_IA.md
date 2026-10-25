# Registro de uso de Inteligência Artificial

Este registro descreve como ferramentas de IA e automação auxiliaram o desenvolvimento do catálogo. Os prompts abaixo são resumos fiéis da abordagem informada pelo autor, não transcrições literais: o histórico exato dos prompts e as versões/modelos utilizados não foram registrados durante o trabalho e, portanto, não são presumidos aqui.

## Ferramentas utilizadas

- **ChatGPT:** apoio à interpretação das regras de negócio, à modelagem inicial do banco/API e à elaboração de especificações.
- **OpenSpec, da Fission AI:** fluxo de desenvolvimento orientado a especificações (SDD), incluindo a organização das mudanças e o scaffold inicial da API .NET.
- **Playwright, operado por um agente de navegação:** automação do navegador para percorrer o protótipo, observar as telas e registrar conteúdo e características visuais. O modelo do agente não foi anotado; Playwright é a ferramenta de navegação, não o modelo de IA.

## Objetivos do uso

1. Interpretar o documento da atividade e transformar as regras de negócio em uma modelagem inicial de dados e API.
2. Organizar a implementação em especificações verificáveis antes de solicitar a construção do scaffold da API e, posteriormente, do frontend.
3. Documentar o protótipo navegável quando não foi possível obter acesso ao Figma pelo MCP: percorrer as telas como usuário, observar cores, tipografia, bordas e outros elementos visuais, e registrar o conteúdo para apoiar o seed fictício.
4. Comparar as telas com os endpoints disponíveis, identificar lacunas de integração e especificar a correção.

As decisões finais sobre escopo, interpretação do protótipo e aceitação da implementação foram revisadas pelo autor. O projeto não integra com Moodle, conforme permitido pelas instruções da atividade.

## Prompts relevantes e resultados

Os enunciados abaixo resumem os pedidos realizados; não devem ser lidos como citações literais.

| Prompt ou pedido (resumo) | O que foi aproveitado | O que foi descartado ou não ficou registrado |
| --- | --- | --- |
| Interpretar o documento de regras de negócio e propor uma modelagem inicial do banco e da API. | A modelagem inicial serviu de ponto de partida para a especificação do backend. | Não foi registrado um item específico da proposta que tenha sido descartado; a modelagem foi tratada como inicial e validada contra as regras, não como decisão automática. |
| Iniciar o projeto a partir de um diretório vazio, usando OpenSpec/SDD para especificar o scaffold da API em .NET. | A abordagem de especificar o trabalho e organizar a mudança antes da implementação, incluindo o scaffold inicial. | Não há registro de sugestões individuais rejeitadas nessa etapa. |
| Percorrer o protótipo com Playwright como um usuário; não produzir somente capturas de tela; coletar elementos visuais e documentar o conteúdo das páginas para apoiar o seed. | A navegação, o inventário das telas e a documentação do conteúdo e das evidências visuais. | Capturas isoladas não foram consideradas suficientes como método de levantamento. Medidas e estilos que não eram diretamente legíveis foram documentados como estimativas, e não como tokens exatos do Figma. |
| Com base nos assets fornecidos e no levantamento do protótipo, ajudar a escrever uma especificação para o frontend; depois, solicitar a implementação dessa especificação ao agente via OpenSpec. | A especificação foi usada para orientar a implementação do frontend e o reaproveitamento dos assets fornecidos. | Não foi registrada uma lista de propostas visuais individuais rejeitadas. |
| Comparar as telas com os endpoints existentes, apontar lacunas e escrever uma especificação para integrar o frontend à API. | O levantamento de lacunas e a especificação de integração, usados para orientar a implementação e as verificações. | Não foram preservados os prompts literais nem uma lista separada de sugestões descartadas. |

## Como o conteúdo foi validado

- As regras e o escopo foram conferidos com o documento `docs/instrucoes.pdf` e com os requisitos do projeto.
- A API foi exercitada manualmente pelo Swagger, verificando cada endpoint e confrontando as respostas com as regras de negócio.
- O protótipo foi percorrido com navegação real no navegador; conteúdo e observações visuais foram documentados em `docs/figma-reverse-engineering/`.
- Um scanner das telas e dos endpoints foi seguido por uma rodada de testes manuais de integração entre frontend e API para verificar o atendimento aos requisitos.
- Cores amostradas e propriedades visuais estimadas foram identificadas como observações do protótipo, não como valores extraídos dos estilos internos do arquivo Figma. As limitações da inspeção estão registradas na documentação de engenharia reversa.

## Exemplo de inadequação identificada e correção

Na verificação de integração, identificou-se uma lacuna entre o estado inicial do frontend e os requisitos: as telas ainda dependiam de dados locais desconectados da API, e o fluxo de navegação completa estava limitado ao curso de Matemática. Essa inadequação foi identificada pela comparação das telas com os endpoints e os fluxos exigidos; não é atribuída aqui a uma resposta isolada de um modelo.

A correção foi especificada e implementada para consumir a listagem `GET /api/courses` e os detalhes `GET /api/courses/{id}`, relacionar as rotas ao ID de cada curso e cobrir os seis cursos fictícios. A especificação também orientou os estados de carregamento e falha e a validação manual da integração.

## Ferramentas utilizadas e links

- [ChatGPT — OpenAI](https://chatgpt.com/) — análise das regras de negócio e apoio às especificações e à modelagem inicial.
- [OpenSpec — repositório GitHub da Fission AI](https://github.com/Fission-AI/openspec) — fluxo de trabalho baseado em especificações.
- [Playwright — repositório GitHub](https://github.com/microsoft/playwright) — automação de navegador usada pelo agente para navegar e inspecionar o protótipo.
