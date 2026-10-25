# Tasks

## 1. AVA editorial data and API contract

- [x] 1.1 Extend the editorial model and typed DTO so each AVA module contains ordered lesson labels and descriptions; validate six modules with two plain-text lessons each and verify JSON round-trip and rejection of missing lesson content.
- [x] 1.2 Add one deterministic generic six-module/twelve-lesson outline to the shared presentation seed for all six courses; verify identical AVA module and lesson content while preserving distinct relational Details modules, course articles, recommendations, and the fixed demonstration progress.
- [x] 1.3 Map nested lessons through `CourseQueryService` and update API contract/OpenAPI tests; verify all six detail responses expose the nested arrays in order without changing the top-level relational module and lesson DTOs or adding endpoints.

## 2. Existing presentation data

- [x] 2.1 Add and review an incremental data migration that writes the generic AVA outline only to `learningPreview.modules` for the six seeded course IDs; verify fresh and previously seeded PostgreSQL databases, preserved presentation fields and relational curricula, rollback behavior, and repeatable `dotnet ef database update` without dropping a database or volume.

## 3. Frontend integration

- [x] 3.1 Extend frontend presentation types and runtime guards for module lessons; verify valid nested data is accepted and malformed or missing lesson arrays are rejected before entering the query cache.
- [x] 3.2 Render modules and lessons from the AVA detail response as accessible, locally expandable modules; verify keyboard operation, nested lesson text, responsive sidebar access, and that lesson rows do not navigate, play media, or alter the fixed 10% progress.
- [x] 3.3 Use the selected course's `imageUrl` for the Details hero; verify its rendered image URL matches the same course's Landing card image while preserving image proportions and the non-playing illustration.
- [x] 3.4 Update backend and frontend READMEs to document the shared generic AVA outline, local expansion, non-navigable lessons, fixed progress, and shared Details/card image; verify the docs do not imply a functional LMS or learner progress.

## 4. Integrated verification

- [x] 4.1 From `apps/backend`, restore tools and dependencies, apply the new migration to the confirmed local database, repeat the update, and run `dotnet build`/`dotnet test`; verify the database retains six courses and each AVA presentation has the same nested lesson seed.
- [x] 4.2 Run frontend typecheck/build and browser acceptance for all six courses in development and preview; verify matching card/Details images, module expansion and lessons on direct AVA entry, CORS, keyboard access, mobile layout, no overflow, no writes, and no course identity mixing.
