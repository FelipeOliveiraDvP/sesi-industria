# Detalhes curso

## 1. Identificação

- **ID da página:** 002
- **Identificador do frame:** `page-id=3:2`, `node-id=6017:707` — fluxo `02. Detalhes curso`.
- **URL:** [abrir Detalhes curso no protótipo](https://www.figma.com/proto/YB0o91nnOEB5bXXCtmy5s6/-TEST-DEV--SESI-Ind%C3%BAstria?node-id=6017-707&p=f&viewport=-1570%2C411%2C0.09&t=tvoKbn8KIvr7vLak-1&scaling=contain&content-scaling=fixed&starting-point-node-id=6017%3A707&page-id=3%3A2)
- **Página de origem:** Landing page (`6017:342`), pelo card 1 do carrossel ou pelo CTA inferior `Matricule-se`; também selecionável pela lista `Flows`.
- **Forma de navegação até esta página:** clique no primeiro card `Matemática para a Precisão: Medidas e Escalas na Metalurgia` ou no CTA inferior `Matricule-se` da Landing. `Início` retorna à Landing. A página é também selecionável diretamente como flow 02.
- **Estados inspecionados:** viewport inicial; rolagem até o fim; os três acordeões expandidos; barra fixa de matrícula; clique no play; CTA `Mandar uma mensagem`; retorno após o AVA.
- **Dimensões do viewport do navegador:** 1908 × 844 px CSS, DPR 1 (**confirmado pelo DOM**).
- **Frame visível:** aproximadamente `x=309, y=60, 1288 × 724 px` no screenshot (**estimado pelo raster**); canvas do visualizador `x=48, y=60, 1812 × 724 px` (**confirmado pelo DOM**).
- **Dimensões totais estimadas:** estado com `Módulos`, `Aulas online e avaliações` e `Dúvidas frequentes` expandidos, cerca de 1440 × 6.400 px (**estimado** pela soma de rolagens; dimensão não exposta pelo DOM). A altura no estado fechado é menor e não foi medida.

## 2. Objetivo e descrição geral

Página de apresentação de um curso de Matemática para a Precisão. Mostra título, início, certificação, benefícios, descrição, módulos, informações de aulas/avaliações, perguntas frequentes, depoimentos, CTA de contato e rodapé. Uma faixa fixa de matrícula aparece depois da rolagem.

## 3. Estrutura visual

1. **Header compartilhado** com logotipo e navegação `Início`, `Cursos`, `Sobre`, `Área do aluno`; aparece no topo antes da rolagem e sai do viewport ao rolar.
2. **Hero de curso em duas colunas:** badge e textos à esquerda; fotografia 16:9 com botão de play à direita. O CTA principal é escuro.
3. **Faixa de benefícios:** quatro colunas com ícone, título e descrição.
4. **Sobre o curso e acordeões:** duas colunas. Descrição à esquerda; `Módulos`, `Aulas online e avaliações` e `Dúvidas frequentes` à direita. O conteúdo expandido flui verticalmente e aumenta muito a página.
5. **Depoimentos:** fundo laranja claro, cards brancos e setas do carrossel.
6. **CTA de ajuda:** seção vermelha centrada, com eyebrow, heading e botão.
7. **Footer:** mesmo sistema de colunas da Landing. Quando a barra fixa está visível, ela cobre parte da área inferior do footer.
8. **Barra fixa:** surge após a rolagem, posicionada sobre o conteúdo perto da borda inferior do viewport; permanece visível ao rolar.

As posições abaixo usam pixels da screenshot do viewport, não coordenadas de nodes Figma. O frame, limites e componentes internos são **estimados visualmente**.

## 4. Conteúdo textual

Os textos abaixo são **Confirmados por transcrição visual** das screenshots. O conteúdo parcialmente encoberto pela barra fixa está marcado como tal; não foi possível extrair strings internas do canvas via DOM.

### Header e hero

- Logotipo: `SESI + INDÚSTRIA`
- Navegação: `Início` · `Cursos` · `Sobre` · `Área do aluno`
- Badge: `MATEMÁTICA`
- Título: `Matemática para a Precisão: Medidas e Escalas na Metalurgia`
- `Início do curso: 05/01/2026`
- `Certificação: SESI`
- Botão: `Matricule-se →`
- Ícone sobre a fotografia: play triangular em círculo.

### Benefícios

- `Certificação SESI` — `Cursos com a melhor certificação do mercado industrial reconhecidos pelo SESI.`
- `Professores especialistas` — `Contamos com professores reconhecidos por sua excelência no mercado.`
- `Suporte contínuo` — `Você não está sozinho! Acompanhamento online durante todo o curso.`
- `Flexibilidade` — `Turmas presenciais e online com horários que se adaptam à sua rotina.`

### Sobre o curso

**Título:** `Sobre o curso`

`Explore nosso curso de Matemática para a Precisão: Medidas e Escalas na Metalurgia, perfeito para quem deseja se destacar nesse campo! Este programa abrangente foi desenvolvido para capacitar profissionais com as habilidades e conhecimentos fundamentais para garantir medições precisas e eficientes no ambiente metalúrgico.`

### Acordeão `Módulos`

1. **Introdução à Matemática na Metalurgia**
   - Conceitos fundamentais de medidas e escalas aplicados à metalurgia.
   - Importância da precisão nas medições e controle de qualidade.
   - Panorama das normas e regulamentos no Brasil relacionados à metalurgia.
2. **Identificação e Avaliação de Medidas**
   - Tipos de medidas utilizadas: comprimento, massa, volume e temperatura.
   - Técnicas de análise de precisão nas medições.
   - Elaboração de gráficos e tabelas de medidas.
3. **Normas e Regulamentações**
   - Estudo das principais normas aplicáveis à metalurgia (ABNT, ISO, entre outras).
   - Interpretação e aplicação prática das normas de medição.
   - Atualizações e impactos legais nas práticas de medição.
4. **Instrumentos de Medição**
   - Tipos de instrumentos utilizados na metalurgia: paquímetros, micrômetros e balanças.
   - Critérios de seleção, uso e calibração.
   - Responsabilidades legais de operadores e supervisores.
5. **Prevenção de Erros de Medição**
   - Tipos de erros e incertezas nas medições.
   - Procedimentos para garantir a precisão e exatidão.
   - Treinamento prático em técnicas de medição.
6. **Ergonomia e Eficiência na Medição**
   - Princípios ergonômicos aplicados ao uso de instrumentos de medição.
   - Prevenção de lesões por uso inadequado de ferramentas.
   - Qualidade de vida no ambiente de trabalho.
7. **Análise de Resultados de Medição**
   - Técnicas de análise de dados de medições.
   - Elaboração de relatórios e planos de ação para melhorias.
   - Indicadores de precisão e melhoria contínua.
8. **Cultura de Precisão e Comportamento Seguro**
   - Desenvolvimento de uma cultura organizacional voltada à precisão nas medições.
   - Comunicação eficaz e liderança em práticas de medição.
   - Programas de conscientização e engajamento dos colaboradores.
9. **Tecnologias Aplicadas à Medição**
   - Uso de plataformas digitais, sensores e automação na medição de precisão.
   - Sistemas de gestão de qualidade.
   - Tendências e inovações no setor metalúrgico.
10. **Avaliação Final e Certificação**
    - Teste de conhecimentos adquiridos sobre medidas e escalas.
    - Atividades práticas e estudos de caso.
    - Emissão de certificado digital de conclusão.

### Acordeão `Aulas online e avaliações`

**Aulas online**

`As aulas são organizadas em módulos temáticos e disponibilizadas em formato multimídia, combinando:`

- Vídeo-aulas com especialistas da área, gravadas em alta qualidade e com recursos visuais que facilitam a compreensão dos conteúdos técnicos.
- Materiais complementares como apostilas digitais, infográficos, artigos e estudos de caso.
- Atividades interativas, como quizzes, simulações e fóruns de discussão, que promovem o engajamento e a troca de experiências entre os participantes.
- Acesso multiplataforma, permitindo que os alunos estudem pelo computador, tablet ou celular, a qualquer hora e de qualquer lugar.

`Cada aula é acompanhada de recursos de acessibilidade, como legendas, transcrição de áudio e navegação intuitiva, garantindo inclusão e autonomia para todos os perfis de usuários.`

**Provas e Avaliações**

`As avaliações são realizadas diretamente na plataforma, com foco na verificação do aprendizado e na aplicação prática dos conhecimentos adquiridos:`

- Provas objetivas ao final de cada módulo, com correção automática e feedback imediato.
- Estudos de caso e atividades práticas, que simulam situações reais da indústria e exigem análise crítica e tomada de decisão.
- Avaliação final integradora, que consolida os conteúdos do curso e valida a competência do aluno para atuar com segurança no ambiente industrial.
- Sistema antifraude, com controle de tempo, embaralhamento de questões e, opcionalmente, reconhecimento facial para garantir a integridade das avaliações.

`Ao concluir o curso e ser aprovado nas avaliações, o aluno recebe um certificado digital reconhecido, que pode ser compartilhado em redes profissionais e utilizado para comprovação de capacitação técnica.`

### Acordeão `Dúvidas frequentes`

1. **O curso é totalmente online?**  
   Sim. Todas as aulas, atividades e avaliações são realizadas pela plataforma digital, permitindo que o aluno estude de onde estiver, no seu próprio ritmo.
2. **Preciso estar disponível em horários específicos?**  
   Não. As aulas são gravadas e ficam disponíveis 24 horas por dia. O aluno pode acessar o conteúdo quando for mais conveniente, respeitando os prazos de cada módulo.
3. **Como são feitas as avaliações?**  
   As avaliações são realizadas diretamente na plataforma, por meio de provas objetivas, estudos de caso e atividades práticas. Algumas avaliações podem ter tempo limitado e exigem atenção às instruções.
4. **O curso oferece certificado?**  
   Sim. Ao concluir todos os módulos e ser aprovado nas avaliações, o aluno recebe um certificado digital de conclusão, válido em todo o território nacional.
5. **Quem são os professores?**  
   O corpo docente é formado por especialistas, engenheiros e professores com ampla experiência em segurança do trabalho e atuação direta na indústria. Os perfis dos docentes estão disponíveis na plataforma.
6. **Há suporte para dúvidas durante o curso?**  
   Sim. Os alunos podem enviar dúvidas por meio dos fóruns de discussão, chats da plataforma ou durante os webinários ao vivo. A equipe pedagógica está disponível para auxiliar durante todo o curso.
7. **O curso exige algum conhecimento prévio?**  
   Não é obrigatório, mas é recomendado que o aluno tenha interesse ou alguma familiaridade com o ambiente industrial. O curso foi estruturado para atender tanto iniciantes quanto profissionais em busca de atualização.
8. **Posso acessar o curso pelo celular?**  
   Sim. A plataforma é responsiva e compatível com computadores, tablets e smartphones, garantindo uma experiência completa em qualquer dispositivo.
9. **Quanto tempo tenho para concluir o curso?**  
   O prazo para conclusão varia conforme o plano escolhido. Em geral, o aluno tem entre 3 a 6 meses para finalizar todos os módulos e avaliações.
10. **O curso é reconhecido por órgãos oficiais?**  
    O curso segue as diretrizes das Normas Regulamentadoras (NRs) e boas práticas da área, sendo reconhecido como formação complementar. Para certificações oficiais, é necessário verificar os requisitos específicos de cada órgão regulador.

### Depoimentos

- Eyebrow: `DEPOIMENTOS`
- Título: `Quem fez recomenda`
- `“A experiência do curso foi incrível! As aulas foram muito bem estruturadas e o cronograma foi cuidadosamente planejado, facilitando muito o aprendizado.”` — **Lucas Pereira**, `Analista na Empresa X`, `★★★★★`
- `“Concluir o curso foi uma jornada enriquecedora. As aulas eram dinâmicas e o horário foi organizado para maximizar nosso tempo de estudo.”` — **Mariana Costa**, `Gestora na Empresa X`, `★★★★★`
- `“O curso superou minhas expectativas! As aulas eram bem elaboradas e o planejamento do horário ajudou a manter o foco e a produtividade.”` — **Thiago Almeida**, `Técnico na Empresa X`, `★★★★★`
- `“Finalizar o curso foi uma experiência motivadora. As aulas foram muito bem preparadas e o cronograma foi ideal para o aprendizado.”` — **Fernanda Silva**, `Analista na Empresa X`, `★★★★★`
- `“A conclusão do curso foi uma etapa muito gratificante. As aulas eram bem organizadas e o horário foi pensado para facilitar nosso aprendizado.”` — **Gabriel Santos**, `Analista na Empresa X`, `★★★★★`

### CTA e rodapé

- Eyebrow: `PRECISA DE AJUDA PARA DECIDIR?`
- Título: `Fale com a gente`
- Botão: `Mandar uma mensagem →`
- Logotipo: `SESI + INDÚSTRIA`
- Contato: `contato@sesiindustria.com.br`
- Endereço: `Av. Paulista, 1313`
- `NAVEGAÇÃO`: `Início`, `Certificação`, `Cursos`, `Sobre`, `Blog`
- `CURSOS POPULARES`: `Cultura Organizacional`, `Ferramentas Google para Produtividade`, `Excel Básico ao Avançado`, `Trabalho em Equipe e Colaboração`, `Gestão do Tempo e Produtividade`, `Comunicação Assertiva`; `Inteligência Emocional no Trabalho` aparece parcialmente encoberto pela barra fixa (o texto completo está visível na Landing).
- `SUPORTE`: `Termos de Uso`, `Política de Privacidade`, `Trabalhe Conosco`, `Fale Conosco`, `Central de Ajuda`
- Parte inferior/copyright e ícones sociais ficam parcial ou totalmente cobertos pela barra fixa nas capturas de Detalhes; essa parte não foi transcrita como visível nesta página.

### Barra fixa de matrícula

- Título: `Matemática para a Precisão: Medidas e Escalas na Metalurgia`
- Botão: `Matricule-se →`

## 5. Componentes e posicionamento

| Componente | Posição/dimensão aproximada | Estilo e relação espacial | Classificação |
|---|---|---|---|
| Hero | screenshot inicial: área interna do frame desde `y≈187`; duas colunas até `y≈539` | Texto à esquerda; imagem retangular à direita. Badge `MATEMÁTICA` acima do título. | **Estimado** |
| Badge de curso | `x≈411, y≈203, 103 × 26 px` | Fundo pêssego, ícone matemático escuro e label em caixa alta. | **Estimado** |
| Título e dados | coluna esquerda, `x≈411`; h1 em três linhas; metadados abaixo | Título preto em bold; ícones pequenos de calendário e certificação. | **Estimado** |
| Imagem do hero | `x≈869, y≈187, 626 × 352 px`, proporção próxima de 16:9 | Cantos arredondados; overlay escuro; botão circular de play ao centro. | **Estimado** |
| CTA inicial | `x≈411, y≈485, 151 × 38 px` | Preto, texto branco e seta. | **Estimado** |
| Benefícios | quatro colunas, começa aproximadamente em `y≈628` no viewport inicial | Ícones em quadrados pêssego; título e texto alinhados à esquerda. | **Estimado** |
| Sobre/acordeões | screenshot `section-02-sobre-o-curso.png`; descrição à esquerda (`x≈411`), acordeões à direita (`x≈815`) | Heading `Sobre o curso`; linhas de acordeão com divisores finos e ícone `+`/`−`. | **Estimado** |
| Depoimentos | fundo claro amarelado/laranja | Cards brancos, mesma hierarquia de citação/autor/estrelas da Landing. | **Estimado** |
| Barra fixa | screenshot `section-05-faq-closed.png` e seguintes; largura aproximada 1084 px, altura 71 px, perto do rodapé da viewport | Retângulo preto elevado e arredondado; nome do curso à esquerda e botão branco à direita. Encobre conteúdo abaixo dela. | **Estimado** |
| CTA contato | screenshot `section-09-cta.png`; bloco vermelho | Conteúdo centralizado; eyebrow pequena, heading grande e botão branco. | **Estimado** |

## 6. Imagens e elementos gráficos

| Elemento | Posição/dimensão aproximada | Descrição visual e ajuste | Evidência / origem |
|---|---|---|---|
| Foto do hero | `≈626 × 352 px` no screenshot, lado direito | Dois profissionais em ambiente de produção; um com capacete/roupa azul demonstra equipamento; overlay escuro e play central. Recorte aparente `cover`. | `viewport.png`; nome/origem do arquivo **não identificados**. |
| Ícones de metadados | pequenos, ao lado de data e certificação | Calendário e selo/medalha, traço escuro. | `viewport.png`; origem **não identificada**. |
| Ícones dos benefícios | cerca de `45 × 45 px` no screenshot | Medalha, capelo, headset e relógio dentro de quadrados pêssego arredondados. | `viewport.png`; origem **não identificada**. |
| Cards de depoimento | largura visual aproximada de 259 px por card | Sem fotografia; texto, autoria e cinco estrelas. Fundo branco sobre seção `#FCD19C`. | `section-08-depoimentos.png`; origem de componentes **não identificada**. |
| Ícones de acordeão | sinais `+` e `−` | Ícones pretos simples, alinhados à direita dos headings. | `state-modulos-open.png`, `state-faq-open.png`. |
| Logo do footer | coluna esquerda | `SESI + INDÚSTRIA` em branco. | `section-10-footer.png`; arquivo de logo **não identificado**. |

## 7. Estilos observados

- Superfície principal branca; texto escuro próximo de `#0C0C0D` (**amostrado do raster**).
- Badge matemática e seção de depoimentos usam pêssego amostrado como `#FCD19C` (**raster**, não CSS).
- CTA final e fundo dos cards de depoimento usam vermelho profundo amostrado como `#890000`; os cards de depoimento são brancos.
- CTA de matrícula é preto/quase preto; texto e botão da barra fixa contrastam em branco.
- Acordeões usam texto em bold e corpo em cinza, divisores finos e espaçamento vertical generoso.
- Família tipográfica interna, valores CSS de sombra, raio exato e tokens de espaçamento: **não identificados**; consultar `../design-system.md`.

## 8. Interações e navegação

| Controle | Resultado observado | Classificação |
|---|---|---|
| Primeiro card na Landing | Abre esta página, nó `6017:707`. | **Confirmada** |
| `Início` no header | Retorna à Landing, nó `6017:342`. | **Confirmada** |
| `Módulos` | Expande inline e mostra os 10 módulos transcritos nesta página. | **Confirmada** |
| `Aulas online e avaliações` | Expande inline com `Aulas online` e `Provas e Avaliações`. | **Confirmada** |
| `Dúvidas frequentes` | Expande inline com 10 perguntas/respostas. | **Confirmada** |
| Acordeões simultâneos | Os três conteúdos permaneceram expandidos ao abrir os seguintes. | **Confirmada** |
| `Matricule-se →` do hero e da barra fixa | Ambos levam ao AVA, nó `6017:1042`. | **Confirmada** |
| Seta de voltar no AVA | Retorna para Detalhes curso, nó `6017:707`. | **Confirmada na página AVA** |
| Play na foto do hero | Clique não iniciou vídeo nem mudou o frame visualmente. | **Confirmada para o estado observado; reprodução não identificada** |
| `Mandar uma mensagem →` | Clique não alterou URL, tela ou quantidade de abas. | **Confirmada para a sessão; destino não identificado** |

## 9. Estados e variações

- **Inicial:** header, hero do curso e parte da faixa de benefícios; a barra fixa ainda não é visível.
- **Rolado:** o header do site sai do viewport e aparece a faixa preta fixa de matrícula na borda inferior.
- **Acordeões abertos:** `Módulos`, `Aulas online e avaliações` e `Dúvidas frequentes` foram abertos; expansão é inline, sem modal.
- **Após matrícula:** o protótipo entra no AVA. O retorno pela seta do AVA volta para este frame.
- O clique no play e no CTA de mensagem não produziu estado alternativo perceptível.
- No fim da rolagem, uma rolagem adicional produziu PNG idêntico; o footer estava no final. A barra fixa encobre parte do footer inferior.

## 10. Evidências

- [Viewport inicial](../assets/002-detalhes-curso/viewport.png)
- [Viewport com flows visíveis](../assets/002-detalhes-curso/viewport-with-flows.png)
- [Sobre o curso e início dos depoimentos](../assets/002-detalhes-curso/section-02-sobre-o-curso.png)
- [Módulos — início](../assets/002-detalhes-curso/state-modulos-open.png)
- [Módulos — continuação](../assets/002-detalhes-curso/section-03-modulos-continuacao.png)
- [Módulos — final](../assets/002-detalhes-curso/section-04-modulos-final.png)
- [Aulas online e avaliações aberto](../assets/002-detalhes-curso/state-aulas-online-open.png)
- [Aulas online — continuação e avaliações](../assets/002-detalhes-curso/section-06-aulas-online.png)
- [Avaliações — final e início das FAQ](../assets/002-detalhes-curso/section-07-avaliacoes-final.png)
- [Dúvidas frequentes — início](../assets/002-detalhes-curso/state-faq-open.png)
- [Dúvidas frequentes — continuação](../assets/002-detalhes-curso/state-faq-continuacao.png)
- [Dúvidas frequentes — final](../assets/002-detalhes-curso/state-faq-final.png)
- [Depoimentos](../assets/002-detalhes-curso/section-08-depoimentos.png)
- [CTA de contato](../assets/002-detalhes-curso/section-09-cta.png)
- [Footer](../assets/002-detalhes-curso/section-10-footer.png)
- [Confirmação de fim da rolagem](../assets/002-detalhes-curso/section-11-bottom.png)
- [Estado após retornar do AVA](../assets/002-detalhes-curso/state-return-from-ava.png)
- [Entrada pelo CTA inferior da Landing](../assets/002-detalhes-curso/state-home-cta-to-details.png)
- [Clique no play](../assets/002-detalhes-curso/state-video-click.png)

## 11. Incertezas e lacunas

- O rodapé inferior fica parcialmente encoberto pela barra fixa; o copyright e os ícones sociais não foram confirmados como conteúdo visível neste frame.
- O destino de `Mandar uma mensagem →` não foi identificado, pois o clique não gerou mudança visível.
- O play é uma affordance visual, mas não houve reprodução observada.
- Fonte, nomes/URLs de imagens, dimensões originais de nodes, valores de CSS, breakpoint responsivo e altura exata do conteúdo não estão disponíveis pela inspeção do canvas.
