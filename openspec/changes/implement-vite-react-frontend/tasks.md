# Tasks

Todas as tarefas abaixo pertencem à futura fase de aplicação. Nenhuma está concluída por este planejamento. Os contratos estão em `specs/`; as decisões, versões e matriz de validação estão em `design.md`.

## 1. Runtime e workspace independentes

- [x] 1.1 Registrar a lista e os hashes dos 65 assets atuais antes de criar arquivos de implementação, conferindo também o trabalho existente no Git; verificar que o inventário cobre PNGs, SVGs, TTFs e os dois TXT sem incluir arquivos Zone.Identifier.
- [x] 1.2 Criar o workspace pnpm mínimo na raiz e o pacote privado `@sesi/frontend`, com `packageManager` fixado em `11.9.0`, Node compatível e dependências da matriz do design; executar `pnpm install` e verificar peers compatíveis e um único `pnpm-lock.yaml` na raiz.
- [x] 1.3 Criar configurações Vite/plugin React, TypeScript estrito de aplicação e tooling e PostCSS Mantine; verificar arquivos entregues, cobertura de src/configuração pelos tsconfigs e ausência de proxy, endpoint de API ou biblioteca UI concorrente.
- [x] 1.4 Criar entrada React, providers e roteamento centralizados, com as três composições de página inicializáveis e fallback de não encontrado; verificar por navegador a abertura direta das três URLs aprovadas e o retorno à Landing a partir da 404, sem tratar o scaffold como conclusão das páginas.
- [x] 1.5 Configurar `dev`, `typecheck`, `build` e `preview`, com build precedido por tipos; executar `pnpm --filter @sesi/frontend typecheck` e `pnpm --filter @sesi/frontend build`, verificando código zero e ausência de emissão de JavaScript pelo typecheck.
- [x] 1.6 Criar as instruções iniciais em `apps/frontend/README.md`, acrescentar execução independente no README da raiz e ignorar apenas artefatos gerados aplicáveis; verificar os comandos documentados de dev/preview e o diff, preservando os passos .NET, Compose e arquivos do backend.

## 2. Identidade visual, assets e fixtures tipadas

- [x] 2.1 Centralizar os caminhos dos assets e declarar Open Sans local com `@font-face`, pesos suportados e `font-display: swap`; verificar no navegador fontes e imagens do mesmo origin, nomes com caixa correta e preservação de todos os arquivos originais/licença.
- [x] 2.2 Criar o tema Mantine único, estilos globais e bases CSS Modules com paleta, títulos, superfícies, espaçamento, raios e breakpoints do design; verificar estilos computados de uma tela inicializável, esquema claro sem persistência e ordem correta dos estilos Mantine/overrides.
- [x] 2.3 Criar fixtures tipadas dos seis cards na ordem documentada, quatro métricas, benefícios e cinco depoimentos compartilhados; verificar contagens, títulos, descrições, citações e o mapeamento aprovado de recortes locais contra a transcrição da Landing.
- [x] 2.4 Criar fixtures tipadas do curso, metadados, dez módulos com todos os tópicos, conteúdo integral de aulas/avaliações e dez FAQs; verificar contagens, ordem e todos os textos contra a documentação de Detalhes, sem acrescentar regras de negócio ou dados da API.
- [x] 2.5 Criar fixtures do AVA com progresso fixo de 10%, Introdução e três itens, seis módulos genéricos e os sete parágrafos do artigo; verificar que os estados e textos correspondem ao documento do AVA sem tentar derivá-los dos dez módulos de Detalhes.
- [x] 2.6 Executar o typecheck e conferir a fundação no navegador, verificando ausência de `any`, supressões e URLs remotas, carregamento das faces necessárias e layout sem os controles/margens do Figma; corrigir os problemas antes de integrar as páginas completas.
- [x] 2.7 Documentar no README do frontend as fontes locais, inventário/mapeamento de assets e limitações de tokens, fotografias e mobile; verificar que o texto distingue referências confirmadas, estimativas e a reutilização de fotos aprovada pelo usuário.

## 3. Landing completa e componentes compartilhados de marketing

- [x] 3.1 Implementar MarketingLayout, header e footer compartilhados, incluindo variante inicial/rolada da Landing, conteúdo integral do footer e indisponibilidade acessível dos destinos não confirmados; verificar no navegador marca correta, header visível ao rolar e todas as colunas/linha inferior do footer.
- [x] 3.2 Implementar o hero industrial com fotografia local, overlay, texto integral e Conhecer cursos; verificar composição em desktop, navegação para a seção de cursos sem encobrimento pelo header e redução de movimento quando solicitada.
- [x] 3.3 Implementar os seis cards e o card final Todos os cursos com badges, textos completos, gradientes e recortes locais definidos; verificar ordem e integridade visual, primeiro card navegando para Detalhes e ausência de novos destinos nos demais cards.
- [x] 3.4 Implementar o trilho horizontal compartilhado com setas acessíveis e limites recalculados após scroll/resize; verificar avanço até o card final, retorno ao início, operação por teclado e ausência de loop, duplicação ou overflow horizontal da página.
- [x] 3.5 Implementar Impacto transformador e BenefitList reutilizável na variante de três benefícios; verificar os quatro valores estáticos, textos, ícones vermelhos e reorganização das colunas sem perda de conteúdo.
- [x] 3.6 Implementar Testimonials compartilhado na variante vermelha e a CTA final com VideoPreview local; verificar leitura dos cinco depoimentos com avanço/retorno, Matricule-se abrindo Detalhes e play indisponível sem requisição de vídeo ou confirmação de matrícula.
- [x] 3.7 Validar a Landing inteira no navegador em 1440, 1024, 768 e 375 px, incluindo navegação móvel Mantine, teclado, foco, carrosséis, âncora e ambas as entradas em Detalhes; comparar seções às evidências locais, executar typecheck/build e corrigir erros de console/recursos desta página.
- [x] 3.8 Atualizar o README do frontend com a rota `/`, interações reais e ações ilustrativas da Landing; verificar que não promete página de catálogo, login, reprodução ou destinos de footer não implementados.

## 4. Detalhes do curso e acesso demonstrativo ao AVA

- [x] 4.1 Implementar a composição de Detalhes com header claro não fixo, hero em duas colunas, metadados estáticos, VideoPreview e quatro benefícios; verificar data `05/01/2026`, certificação SESI, textos e uso dos ícones pêssego.
- [x] 4.2 Implementar Sobre o curso e os três grupos Accordion Mantine com todo o conteúdo das fixtures; verificar renderização integral dos dez módulos/tópicos, Aulas online e Provas e Avaliações, e dez FAQs sem cortes ou altura fixa.
- [x] 4.3 Configurar os acordeões inicialmente fechados e expansão/recolhimento independentes; verificar por mouse e teclado os três abertos simultaneamente, o fechamento de um sem afetar os demais e os estados acessíveis de mais/menos.
- [x] 4.4 Implementar a barra flutuante condicionada à passagem do final do hero, com compensação de espaço e foco; verificar ausência no topo, exibição após rolagem, desaparecimento no retorno e leitura do fim dos acordeões/footer em desktop e mobile.
- [x] 4.5 Integrar depoimentos na variante branca/pêssego, CTA de contato visual, footer e links Matricule-se/Início; verificar carrossel, duas CTAs levando ao AVA, Início levando à Landing e contato/play sem operações ou mensagens fictícias.
- [x] 4.6 Validar Detalhes completo em 1440, 1024, 768 e 375 px, incluindo acordeões abertos, textos longos, foco e barra fixa; comparar às capturas, executar typecheck/build e corrigir erros de console, assets e regressões nos componentes compartilhados.
- [x] 4.7 Documentar a URL de Detalhes, expansão simultânea, barra fixa de matrícula e natureza demonstrativa de Matricule-se no README; verificar que descrições de avaliações/certificação são identificadas como conteúdo da referência, não recursos implementados.

## 5. Ambiente de aprendizagem demonstrativo

- [x] 5.1 Implementar o layout próprio do AVA com identificação/progresso escuros, navegação inferior clara e coluna principal de rolagem independente; verificar em desktop a sidebar estável do topo ao fim e a ausência de header/footer de marketing.
- [x] 5.2 Renderizar título do curso, progresso de 10%, busca visual rotulada e árvore lateral com estados documentados; verificar check/play/círculo vazio, seis módulos recolhidos, indisponibilidade acessível e ausência de filtro, novas lições ou alteração de progresso.
- [x] 5.3 Implementar banner panorâmico com marca local e artigo textual integral; verificar três parágrafos de Boas práticas e quatro de Recomendações até o último texto, sem truncamento, rasterização do artigo ou recursos externos.
- [x] 5.4 Implementar retorno explícito à URL de Detalhes e indicação acessível/documental de demonstração; verificar entrada direta no AVA em nova aba, retorno sem histórico prévio e ciclo Detalhes → AVA → Detalhes sem falsa confirmação de matrícula ou salvamento.
- [x] 5.5 Adaptar a sidebar para Drawer Mantine em mobile, com acionamento acessível, Escape, fechamento e retorno de foco; verificar o acesso a navegação, artigo e seta de retorno em 768 e 375 px sem conteúdo encoberto.
- [x] 5.6 Validar o AVA inteiro no navegador em 1440, 1024, 768 e 375 px, incluindo rolagem, teclado e controles ilustrativos; comparar às capturas, executar typecheck/build e verificar progresso ainda em 10% após interações/refresh, console limpo e ausência de persistência de usuário.
- [x] 5.7 Documentar a URL do AVA, conteúdo disponível, progresso estático e limites da busca/sidebar no README; verificar que a documentação não descreve autenticação, matrícula real, novas aulas ou um LMS funcional.

## 6. Verificações integradas e entrega

- [x] 6.1 Executar no navegador o ciclo completo de navegação, as três URLs diretas com refresh, histórico, URL desconhecida e slug desconhecido em dev e preview do build; verificar telas correspondentes e 404 consistente sem API ou banco em execução.
- [x] 6.2 Executar `pnpm install --frozen-lockfile`, typecheck e build finais após a integração de todas as telas; verificar códigos zero, um lockfile centralizado e ausência de dependências proibidas, registrando lint como não aplicável se continuar sem ferramenta configurada.
- [x] 6.3 Revalidar visualmente as três telas completas na matriz de larguras, incluindo estados abertos, navegação móvel, foco e movimento reduzido; salvar evidências e verificar que regressões de componentes compartilhados foram corrigidas, registrando somente divergências de fato observadas.
- [x] 6.4 Inspecionar console e rede durante todos os fluxos integrados e conferir o código de acesso a dados/armazenamento; verificar ausência de erros relevantes, assets/fontes quebrados, chamadas ao backend/terceiros, embeds remotos e persistência de matrícula/progresso.
- [x] 6.5 Comparar hashes/lista dos assets com o inventário inicial e revisar o diff completo; verificar preservação byte a byte dos recursos, ausência de alterações em backend/banco/Compose e escopo mínimo dos ajustes de raiz.
- [x] 6.6 Reproduzir os comandos finais documentados e apresentar o relatório de implementação com páginas/fluxos, componentes, rotas, assets, dependências/justificativas, configurações, comandos e resultados reais; verificar que inclui limites de simulação, fallback de hospedagem estática e qualquer validação não realizada, sem declarar a implementação concluída com verificações essenciais pendentes.
