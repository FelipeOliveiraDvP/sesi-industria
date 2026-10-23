# Tasks

## 1. Descrição na listagem

- [ ] 1.1 Incluir `Description` no DTO de resumo e na projeção de `GET /api/courses`; atualizar os testes de serialização e HTTP para confirmar que a listagem retorna a mesma descrição dos detalhes. Verificar com `dotnet test --filter "FullyQualifiedName~ContractTests"` e `dotnet test --filter "FullyQualifiedName~CourseSearchHttpTests"` em `apps/backend`, com Docker disponível para os testes HTTP.
- [ ] 1.2 Atualizar a documentação de `GET /api/courses` em `apps/backend/README.md` para listar `description` no resumo e esclarecer que o campo de detalhes permanece. Verificar se a tabela e os exemplos descrevem o contrato JSON atualizado.

## 2. Catálogo inicial fictício

- [ ] 2.1 Atualizar `CatalogSeedData` com os seis nomes e descrições dos cards transcritos, exatamente três categorias, duas modalidades e três status; completar cargas horárias, públicos-alvo, módulos e aulas com dados estáticos fictícios variados. Usar os dez módulos documentados para Matemática para a Precisão e definir IDs determinísticos. Verificar com `dotnet test --filter "FullyQualifiedName~SeedDataTests"` em `apps/backend`.
- [ ] 2.2 Ajustar os testes de seed para validar as quantidades exatas, os seis pares de nome/descrição, classificações válidas, diversidade, ordem, relações e reprodução determinística dos dados persistidos. Verificar com `dotnet test --filter "FullyQualifiedName~SeedIntegrityTests"` em `apps/backend` com Docker disponível.

## 3. Atualização de bancos existentes

- [ ] 3.1 Criar uma migration de dados reversível posterior a `InitialCreate` que atualize classificações, cursos, módulos e aulas do conjunto de IDs do seed anterior, sem adicionar coluna para `Description` nem alterar registros fora desse conjunto. Verificar a migration gerada e o SQL com `dotnet ef migrations script --project src/Catalog.Api --startup-project src/Catalog.Api` em `apps/backend`.
- [ ] 3.2 Adicionar testes PostgreSQL que iniciem com os dados do seed anterior, apliquem a migration, confirmem o novo catálogo e a preservação de registros adicionais, e validem a reversão. Verificar com `dotnet test --filter "FullyQualifiedName~SeedDataMigrationTests"` em `apps/backend` com Docker disponível.
- [ ] 3.3 Documentar no README do backend que a atualização versionada atualiza os IDs gerenciados pelo seed e mantém intactos registros fora dele. Verificar as instruções de migration contra o comando `dotnet ef database update` já documentado.

## 4. Verificação integrada

- [ ] 4.1 Em `apps/backend`, executar `dotnet tool restore`, `dotnet restore`, `dotnet build` e `dotnet test`; confirmar que testes unitários e de integração passam e que a API mantém somente os endpoints de leitura.
