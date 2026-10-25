# Spec Delta

## MODIFIED Requirements

### Requirement: Apresentação visual do curso documentado

A página de Detalhes SHALL apresentar o curso Matemática para a Precisão: Medidas e Escalas na Metalurgia com badge Matemática, início em `05/01/2026`, certificação SESI, CTA Matricule-se e thumbnail local. Header, hero em duas colunas, benefícios, informações, depoimentos, contato e footer SHALL seguir a ordem da referência.

#### Scenario: Entrada direta na página do curso

- **WHEN** o usuário abre `/cursos/matematica-para-a-precisao`
- **THEN** encontra o título integral, a data estática da referência, a certificação e o CTA de matrícula
- **AND** visualiza a mesma imagem de curso usada pelo card da Landing, recebida em `imageUrl`, na proporção documentada
- **AND** o play permanece apenas ilustrativo, sem reproduzir vídeo externo
- **AND** consegue percorrer todas as seções da página até o footer
