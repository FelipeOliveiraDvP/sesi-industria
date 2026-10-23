# Design

## Context

`Course.Description` já é obrigatório no modelo, mapeado para a coluna `Courses.Description` da migration inicial e incluído em `CourseDetailDto`. A listagem não o projeta em `CourseSummaryDto`. O seed atual cria seis cursos e as classificações, mas `CatalogSeed` insere somente IDs ausentes; um teste de integração confirma que uma execução repetida preserva alterações já gravadas. A documentação da engenharia reversa traz seis nomes e descrições breves e detalha os módulos do primeiro curso; cargas horárias, públicos-alvo e conteúdo dos outros cursos precisam ser fictícios.

## Goals / Non-Goals

**Goals:**

- Entregar a mesma descrição persistida na listagem e nos detalhes.
- Manter um único catálogo inicial estável para banco vazio e para bancos que receberam o seed anterior.
- Preservar dados fora dos IDs pertencentes ao seed anterior.

**Non-Goals:**

- Adicionar outra coluna de descrição ou alterar o endpoint de detalhes.
- Mudar pesquisa, filtros, operações de escrita da API ou a política de inicialização HTTP.
- Reproduzir informações não disponíveis na engenharia reversa como se fossem fatos; cargas horárias, públicos-alvo e detalhes não documentados serão dados fictícios.

## Decisions

1. **Projetar a descrição existente na listagem.** Acrescentar `Description` ao DTO de resumo e à projeção LINQ da consulta. A entidade, a configuração do EF Core, a migration inicial e o DTO de detalhes já cobrem o campo; uma alteração estrutural de banco não agrega valor. Alternativa descartada: criar outra coluna ou duplicar a descrição no modelo.

2. **Manter os dados transcritos e identificar as extensões fictícias.** Usar os seis nomes e as descrições breves de `docs/figma-reverse-engineering/pages/001-landing-page.md`, e os dez títulos de módulos descritos em `pages/002-detalhes-curso.md` para Matemática para a Precisão. Criar dados fictícios estáveis para cargas horárias, públicos-alvo, modalidades, status e para os outros cursos. A fonte mostra somente as categorias Matemática e Ciências da Natureza; a terceira categoria será `Engenharia de Materiais`, associada ao curso Ciência dos Materiais na Metalurgia. Os status existentes (`Disponível`, `Em breve`, `Encerrado`) e modalidades (`Online`, `Presencial`) podem manter seus IDs.

3. **Atualizar dados existentes por migration, sem tornar o seed um mecanismo de sobrescrita.** Criar uma migration de dados posterior à inicial que atualize as classificações e cursos conhecidos do seed e substitua módulos/aulas associados aos IDs reservados ao seed. O comportamento `AddMissing` do seed permanece: novas instalações recebem o catálogo atualizado após as migrations, enquanto uma repetição comum continua sem sobrescrever alterações posteriores. A migration altera apenas o conjunto de IDs do seed; dados adicionais não relacionados ficam intactos. Alternativa descartada: transformar `CatalogSeed` em upsert recorrente, pois isso quebraria a garantia existente de preservar dados já gravados.

4. **Manter reversão limitada ao catálogo inicial.** A operação `Down` restaura os valores e a estrutura anteriores do seed para os IDs afetados; não tenta reverter alterações feitas em registros fora desse conjunto. A migration deve respeitar a ordem das chaves estrangeiras ao substituir módulos e aulas.

## Risks / Trade-offs

- **A migration substituirá alterações manuais feitas nos IDs reservados ao seed.** Isso é intencional para atualizar o catálogo inicial; limitar a migração aos IDs conhecidos e testar registros adicionais preservados reduz o risco de alcançar dados não pertencentes ao seed.
- **A descrição passa a aparecer em todos os itens da listagem.** É uma ampliação aditiva do JSON; atualizar contrato, testes e documentação da API torna a mudança explícita para consumidores.
- **Parte do conteúdo do seed não vem da fonte visual.** Usar valores estáticos fictícios, sem nomes de pessoas ou dados aleatórios, mantém a reprodução determinística e evita atribuir à engenharia reversa dados que ela não documenta.

## Migration Plan

1. Atualizar DTO de resumo, projeção da listagem, dados fictícios e testes; nenhuma migration de esquema é necessária para `Description`.
2. Criar e revisar a migration de dados após `InitialCreate`, incluindo a atualização dos registros de seed existentes e a restauração correspondente em `Down`.
3. Aplicar `dotnet ef database update` conforme o README do backend. Em banco vazio, as migrations precedem a inserção do novo seed; em banco existente, a migration atualiza o conjunto conhecido e o seed mantém seu comportamento de inserir apenas IDs ausentes.
4. Validar com banco PostgreSQL de integração: conteúdo final, relações, contagens, repetição da atualização, preservação de registro adicional e reversão dos dados do seed.

## Open Questions

Nenhuma.
