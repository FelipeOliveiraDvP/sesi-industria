# Spec Delta

## ADDED Requirements

### Requirement: Seed genérico de módulos e aulas do AVA

Cada apresentação persistida SHALL conter a mesma estrutura ordenada de seis módulos genéricos do AVA, cada um com duas aulas que incluem rótulo e descrição. Essa estrutura demonstrativa SHALL ser separada dos módulos e aulas relacionais específicos usados em Detalhes e SHALL NOT representar progresso de aluno.

#### Scenario: Preparação de um banco vazio

- **WHEN** o seed é aplicado a um banco vazio
- **THEN** as seis apresentações de curso contêm os mesmos seis módulos e as mesmas duas aulas genéricas por módulo, na mesma ordem
- **AND** os módulos e aulas relacionais de cada curso permanecem específicos e válidos
- **AND** o progresso demonstrativo continua fixo, sem registros de aluno

#### Scenario: Atualização do catálogo existente

- **WHEN** o operador aplica as migrations e o seed pelo fluxo documentado em um banco local já populado com os seis cursos
- **THEN** as apresentações dos cursos passam a conter a estrutura genérica de módulos e aulas do AVA
- **AND** descrições, artigos, recomendações e módulos relacionais de Detalhes permanecem inalterados
- **AND** a atualização é determinística e não duplica cursos, módulos ou aulas
