# Spec Delta

## MODIFIED Requirements

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
