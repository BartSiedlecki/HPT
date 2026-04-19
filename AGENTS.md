# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

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
npm test               # Run all Vitest tests
npm test -- src/foo.test.ts          # Run a single test file
npm run check          # Prettier + ESLint fix
```

**Server** (`hpt-server/`):
```
dotnet build                                  # Build solution
dotnet test                                   # Run all tests
dotnet test --filter "FullyQualifiedName~UserTests"  # Run a single test class
dotnet run --project HPT.Api/HPT.Api.csproj   # Run API
dotnet ef migrations add <Name> --project HTP.Infrastructure --startup-project HPT.Api
dotnet ef database update --project HTP.Infrastructure --startup-project HPT.Api
```

## Architecture

### CQRS via custom mediator

Commands and queries are dispatched through a custom `IDispatcher` that resolves handlers at runtime via `IServiceProvider`. Handlers are auto-discovered by Scrutor assembly scanning — no manual registration needed. Each use case lives in `HTP.App/{Feature}/` as a `Command`/`Query` + `Handler` + optional `Validator` trio.

### Decorator pipeline

Every handler is automatically wrapped (outermost → innermost): `LoggingDecorator` → `ValidationDecorator` → handler. The `ValidationDecorator` runs FluentValidation and short-circuits with a `Result.Failure` before the handler executes.

### Result pattern

All handlers return `Result` or `Result<T>` — never throw for business errors. Errors are typed (`NotFound`, `Conflict`, `Validation`, `Unauthorized`, `Forbidden`, etc.) and mapped to HTTP status codes in `HPT.Api/Infrastructure/CustomResults.cs`. Use `ResultExtensions.Match()` in controllers to build responses.

### JWT + Refresh tokens

Access tokens carry user ID, email, roles, and permissions as claims. Refresh tokens are domain entities supporting rotation via `RefreshToken.Update()`. Both are issued together by `JwtTokenIssuer`.

## Additional Documentation

- [Server details](hpt-server/CLAUDE.md) — Clean Architecture layers, entry points, DI setup
- [Client details](hpt-client/CLAUDE.md) — routing, components, data fetching
- [Architectural patterns](.agents/docs/architectural_patterns.md) — full pattern details with file references
