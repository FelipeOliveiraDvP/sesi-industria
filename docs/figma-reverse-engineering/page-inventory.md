# Inventário de páginas

As três entradas abaixo foram observadas na lista **Flows** do visualizador Figma: `01. Landing page`, `02. Detalhes curso` e `03. AVA`. A lista mostrava `No description` para o fluxo selecionado.

| ID | Página | Identificador/URL | Estados relevantes | Status |
|---|---|---|---|---|
| 001 | [Landing page](pages/001-landing-page.md) | `page-id=3:2`, `node-id=6017:342`; início da URL fornecida | Topo; rolagem até o fim; carrosséis; CTA `Matricule-se` para Detalhes curso; navegação por `Cursos`/`Conhecer cursos`; tentativas em links de footer, `Sobre` e `Área do aluno` | Documentada integralmente com evidências visuais; conteúdo interno sem DOM acessível |
| 002 | [Detalhes curso](pages/002-detalhes-curso.md) | `page-id=3:2`, `node-id=6017:707`; fluxo `02. Detalhes curso` | Hero; conteúdo abaixo da dobra; `Módulos` aberto; `Aulas online e avaliações` aberto; `Dúvidas frequentes` aberto; barra fixa `Matricule-se`; retorno do AVA | Documentada; rodapé inferior parcialmente encoberto pela barra fixa quando ela está presente |
| 003 | [AVA](pages/003-ava.md) | `page-id=3:2`, `node-id=6017:1042`; fluxo `03. AVA` | Progresso 10%; conteúdo `Boas práticas`; rolagem da coluna principal até o fim; retorno por seta à página de detalhes | Documentada integralmente quanto ao conteúdo exibido; itens da navegação lateral não mudaram de estado nos cliques testados |

## Descoberta e ciclos

- **Landing → Detalhes curso:** clique no primeiro card `Matemática para a Precisão: Medidas e Escalas na Metalurgia` ou no CTA `Matricule-se` da seção final alterou o nó para `6017:707`. Os cards 2 e 3 testados não alteraram a tela.
- **Detalhes curso → AVA:** clique no botão `Matricule-se` do hero ou na barra fixa alterou o nó para `6017:1042`.
- **AVA → Detalhes curso:** a seta de voltar na barra lateral retornou ao nó `6017:707`.
- Os controles `Flows` permitem selecionar diretamente os três fluxos e fechar o painel. O controle `Next frame` avançou da Landing para Detalhes quando acionado; `Previous frame` estava marcado como indisponível no estado testado. `Restart` reiniciou para o ponto inicial atualmente selecionado.
- Os cliques em `Sobre`, `Área do aluno`, `Ver todos os cursos`, no card final `Todos os cursos`, em links representativos do footer e em `Mandar uma mensagem` não produziram mudança de URL ou tela na sessão observada. Os ícones de play testados nas imagens da Landing e de Detalhes também não iniciaram reprodução visível.
- Não foram identificados outros fluxos/páginas na lista do protótipo nem destinos novos nas interações testadas. Os itens internos da sidebar do AVA são registrados como controles visuais/estados, não como páginas adicionais, pois os cliques testados não os alteraram.
