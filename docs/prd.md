# PRD — Catálogo de cursos educacionais

## Objetivo do produto

A aplicação disponibiliza um catálogo de cursos educacionais para consulta, pesquisa, filtragem e visualização de detalhes.

## Entidades

### Curso

- Identificador único.
- Nome completo.
- Nome breve.
- Descrição.
- Categoria.
- Modalidade.
- Carga horária.
- Público-alvo.
- Status.
- Imagem de apresentação.
- Data de disponibilização, quando aplicável.

Cada curso está associado a uma categoria, uma modalidade e um status. Um curso pode possuir vários módulos.

### Categoria

- Identificador.
- Nome.

### Modalidade

- Identificador.
- Nome.

### Status do curso

- Identificador.
- Nome.

### Módulo

- Identificador.
- Curso associado.
- Título.
- Descrição.
- Ordem de apresentação.

Cada módulo está associado a um curso e pode possuir várias aulas.

### Aula

- Identificador.
- Módulo associado.
- Título.
- Descrição.
- Ordem de apresentação.

Cada aula está associada a um módulo.

## Regras de negócio e requisitos funcionais

- **RN01:** O catálogo deve possuir, no mínimo, seis cursos.
- **RN02:** O catálogo deve possuir, no mínimo, três categorias distintas.
- **RN03:** O catálogo deve possuir, no mínimo, duas modalidades distintas.
- **RN04:** O catálogo deve possuir, no mínimo, três status distintos.
- **RN05:** Todos os dados utilizados no catálogo inicial devem ser fictícios.
- **RN06:** Cada curso deve possuir as informações obrigatórias descritas neste documento.
- **RN07:** A data de disponibilização é opcional.
- **RN08:** Cada curso deve estar associado a uma categoria, modalidade e status válidos.
- **RN09:** Os cursos devem apresentar diversidade de cargas horárias, módulos, descrições de aulas e públicos-alvo.
- **RN10:** A API deve permitir listar cursos.
- **RN11:** A API deve permitir pesquisar cursos pelo nome.
- **RN12:** A pesquisa deve considerar o nome completo e o nome breve.
- **RN13:** A API deve permitir filtrar por categoria, modalidade e status.
- **RN14:** A pesquisa e os filtros devem poder ser combinados.
- **RN15:** Os critérios de pesquisa e filtragem combinados devem ser aplicados cumulativamente.
- **RN16:** A API deve informar a quantidade de resultados da consulta.
- **RN17:** A API deve permitir consultar os detalhes de um curso.
- **RN18:** Consultas sem resultados devem retornar uma coleção vazia e quantidade igual a zero.
- **RN19:** A API não deve disponibilizar operações de cadastro, edição ou exclusão de cursos.

São obrigatórias as informações do curso listadas em Entidades, exceto a data de disponibilização.

## Casos de uso

### 1. Listar cursos

**Finalidade:** consultar os cursos do catálogo.

**Comportamento esperado:** a API retorna a coleção de cursos, suas classificações e a quantidade de resultados.

### 2. Pesquisar cursos pelo nome

**Finalidade:** localizar cursos por seu nome completo ou nome breve.

**Comportamento esperado:** a API considera os dois nomes e retorna os cursos correspondentes à pesquisa e a quantidade encontrada.

### 3. Filtrar cursos por categoria

**Finalidade:** consultar cursos de uma categoria.

**Comportamento esperado:** a API retorna os cursos associados à categoria informada e a quantidade encontrada.

### 4. Filtrar cursos por modalidade

**Finalidade:** consultar cursos de uma modalidade.

**Comportamento esperado:** a API retorna os cursos associados à modalidade informada e a quantidade encontrada.

### 5. Filtrar cursos por status

**Finalidade:** consultar cursos de um status.

**Comportamento esperado:** a API retorna os cursos associados ao status informado e a quantidade encontrada.

### 6. Combinar pesquisa e filtros

**Finalidade:** consultar cursos que atendam simultaneamente aos critérios informados.

**Comportamento esperado:** a API aplica cumulativamente a pesquisa pelo nome e os filtros de categoria, modalidade e status utilizados na consulta. Retorna os cursos que atendem a todos os critérios e a quantidade encontrada.

### 7. Consultar detalhes de um curso

**Finalidade:** consultar as informações de um curso e sua estrutura educacional.

**Comportamento esperado:** a API retorna as informações do curso, suas classificações, módulos e aulas, com os títulos, descrições e ordens de apresentação correspondentes.
