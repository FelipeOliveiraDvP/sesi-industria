# Spec Delta

## ADDED Requirements

### Requirement: Aulas genéricas tipadas na prévia do AVA

O detalhe do curso SHALL incluir, em `presentation.learningPreview.modules`, módulos ordenados com suas aulas ordenadas. Cada aula SHALL expor rótulo e descrição em texto simples; essa coleção demonstrativa SHALL ser idêntica para os seis cursos e distinta de `modules`, que permanece a estrutura relacional específica de Detalhes.

#### Scenario: Consulta do detalhe com aulas do AVA

- **WHEN** o consumidor consulta `GET /api/courses/{id}` para qualquer um dos seis cursos
- **THEN** cada módulo da prévia contém duas aulas com rótulo e descrição
- **AND** a ordem de módulos e aulas é estável
- **AND** a resposta preserva os módulos relacionais específicos do curso no campo superior `modules`

#### Scenario: Conteúdo uniforme entre identificadores

- **WHEN** o consumidor compara `presentation.learningPreview.modules` de dois ou mais cursos
- **THEN** rótulos, descrições, quantidades e ordens dos módulos e aulas da prévia são iguais
- **AND** nenhuma matrícula, conclusão ou progresso de usuário é incluído
