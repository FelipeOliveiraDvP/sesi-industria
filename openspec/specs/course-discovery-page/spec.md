# course-discovery-page Specification

## Purpose

Permitir conhecer a proposta de capacitação SESI Indústria e percorrer os cursos e depoimentos apresentados na Landing, com conteúdo demonstrativo integral e navegação apenas para destinos documentados.

## Requirements

### Requirement: Landing com conteúdo integral e ordem documentada

A Landing SHALL apresentar header, hero industrial, Principais cursos, Impacto transformador, depoimentos, CTA de matrícula e footer, nesta ordem. Textos e hierarquia SHALL corresponder a `docs/figma-reverse-engineering/pages/001-landing-page.md`, sem reduzir a entrega à primeira dobra ou inserir seções de negócio adicionais.

#### Scenario: Leitura completa da página inicial

- **WHEN** o usuário abre `/` e percorre a página até o fim
- **THEN** encontra o hero com `Impulsione sua carreira na indústria`, descrição e `Conhecer cursos`
- **AND** encontra todas as seções subsequentes, inclusive a CTA `Pronto para transformar sua carreira?` e o footer completo
- **AND** a fotografia do hero, sua sobreposição vermelha e a thumbnail da CTA utilizam os arquivos locais correspondentes

### Requirement: Apresentação dos seis cursos de destaque

A seção Principais cursos SHALL apresentar os cursos recebidos da API, com título, descrição breve, categoria e imagem de cada item, sem lista de cursos local ou ranking. No seed inicial serão os seis cursos documentados, com os badges correspondentes. O trilho MUST terminar com Todos os cursos, e os controles de catálogo sem destino continuarão indisponíveis.

#### Scenario: Conteúdo dos cards

- **WHEN** o usuário percorre todo o trilho após carregar o seed inicial
- **THEN** encontra Matemática para a Precisão: Medidas e Escalas na Metalurgia, Geometria e Modelagem no Design Têxtil e Estatística e Controle de Qualidade na Produção com badge Matemática
- **AND** encontra Física do Movimento e da Energia na Metalurgia, Química e Sustentabilidade e Ciência dos Materiais na Metalurgia com badge Ciências da Natureza
- **AND** cada card utiliza o título, a descrição breve, o identificador e a imagem da resposta, preservando o visual documentado
- **AND** Todos os cursos e Ver todos os cursos permanecem visíveis e indisponíveis, sem abrir uma página de catálogo

#### Scenario: Ordem sem classificação editorial

- **WHEN** a API retorna a coleção em sua ordem técnica estável
- **THEN** o trilho respeita essa ordem sem ordenar por popularidade, destaque, status ou preferências locais
- **AND** nenhum curso do seed é ocultado implicitamente por estar encerrado, em breve ou sem data

#### Scenario: Coleção dinâmica sem requisições por card

- **WHEN** a consulta da coleção é concluída
- **THEN** a Landing monta os cards a partir dos itens retornados, usando seus identificadores
- **AND** não faz uma consulta de detalhes para cada item apenas para obter a descrição ou imagem
- **AND** não acrescenta busca, filtros ou contador visual ausentes da engenharia reversa

### Requirement: Navegação local do carrossel de cursos

O carrossel SHALL permitir avançar e retornar pelo trilho carregado com controles acessíveis. A navegação MUST ser finita, local e não automática, sem duplicar cursos, criar vazios indevidos ou rolagem horizontal do documento. A chegada dos dados e alterações da largura deverão manter os limites dos controles coerentes.

#### Scenario: Avanço e retorno entre os grupos de cards

- **WHEN** o usuário aciona a seta de avançar uma ou mais vezes após o carregamento
- **THEN** o trilho revela os cursos seguintes até o último item retornado e o card Todos os cursos
- **AND** ao acionar a seta de voltar consegue retornar ao grupo inicial
- **AND** mantém a ordem da resposta em desktop e mobile
- **AND** as setas apenas deslocam o trilho, sem solicitar outra página da API

#### Scenario: Limites do trilho

- **WHEN** o usuário chega ao começo ou ao fim do carrossel
- **THEN** a ação correspondente ao limite não desloca o conteúdo para além dos cards
- **AND** o estado indisponível do controle é comunicado de forma acessível

#### Scenario: Carrossel após carregamento assíncrono

- **WHEN** os placeholders são substituídos pelos cards recebidos
- **THEN** os controles refletem o tamanho real da coleção e da área visível
- **AND** continuam corretos após redimensionamento ou nova consulta

### Requirement: Métricas e benefícios da apresentação institucional

A seção Impacto transformador SHALL apresentar o texto de apoio documentado, quatro métricas estáticas e os três benefícios Certificação SESI, Professores especialistas e Suporte contínuo, com suas descrições e ícones locais na variante vermelha.

#### Scenario: Conferência dos indicadores e benefícios

- **WHEN** o usuário alcança Impacto transformador
- **THEN** visualiza `25.000+` Profissionais Capacitados, `500+` Empresas parceiras, `4.8/5` Avaliação média e `2.5M+` Horas de capacitação
- **AND** visualiza os três benefícios completos abaixo da área de métricas
- **AND** os valores são conteúdo estático da referência, não estatísticas consultadas ou calculadas a partir de usuários reais

### Requirement: Depoimentos completos em carrossel

A seção Quem fez recomenda SHALL apresentar os cinco depoimentos documentados com citação, autor, cargo e cinco estrelas. A Landing SHALL usar cards vermelhos sobre vermelho profundo, com navegação local de avanço e retorno, sem avaliações recebidas de serviços externos.

#### Scenario: Percurso pelos cinco depoimentos

- **WHEN** o usuário percorre o carrossel de depoimentos com seus controles
- **THEN** consegue ler as citações completas de Lucas Pereira, Mariana Costa, Thiago Almeida, Fernanda Silva e Gabriel Santos
- **AND** os cargos e a classificação visual correspondem à documentação
- **AND** consegue retornar aos depoimentos iniciais sem alterar de página

### Requirement: Destinos confirmados da Landing

Cursos no header e Conhecer cursos SHALL continuar levando à seção Principais cursos. Todo card de curso MUST abrir `/cursos/{id}` com seu próprio identificador. Matricule-se na CTA inferior abrirá os detalhes de Matemática, independentemente de sua posição na coleção. As demais ações sem destino continuarão indisponíveis.

#### Scenario: Acesso à seção de cursos

- **WHEN** o usuário aciona Cursos no header da Landing ou Conhecer cursos no hero
- **THEN** a seção Principais cursos é trazida para a área visível sem ser encoberta pelo header
- **AND** a tela permanece na Landing, admitindo apenas uma âncora local na URL

#### Scenario: Entrada nos detalhes do curso documentado

- **WHEN** o usuário aciona o card de Matemática ou Matricule-se na CTA inferior
- **THEN** a aplicação abre os detalhes do curso de Matemática pela rota de seu identificador
- **AND** não registra matrícula nem apresenta confirmação de operação remota

#### Scenario: Entrada nos demais cursos

- **WHEN** o usuário aciona qualquer um dos outros cinco cards do seed
- **THEN** abre os detalhes do identificador correspondente, com os dados daquele curso
- **AND** não é encaminhado ao conteúdo de Matemática por um destino fixo compartilhado

#### Scenario: Controles sem fluxo identificado

- **WHEN** o usuário encontra Todos os cursos, Ver todos os cursos, Sobre, Área do aluno ou o play da thumbnail
- **THEN** nenhum desses elementos abre página, login, player ou operação não documentados
- **AND** os controles indisponíveis e o conteúdo ilustrativo são distinguíveis de ações funcionais por seus recursos de acessibilidade

