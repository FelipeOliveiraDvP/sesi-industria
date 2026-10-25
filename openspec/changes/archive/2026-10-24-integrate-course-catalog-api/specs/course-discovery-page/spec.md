# Spec Delta

## MODIFIED Requirements

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
