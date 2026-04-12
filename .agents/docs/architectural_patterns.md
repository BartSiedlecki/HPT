# Architectural Patterns

## 1. CQRS / Mediator Pattern

Commands and queries are dispatched through `IDispatcher` (`HTP.App/Core/Abstractions/Mediator/IDispatcher.cs:5-10`) which resolves handlers at runtime via `IServiceProvider`.

- **Commands**: Implement `ICommand` or `ICommand<TResponse>` (`HTP.App/Core/Abstractions/Mediator/ICommand.cs:3-4`)
- **Queries**: Implement `IQuery<TResponse>` (`HTP.App/Core/Abstractions/Mediator/IQuery.cs:3`)
- **Handlers**: Implement `ICommandHandler<T>` / `ICommandHandler<T,R>` / `IQueryHandler<T,R>` (`HTP.App/Core/Abstractions/Mediator/ICommandHandler.cs:5-15`, `IQueryHandler.cs:5-9`)
- **Runtime dispatch**: `HTP.Infrastructure/Services/Mediator/Dispatcher.cs:7-29` — uses reflection to resolve and invoke handlers

Example: `HTP.App/Auth/Login/LoginUserCommand.cs:5-7` → handled by `LoginUserCommandHandler.cs:13-54`.

## 2. Decorator Pipeline (Validation + Logging)

Handlers are wrapped by decorators registered via Scrutor's `.Decorate<>()`:

1. **ValidationDecorator** (`HTP.App/Core/Behaviors/ValidationDecorator.cs:9-43`) — runs FluentValidation before the handler; returns `Result` with `ValidationError` on failure
2. **LoggingDecorators** (`HTP.App/Core/Behaviors/LoggingDecorators.cs:9-118`) — logs processing start/completion with success/error context

Registration order at `HTP.App/Core/Extensions/DI/MediatorExtenstions.cs:41-46`: validation wraps the handler first, then logging wraps validation.

## 3. Result Pattern (Railway-Oriented Programming)

All commands/queries return `Result` or `Result<T>` (`HTP.SharedKernel/Result.cs:5-61`).

- Factory methods: `Result.Success()`, `Result.Failure(error)`, implicit conversion from value
- **Error** (`HTP.SharedKernel/Error.cs:5-63`): typed errors — `NotFound`, `Validation`, `Conflict`, `Unauthorized`, `Forbidden`, `ToManyRequests`, `Internal`
- **ValidationError** (`HTP.SharedKernel/ValidationError.cs:3-30`): groups field-level errors with camelCase key conversion
- **API mapping**: `HPT.Api/Extensions/ResultExtensions.cs:7-22` provides `Match()` for railway-style HTTP response building
- **HTTP status mapping**: `HPT.Api/Infrastructure/CustomResults.cs:77-87` maps error types to status codes (400, 401, 403, 404, 409, 429, 500)

## 4. Repository Pattern

Repositories are split into read and write interfaces defined in the App layer and implemented in Infrastructure:

- Abstraction: `HTP.App/Abstractions/Repositories/Read/IUserRepository.cs:6-15`
- Implementation: `HTP.Infrastructure/Persistence/Repositories/UserRepository.cs:12-34`
- Registration: `HTP.Infrastructure/Extensions/DI/Services/RepositoriesServiceCollectionExtensions.cs:32-37`
- **Unit of Work**: `HTP.App/Core/Abstractions/IUnitOfWork.cs:4-7` → `HTP.Infrastructure/Persistence/UnitOfWork.cs:8-14`

## 5. Clean Architecture Dependency Flow

Strict one-way references enforced via project references:

```
SharedKernel (no deps) ← Domain ← App ← Infrastructure ← Api
```

- `HTP.Domain/HTP.Domain.csproj:10-11` references SharedKernel
- `HTP.App/HTP.App.csproj:19-20` references Domain + SharedKernel
- `HTP.Infrastructure/HTP.Infrastructure.csproj:25` references App
- `HPT.Api/HPT.Api.csproj:20-21` references App + Infrastructure

## 6. JWT + Refresh Token Authentication

- **Token issuer abstraction**: `HTP.App/Abstractions/Authentication/ITokenIssuer.cs:6-12`
- **JWT generation**: `HTP.Infrastructure/Authentication/JwtTokenGenerator.cs:20-53` — HMAC-SHA256, claims include user ID, email, roles, permissions
- **Token orchestration**: `HTP.Infrastructure/Identity/JwtTokenIssuer.cs:11-24` — creates both access + refresh tokens
- **Refresh token entity**: `HTP.Domain/Entities/RefreshTokens/RefreshToken.cs:3-26` — supports token rotation via `Update()`
- **Refresh token service**: `HTP.Infrastructure/Identity/RefreshTokenService.cs:17-28`
- **Identity config**: `HTP.Infrastructure/Extensions/DI/Services/IdentityServiceCollectionExtensions.cs:16-28` — ASP.NET Identity with password policy

## 7. DI Registration with Scrutor

All handler and validator registrations use Scrutor assembly scanning (`HTP.App/Core/Extensions/DI/MediatorExtenstions.cs:15-38`):

- Scans `HTP.App` assembly for `ICommandHandler<>`, `ICommandHandler<,>`, `IQueryHandler<,>`, `IValidator<>` implementations
- Registers as scoped services with `AsImplementedInterfaces()`
- Decorators applied after scanning (lines 41-46)

Composition root: `HPT.Api/Program.cs:18-19` calls `AddAppServices()` and `AddInfrastructureServices()`.
