# HPT — Fullstack Web Application

## Overview

HPT is a fullstack web application with a React frontend and ASP.NET Core backend. The project implements user management, authentication (JWT + refresh tokens), and role-based authorization with a permission system.

## Tech Stack

| Layer    | Stack                                                                 |
|----------|-----------------------------------------------------------------------|
| Client   | React 19, TanStack Start/Router/Query, Tailwind CSS 4, Vite 8, TypeScript |
| Server   | .NET 10, ASP.NET Core Web API, EF Core 10, PostgreSQL                |
| Auth     | JWT Bearer + ASP.NET Core Identity + Refresh Tokens                  |
| Testing  | Client: Vitest + jsdom · Server: xUnit + FluentAssertions            |

## Key Directories

```
hpt-client/          # React frontend (TanStack Start)
  src/routes/        # File-based routing definitions
  src/components/    # Reusable UI components (shadcn/ui)
  src/integrations/  # TanStack Query setup & devtools

hpt-server/          # ASP.NET Core backend (Clean Architecture)
  HPT.Api/           # Controllers, middleware, DI composition
  HTP.App/           # Commands, queries, handlers, validators
  HTP.Domain/        # Entities, value objects, domain errors
  HTP.Infrastructure/# EF Core, repositories, JWT, identity
  HTP.SharedKernel/  # Result<T>, Error, shared abstractions
  HTP.Tests/         # Unit tests (xUnit)
  HTP.IntegrationTests/ # Integration tests
```

## Build & Test Commands

**Client** (`hpt-client/`):
```
npm install            # Install dependencies
npm run dev            # Dev server (port 3000)
npm run build          # Production build
npm test               # Run Vitest
npm run check          # Prettier + ESLint fix
```

**Server** (`hpt-server/`):
```
dotnet build                                  # Build solution
dotnet test                                   # Run all tests
dotnet run --project HPT.Api/HPT.Api.csproj   # Run API
dotnet ef database update                     # Apply migrations
```

## Additional Documentation

- [Server details](hpt-server/AGENTS.md) — Clean Architecture layers, entry points, DI setup
- [Client details](hpt-client/AGENTS.md) — routing, components, data fetching
- [Architectural patterns](.agents/docs/architectural_patterns.md) — CQRS, Result pattern, decorators, auth flow, DI conventions
