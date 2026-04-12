# HPT Client — React + TanStack Start

## Overview

Single-page application built with React 19 and TanStack Start framework. Uses file-based routing, TanStack Query for server state, and shadcn/ui components styled with Tailwind CSS 4.

## Directory Structure

| Directory            | Purpose                                      |
|----------------------|----------------------------------------------|
| `src/routes/`        | File-based route definitions (TanStack Router)|
| `src/components/`    | Reusable UI components (Header, Footer, ThemeToggle) |
| `src/integrations/`  | TanStack Query client setup & devtools        |
| `src/lib/`           | Utility functions                             |
| `public/`            | Static assets                                 |

## Commands

```
npm install       # Install dependencies
npm run dev       # Dev server (port 3000)
npm run build     # Production build
npm run preview   # Preview production build
npm test          # Run Vitest with jsdom
npm run lint      # ESLint check
npm run check     # Prettier format + ESLint fix
```

## Key Config Files

- `vite.config.ts` — Build configuration
- `app.config.ts` — TanStack Start app config
- `tsconfig.json` — TypeScript settings
- `src/styles.css` — Tailwind CSS entry point
