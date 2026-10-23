# Spec Delta

## MODIFIED Requirements

### Requirement: Conjunto inicial fictício com diversidade mínima

A inicialização SHALL disponibilizar exatamente seis cursos, três categorias, duas modalidades e três status, todos fictícios. Os cursos devem variar em carga horária, público-alvo, quantidade de módulos e descrições de aulas, mantendo todas as associações válidas.

#### Scenario: Catálogo inicial preparado

- **WHEN** a inicialização é concluída em um banco vazio
- **THEN** o catálogo contém exatamente seis cursos, três categorias, duas modalidades e três status
- **AND** os cursos possuem cargas horárias e públicos-alvo variados
- **AND** existem quantidades variadas de módulos e descrições distintas de aulas
- **AND** todos os dados são fictícios e possuem relacionamentos válidos

## ADDED Requirements

### Requirement: Cursos iniciais alinhados à engenharia reversa

O conjunto inicial SHALL incluir os seis nomes e as seis descrições breves transcritos dos cards de cursos da documentação da landing page, mantendo todos os dados fictícios.

#### Scenario: Nomes e descrições dos cards

- **WHEN** o catálogo inicial é preparado
- **THEN** os cursos incluem os seguintes pares de nome e descrição:
  - `Matemática para a Precisão: Medidas e Escalas na Metalurgia` — `Aplicação de grandezas, proporções e escalas em processos de corte, furação e usinagem.`
  - `Geometria e Modelagem no Design Têxtil` — `Explora formas geométricas, simetria e padrões matemáticos presentes em tecidos e estampas.`
  - `Estatística e Controle de Qualidade na Produção` — `Leitura e interpretação de dados de controle de qualidade e produtividade.`
  - `Física do Movimento e da Energia na Metalurgia` — `Estudo de forças, atrito, calor e energia em máquinas, fornos e prensas.`
  - `Química e Sustentabilidade` — `Estudo das reações químicas envolvidas na tinturaria e acabamento de tecidos.`
  - `Ciência dos Materiais na Metalurgia` — `Conceitos básicos de estrutura, resistência e propriedades dos metais.`

### Requirement: Atualização de registros do seed em bancos existentes

Uma atualização versionada do banco SHALL substituir os dados dos registros pertencentes ao seed anterior pelos novos valores do catálogo inicial, sem alterar ou remover registros que não pertençam ao conjunto gerenciado pelo seed.

#### Scenario: Banco já contém o seed anterior

- **WHEN** a atualização é aplicada a um banco que contém o catálogo inicial anterior e registros adicionais fora do seed
- **THEN** os registros gerenciados pelo seed passam a corresponder aos novos cursos, classificações, módulos e aulas
- **AND** os registros adicionais que não pertencem ao seed permanecem inalterados

#### Scenario: Atualização repetida

- **WHEN** o operador repete a atualização do banco após a nova versão já ter sido aplicada
- **THEN** os novos dados do catálogo permanecem estáveis e não são duplicados
- **AND** os registros fora do conjunto gerenciado pelo seed permanecem preservados
