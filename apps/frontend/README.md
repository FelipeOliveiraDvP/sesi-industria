# SESI Indústria — frontend

Single-page React app for the documented SESI Indústria screens. It runs independently of the .NET API, PostgreSQL and Docker.

## Requirements and commands

- Node.js `>=22.13` (the workspace uses Node `24.15.0`).
- pnpm `11.9.0`, pinned in the repository root.

Run these commands from the repository root:

```sh
pnpm install
pnpm --filter @sesi/frontend dev --host 127.0.0.1
pnpm --filter @sesi/frontend typecheck
pnpm --filter @sesi/frontend build
pnpm --filter @sesi/frontend preview --host 127.0.0.1
```

`dev` serves the app locally. `build` runs the strict TypeScript checks before creating `apps/frontend/dist`; `preview` serves that build locally and is not a production server.

## Routes

| Path | Screen |
| --- | --- |
| `/` | Landing |
| `/cursos/matematica-para-a-precisao` | Course details |
| `/ava/matematica-para-a-precisao` | Learning environment preview |

Unknown paths show an accessible not-found page with a link to the Landing. The frontend uses local static content only; it does not require API or database setup.

For static hosting, configure a history fallback that rewrites application routes to `index.html` while leaving missing static assets as 404 responses.

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

The Landing contains the industrial hero, six featured course cards, static impact metrics and benefits, five testimonials, a final call to action and the shared footer. `Cursos` and `Conhecer cursos` scroll to the course rail; the first card and `Matricule-se` open the documented course details route. The course and testimonial rails are local, finite carousels with keyboard and arrow-button controls.

Other course cards, `Todos os cursos`, `Ver todos os cursos`, `Sobre`, `Área do aluno` and footer destinations are unavailable in this demonstration and do not open additional pages. The play mark on the local thumbnail is illustrative and does not play media. Impact values are static reference copy, not live statistics. No catalog, login, video playback or actual enrollment flow is implemented.

## Course details (`/cursos/matematica-para-a-precisao`)

The details screen includes the reference date (`05/01/2026`), SESI certification label, four benefit descriptions, and three initially closed accordions. The accordion sections expand independently and may remain open together. The enrollment bar appears after scrolling past the hero; both `Matricule-se` links open the AVA preview, and `Início` returns to the Landing. The bar reserves room at the end of the page so the footer remains reachable.

The modules, lesson, assessment and FAQ copy is transcribed from the visual reference. Mentions of exams, forums, antifraud measures and certificates are informational copy only; none of those services or operations are implemented. The contact CTA `Mandar uma mensagem` is unavailable and does not open a form or send a message. The date and certification are static display values, not enrollment or credential data.

## Learning environment preview (`/ava/matematica-para-a-precisao`)

The AVA preview presents the local banner and the complete seven-paragraph `Boas práticas` and `Recomendações` article. Its sidebar shows the documented lesson states and six generic, collapsed module labels. Progress is a fixed 10% demonstration value and does not change or persist. The search field and lesson/module items are illustrative and do not filter content, navigate to lessons or record completion. The return arrow always opens the course details route, including after direct entry. This is not an authenticated or functional LMS, and opening it does not enroll the visitor.
