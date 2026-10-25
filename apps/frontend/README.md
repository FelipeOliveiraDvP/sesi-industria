# SESI Indústria — frontend

Single-page React app for the documented SESI Indústria screens. The development server, typecheck and build run independently of the .NET API, PostgreSQL and Docker, but course data on Landing, Details and AVA is loaded from the configured API.

## Requirements and commands

- Node.js `>=22.13` (the workspace uses Node `24.15.0`).
- pnpm `11.9.0`, pinned in the repository root.

Run these commands from the repository root:

```sh
[ -f apps/frontend/.env.local ] || cp apps/frontend/.env.example apps/frontend/.env.local
pnpm install
pnpm --filter @sesi/frontend dev --host 127.0.0.1 --port 5173 --strictPort
pnpm --filter @sesi/frontend typecheck
pnpm --filter @sesi/frontend build
pnpm --filter @sesi/frontend preview --host 127.0.0.1 --port 4173 --strictPort
```

`dev` serves the app locally. `build` runs the strict TypeScript checks before creating `apps/frontend/dist`; `preview` serves that build locally and is not a production server.

## Production Docker image

Build from the repository root. The Dockerfile installs the root pnpm workspace with the frozen lockfile, builds the frontend, and serves only `dist` using Nginx:

```sh
docker build --file apps/frontend/Dockerfile --tag catalog-frontend:local \
  --build-arg VITE_API_BASE_URL=https://api.example .
docker run --rm --publish 8080:8080 --env PORT=8080 catalog-frontend:local
```

Replace `https://api.example` with the public API origin used by the target deployment. The build requires a valid `VITE_API_BASE_URL`; missing values and values containing credentials, a subpath, query, or fragment fail the build. This `VITE_` value is public and compiled into the browser bundle, not a secret or runtime setting. Rebuild the image when the API origin changes; changing `VITE_API_BASE_URL` only when starting the Nginx container does not change the compiled bundle. `PORT` is a runtime setting and defaults to `8080`. Nginx serves the SPA fallback while keeping missing static assets as 404 responses. Use `vite preview` only for local preview, not production. See the [Railway deployment guide](../../docs/deploy-railway.md) for service configuration.

## API configuration and cache

The frontend reads `VITE_API_BASE_URL`, an absolute HTTP or HTTPS API origin. The local default is `http://localhost:5080`; `apps/frontend/.env.example` shows the development value. For local customization, copy it to `apps/frontend/.env.local` (ignored by Git), or set the variable in the shell before starting Vite/building. Vite loads environment files at startup. `VITE_` values are public and embedded in the client bundle: never put credentials, tokens or other secrets there.

The API client uses a 10-second timeout. Course queries use an in-memory cache that considers data fresh for 60 seconds and retains unused entries for five minutes; it is not persisted to browser storage. Details and AVA share the same cached detail for a course ID. A new build is required when changing the API URL used by preview or a deployed static bundle; starting preview does not replace the value already compiled into the files.

`FRONTEND_URL` selects the frontend host used in image URLs: use `http://127.0.0.1:5173` for development and `http://127.0.0.1:4173` for preview, and restart the API when switching. Catalog GET requests accept any browser origin without credentials; this does not change the configured API base (`VITE_API_BASE_URL`).

## Routes

| Path | Screen / data request |
| --- | --- |
| `/` | Landing; `GET /api/courses` |
| `/cursos/:courseId` | Details for a positive integer ID; `GET /api/courses/{id}` |
| `/ava/:courseId` | AVA preview for the same ID/detail response |
| `/cursos/matematica-para-a-precisao` | Legacy alias; replaces history with `/cursos/1` |
| `/ava/matematica-para-a-precisao` | Legacy alias; replaces history with `/ava/1` |

Invalid IDs and unknown paths show an accessible not-found page. A well-formed ID missing from the API shows a course-not-found state; direct entry and refresh do not depend on visiting the Landing first. All course cards use the ID from the list response, preserve its order, and do not request one detail per card. The application uses only the two catalog GET endpoints; it adds no search/filter UI or catalog page, and does not enroll users, navigate lessons, save progress, or call write endpoints.

Static institutional sections and assets remain available without the backend; course-specific content requires the API and does not silently fall back to local fixtures. The course and AVA preview share the same detail cache key for the selected ID.

For static hosting, configure a history fallback that rewrites application routes to `index.html` while leaving missing static assets as 404 responses.

## Loading, errors and recovery

On the first request, Landing, Details and AVA show Mantine skeletons that reserve the page layout; they do not expose placeholder course titles or enrollment actions. The loading state is announced to assistive technology, the skeleton shapes are decorative, and non-essential shimmer is disabled when reduced motion is requested. Institutional content and local assets remain available when the API is offline, but course data does not fall back to fixtures, so this is not a fully offline catalog.

The API's normal outcomes remain distinct: an empty collection reports that no courses are available, and a `404` detail reports that the course was not found. Network, timeout and server (`5xx`) failures receive at most one automatic retry, followed by an explicit `Tentar novamente` action; incompatible API responses are also recoverable but are not retried automatically. A failed background refresh keeps the valid data for that same course visible and offers a retry rather than replacing the page with skeletons. A course ID is never filled with another course's data.

Rendering exceptions are a separate failure class from HTTP or network errors. The React Error Boundary shows a generic recovery/return state for an unexpected rendering failure; the query error states handle API responses and connectivity. Neither state exposes stack traces or raw server details.

## Visual references and local assets

The frontend preserves the 65 supplied files under `public/assets`: 3 PNGs, 22 SVGs, 38 TTFs and 2 text files. The Open Sans variable regular and italic faces are loaded locally with `font-display: swap`; `OFL.txt`, the font README and the unused static font variants remain in place. Open Sans is the supplied local choice, not a confirmed identification of the original Figma typeface.

| Use | Local asset |
| --- | --- |
| Landing industrial hero | `hero-image.png` |
| CTA and course preview | `video-thumb.png` |
| AVA banner | `banner-ava.png` |
| SESI Indústria brand | `logo-sesi-industria-primary.svg`, `logo-sesi-industria-white.svg` |
| Course categories, benefits, carousel, accordion and social icons | Supplied SVGs centralized in `src/app/assets.ts` |

Individual course-card photos were not supplied. As approved for this demonstration, cards reuse crops of the existing local images: cards 1 and 6 use `banner-ava.png`, card 2 uses `hero-image.png`, and cards 3–5 use `video-thumb.png`. This is an accepted visual difference, not a claim that the photos match the reference subjects.

The page copy and observed interactions are transcribed in `docs/figma-reverse-engineering/`. Hex colors were sampled from screenshots and measurements, typography identification and spacing are estimates rather than native Figma tokens. The available Figma file could not be read through the official integration. The source references document desktop only; tablet/mobile layouts are implementation adaptations and need separate visual review.

## Landing (`/`)

The Landing contains the industrial hero, the API-ordered course cards, static impact metrics and benefits, five testimonials, a final call to action and the shared footer. `Cursos` and `Conhecer cursos` scroll to the course rail; each card opens its own ID route, while the lower `Matricule-se` reference CTA opens course 1. The course and testimonial rails are local, finite carousels with keyboard and arrow-button controls.

Other course cards, `Todos os cursos`, `Ver todos os cursos`, `Sobre`, `Área do aluno` and footer destinations are unavailable in this demonstration and do not open additional pages. The play mark on the local thumbnail is illustrative and does not play media. Impact values are static reference copy, not live statistics. No catalog, login, video playback or actual enrollment flow is implemented.

## Course details (`/cursos/:courseId`)

The details screen uses the selected API detail for title, category, date, certification, image, full description, relational modules/lessons, lesson/assessment copy and FAQs. Dates are formatted from calendar values without timezone conversion; a null date appears as `A definir`. The four institutional benefits, testimonials, contact section and footer remain shared local content. Accordions expand independently; the enrollment bar appears after scrolling past the hero, and both `Matricule-se` links open the AVA for the same ID.

The modules, lesson, assessment and FAQ copy is transcribed from the visual reference. Mentions of exams, forums, antifraud measures and certificates are informational copy only; none of those services or operations are implemented. The contact CTA `Mandar uma mensagem` is unavailable and does not open a form or send a message. The date and certification are static display values, not enrollment or credential data.

## Learning environment preview (`/ava/:courseId`)

The AVA preview uses the same course detail as Details: the API provides its banner, sidebar identity, generic module-and-lesson outline, article, and recommendations. All six courses receive the same six generic modules with two lesson descriptions each; this AVA outline is separate from the course-specific relational curriculum shown in Details. Modules expand locally to show their lessons, which remain read-only text and do not navigate or record completion. Progress is fixed at 10%. Desktop and mobile return controls open `/cursos/{sameId}`, including after direct entry. This is not an authenticated or functional LMS, and opening it does not enroll the visitor.

While its data is loading or unavailable, the AVA keeps a route-specific return to `/cursos/{sameId}` available. The displayed 10% is fixed editorial demonstration data only: it is not a learner's progress, is not updated by reading, and is never written to server or browser storage.
