# frontend-visual-foundation Specification

## Purpose

Manter uma identidade SESI Indústria consistente entre as telas, utilizando os recursos locais existentes e tornando os layouts documentados utilizáveis em diferentes larguras e por tecnologias assistivas.

## Requirements

### Requirement: Referência visual comum às telas

As telas SHALL reproduzir a hierarquia, paleta, proporções, tipografia, espaçamentos e superfícies evidenciados em `docs/figma-reverse-engineering/`, sem incluir a moldura ou controles do visualizador Figma. Valores estimados SHALL ser tratados como aproximações sujeitas à conferência visual, não como tokens nativos extraídos.

#### Scenario: Conferência do produto sem a moldura do protótipo

- **WHEN** a interface é comparada às capturas de referência em desktop
- **THEN** a comparação considera somente a área do produto, com escala equivalente
- **AND** a aplicação não inclui margens pretas do visualizador, lista de Flows, controles de frames ou login do Figma
- **AND** preserva as cores de referência, incluindo superfícies brancas, texto quase preto, vermelhos institucionais, pêssego e rodapé vinho escuro

### Requirement: Assets existentes preservados e servidos localmente

A aplicação SHALL preservar nomes, estrutura e conteúdo dos 65 arquivos de `public/assets`, evitar cópias e carregar imagens de curso pelas URLs recebidas da API, sempre referenciando esses assets do frontend. Os demais recursos locais e as proporções reservadas MUST ser preservados, sem substituição por imagens de terceiros ou arquivos ilustrativos do backend.

#### Scenario: Uso dos recursos fornecidos

- **WHEN** o usuário visita Landing, Detalhes e AVA na aplicação compilada
- **THEN** hero, thumbnails, banners, logos, ícones e fontes são carregados dos assets públicos do frontend
- **AND** as imagens específicas de curso utilizam as URLs fornecidas nos contratos da API
- **AND** não há substituição por imagens de terceiros ou vetores genéricos de outro kit
- **AND** as imagens fora da primeira dobra podem carregar de forma adiada sem quebrar suas proporções

#### Scenario: Fotografias dos cards indisponíveis

- **WHEN** a Landing apresenta os seis cursos sem fotografias individuais originais
- **THEN** utiliza os mesmos recortes dos PNGs já aprovados, agora referenciados pelo seed e retornados pela API
- **AND** mantém badges, gradientes, textos e dimensões dos cards, admitindo a ordem retornada pelo catálogo sem ranking
- **AND** documenta a diferença fotográfica como divergência aceita, sem alegar equivalência integral nessa parte

### Requirement: Tipografia local coerente

A interface SHALL utilizar a família Open Sans fornecida em `public/assets/_Font`, com pesos adequados a títulos e corpo, mantendo sua licença no conjunto de assets. A renderização SHALL dispensar serviços de fontes externos e não carregar todas as variantes estáticas sem necessidade.

#### Scenario: Carregamento da fonte da interface

- **WHEN** o usuário abre a aplicação sem acesso a serviços externos
- **THEN** os textos utilizam a fonte local disponibilizada pela aplicação
- **AND** títulos, navegação, labels e parágrafos mantêm hierarquia e legibilidade consistentes
- **AND** a escolha da Open Sans é registrada como uso do asset disponível, não como identificação confirmada da fonte original do Figma

### Requirement: Header e footer compartilhados com variações documentadas

Landing e Detalhes SHALL apresentar a mesma marca, itens de navegação e estrutura de footer. Na Landing, o header SHALL permanecer visível ao rolar, com variante clara após o hero; nos Detalhes, SHALL sair do viewport. O footer SHALL preservar colunas, textos e ícones documentados, sem criar páginas para seus itens sem destino confirmado.

#### Scenario: Header da Landing no topo e após rolagem

- **WHEN** o usuário abre a Landing e depois rola para as seções de conteúdo
- **THEN** o header começa integrado ao hero vermelho com marca e textos claros
- **AND** permanece visível em superfície branca, com marca vermelha e navegação escura, conforme as capturas de rolagem

#### Scenario: Header e footer dos Detalhes

- **WHEN** o usuário percorre a página de Detalhes do topo ao fim
- **THEN** o header claro rola com o documento em vez de permanecer fixo
- **AND** o footer apresenta a marca, contato, navegação, cursos populares, suporte e linha inferior do componente compartilhado
- **AND** seus elementos permanecem alcançáveis mesmo com a barra fixa de matrícula

### Requirement: Acessibilidade das ações e do conteúdo

A interface SHALL usar títulos e regiões semânticas, links para navegação, botões para ações locais, nomes acessíveis e foco visível. Controles MUST funcionar por teclado e comunicar estados, inclusive carregamento e falha. Conteúdo decorativo será distinguido de imagens informativas, e atualizações assíncronas não provocarão perda indevida de foco.

#### Scenario: Navegação por teclado

- **WHEN** o usuário percorre as ações disponíveis com Tab, Enter, Espaço e teclas próprias dos componentes
- **THEN** consegue acionar links de todos os cursos, carrosséis, acordeões, recuperação de falhas e navegação móvel sem mouse
- **AND** identifica visualmente o foco e os estados expandidos ou indisponíveis
- **AND** botões apenas com ícones têm nomes acessíveis e imagens informativas possuem alternativas textuais coerentes com o asset

#### Scenario: Mudança de rota ou abertura da navegação móvel

- **WHEN** o usuário muda de tela ou abre e fecha a navegação em viewport pequeno
- **THEN** o título da página e o foco permitem reconhecer o novo contexto e o curso consultado
- **AND** o fechamento de uma sobreposição devolve o foco ao controle que a abriu
- **AND** não existe armadilha de teclado ou controle essencial encoberto

#### Scenario: Resposta assíncrona sem novo deslocamento de foco

- **WHEN** uma consulta termina depois que a rota já foi apresentada ou um dado em cache é atualizado
- **THEN** o título e a indicação acessível acompanham o estado e o curso corretos
- **AND** a atualização não reposiciona arbitrariamente o foco ou a rolagem de quem já iniciou a leitura

### Requirement: Controles sem comportamento confirmado não inventam funcionalidades

Exceto a navegação de todos os cursos e as ações técnicas de recuperação autorizadas nesta integração, controles sem destino documentado SHALL continuar indisponíveis de forma acessível ou não interativos. Sua ativação MUST NOT abrir fluxos adicionais, reproduzir mídia ausente, emitir confirmação de serviço ou enviar dados.

#### Scenario: Ações meramente ilustrativas

- **WHEN** o usuário encontra Sobre, Área do aluno, links não confirmados do footer, contato, play sem mídia ou controles ilustrativos do AVA
- **THEN** a interface comunica a indisponibilidade sem gerar nova página, modal ou falsa mensagem de sucesso
- **AND** a indicação acessível não altera desnecessariamente a aparência da referência

#### Scenario: Limite das novas ações

- **WHEN** os cursos passam a carregar da API
- **THEN** seus cards e o ciclo Detalhes/AVA são navegáveis
- **AND** não são introduzidos busca, filtros, página de catálogo ou ativação dos controles Todos os cursos e Ver todos os cursos
- **AND** botões de nova tentativa aparecem somente nos estados técnicos de falha correspondentes

### Requirement: Adaptação responsiva sem perda de conteúdo

As telas SHALL ser utilizáveis em desktop, tablet e mobile, preservando a ordem de leitura e todo o conteúdo documentado. Navegação, colunas, carrosséis e elementos fixos SHALL se adaptar sem overflow horizontal do documento ou sobreposição que impeça ações. Preferências de movimento reduzido SHALL ser respeitadas.

#### Scenario: Layout em larguras representativas

- **WHEN** a aplicação é exibida em 1440, 1024, 768 e 375 pixels de largura
- **THEN** textos e imagens continuam legíveis, colunas se reorganizam quando necessário e a navegação permanece acessível
- **AND** somente trilhos de carrossel têm rolagem horizontal intencional, sem fazer a página inteira transbordar
- **AND** o acesso ao conteúdo do AVA e aos CTAs não depende de uma sidebar ou barra que encubra a área útil

#### Scenario: Preferência por menos movimento

- **WHEN** o sistema do usuário solicita movimento reduzido
- **THEN** transições e rolagens animadas não essenciais são reduzidas ou desativadas
- **AND** nenhuma informação ou ação deixa de estar disponível
