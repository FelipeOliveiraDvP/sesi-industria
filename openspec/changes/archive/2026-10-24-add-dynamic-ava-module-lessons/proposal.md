# Proposal

## Why

The course details hero currently uses a separate image path from the catalog card, so the same course can appear with different imagery across the navigation flow. The AVA sidebar also shows generic module labels without lessons, preventing learners from exploring the seeded structure consistently across course IDs.

## What Changes

- Make the Details hero use the same API `imageUrl` as its corresponding Landing card.
- Extend the AVA presentation with a shared, deterministic set of generic modules and lessons for all six courses, seeded as part of the existing catalog initialization command.
- Expose nested AVA lessons through the existing typed course-detail response and render them in expandable, read-only sidebar modules.
- Preserve the course-specific relational modules and lessons used by Details; the generic AVA structure is a separate demonstration payload.
- Add an incremental data migration that updates only the AVA module section of existing seeded presentations, so an already populated local database receives the new lessons through the normal migration update.
- Keep lesson rows non-navigable and demonstration progress fixed; add no player, enrollment, completion, or learner-progress behavior.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `catalog-persistence`: persist one deterministic generic AVA module-and-lesson structure for each seeded course, separate from relational course curricula.
- `course-catalog-query`: expose nested lesson labels and descriptions in the typed AVA presentation.
- `course-details-page`: use the same course image URL for the Landing card and Details hero.
- `learning-environment-preview`: render API-provided AVA modules with expandable, read-only lessons for every course.

## Impact

- Backend: editorial JSONB types, deterministic seed, serializer validation, typed DTO mapping, migration to update existing seeded JSONB, and tests.
- Frontend: Details hero image source, AVA response types and guards, expandable sidebar rendering, and browser verification.
- Documentation and OpenSpec specs: describe image consistency, the generic AVA outline, one-command database preparation, and the non-interactive lesson boundary.
