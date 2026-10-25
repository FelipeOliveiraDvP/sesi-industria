# Design

## Context

See `proposal.md` for motivation. `Course.PresentationJson` already stores an editorial `LearningPreview`; its six module entries currently contain only a label and collapsed state. `CourseQueryService` maps this JSON to a typed detail response, and the frontend guard and `LearningSidebar` consume that shape. Separately, the top-level relational `modules` and `lessons` are course-specific and feed the Details accordion. `CatalogSeedData` builds all six presentation documents through one common constructor, while the seed initializer inserts missing course IDs without overwriting existing rows.

The Details hero currently uses `presentation.detailsImageUrl`; each Landing card uses `imageUrl`. Both values are already returned by the same detail response.

## Goals / Non-Goals

**Goals:**

- Keep one generic AVA module-and-lesson outline in the existing editorial JSONB seed for every course.
- Display that outline from the API, with accessible local expansion and no learner or lesson operations.
- Preserve the distinct relational curricula used on Details and keep the existing AVA article, banner and fixed 10% demonstration.
- Use the course card image for its Details hero.
- Make the same `dotnet ef database update` flow populate fresh databases and update existing seeded presentations.

**Non-Goals:**

- Replacing the course-specific relational modules or lesson descriptions shown in Details.
- Adding lesson routes, a player, progress tracking, enrollment, completion records, or a new endpoint.
- Adding a table or EF entity for the AVA-only outline.

## Decisions

### 1. Store the generic AVA outline inside `LearningPreview`

Extend each `LearningPreview.modules` entry with an ordered `lessons` array. The common seed will contain six modules (`2. Módulo 1` through `7. Módulo 6`) and two generic lessons per module, each with a label and plain-text description. The same factory creates the exact same ordered outline for all six course presentations. `expanded` remains false in the seed; lesson rows carry no state or user identity.

This is separate from the top-level relational `CourseDetail.modules`. Those modules and lessons remain course-specific and continue to feed the Details page. Reusing them in the AVA would replace the generic outline with six different curricula and change Details content, contrary to the requested common seed.

### 2. Update existing JSONB through an incremental data migration

Fresh databases receive the new outline from the regular seed. Existing courses are not overwritten by the seed initializer, so add a SQL-only EF migration that replaces only `learningPreview.modules` for seed IDs 1–6. It must preserve the other JSONB properties and every relational record. Its `Down` restores the previous six-label outline. Applying migrations and seed through `dotnet ef database update` remains the single documented database preparation command; no schema or model snapshot change is needed because `PresentationJson` remains a JSONB string.

### 3. Expose and validate nested lessons in the existing detail contract

Add typed lesson entries below `presentation.learningPreview.modules`. Preserve module and lesson array order. Extend the API runtime validation to reject a missing or malformed lessons array before caching success; do not add a new endpoint or alter the top-level relational `modules` contract. The database and API tests will verify all six presentations share the generic outline and that each course retains its own relational content and article.

### 4. Expand modules locally in the AVA sidebar

Render each API-provided module as an accessible disclosure button with `aria-expanded` and a controlled lesson list. Initialize each module collapsed from the response; toggling is transient component state only. Render lesson labels and descriptions as text, not links or buttons. This provides exploration while preserving the existing boundary: lessons do not navigate or play, and expansion does not change the fixed progress indicator.

### 5. Reuse the card image in Details

Pass `course.imageUrl` to the Details `VideoPreview`, the same value used by the card. Keep `presentation.detailsImageUrl` in the API contract for compatibility with existing consumers, but it is no longer the Details hero's selected source. Treat this repeated image as illustrative, without claiming that it uniquely depicts every course.

## Risks / Trade-offs

- [An existing presentation may have a customized AVA module outline] → The data migration targets only the six seeded course IDs and only the `learningPreview.modules` JSON path; articles, FAQs, and relational curricula remain unchanged.
- [The generic lessons could be mistaken for course-specific content] → Label the AVA as a demonstration and keep lessons read-only, with no completion state or player.
- [Expanded lists increase sidebar height] → Preserve the sidebar's independent scroll and test desktop and mobile widths, including reduced-motion and keyboard use.
- [The common hero image may not depict the selected course] → Reuse the already approved card asset and keep it explicitly illustrative; do not add subject-specific claims to its alternative text.

## Migration Plan

1. Extend the editorial model, DTO, validation and shared seed; test round-trip and identical generic content across the six IDs.
2. Add the JSONB data migration for existing seeded rows and verify both fresh-database setup and an already seeded database through `dotnet ef database update`, without dropping the database or volume.
3. Integrate the nested contract and local expansion into the AVA, and change the Details image source to `imageUrl`.
4. Run backend tests, frontend typecheck/build, and browser checks for image parity, six course IDs, module expansion, keyboard access, mobile layout, and unchanged read-only behavior.

Rollback requires reverting the frontend/API together with the data migration. Apply the migration's `Down` before running an older client that expects AVA modules to contain only labels and expansion flags.
