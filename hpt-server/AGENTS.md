# HPT Server — ASP.NET Core Web API

## Architecture

Clean Architecture with strict one-way dependency flow:

```
SharedKernel ← Domain ← App ← Infrastructure ← Api
```

## Layers

| Layer              | Directory             | Purpose                                          |
|--------------------|-----------------------|--------------------------------------------------|
| API                | `HPT.Api/`            | Controllers, middleware, DI composition root      |
| Application        | `HTP.App/`            | Use cases (commands/queries), validators, DTOs    |
| Domain             | `HTP.Domain/`         | Entities, value objects, domain errors            |
| Infrastructure     | `HTP.Infrastructure/` | EF Core, repositories, JWT, identity services     |
| Shared Kernel      | `HTP.SharedKernel/`   | `Result<T>`, `Error`, `IUnitOfWork`               |
| Unit Tests         | `HTP.Tests/`          | xUnit + FluentAssertions                          |
| Integration Tests  | `HTP.IntegrationTests/`| End-to-end API tests                             |

## Entry Points

- DI composition root: `HPT.Api/Program.cs:1-19`
- Auth endpoints: `HPT.Api/Controllers/AuthController.cs`
- User endpoints: `HPT.Api/Controllers/UsersController.cs`
- Error-to-HTTP mapping: `HPT.Api/Infrastructure/CustomResults.cs`

## Commands & Test

```
dotnet build                                  # Build entire solution
dotnet test                                   # Run unit + integration tests
dotnet run --project HPT.Api/HPT.Api.csproj   # Start API server
dotnet ef migrations add <Name> --project HTP.Infrastructure --startup-project HPT.Api
dotnet ef database update --project HTP.Infrastructure --startup-project HPT.Api
```

## DI Registration

- App services registered via `HTP.App/Core/Extensions/DI/_appServiceCollectionExtensions.cs`
- Infrastructure services via `HTP.Infrastructure/Extensions/DI/_infrastructureServiceCollecionExtensions.cs`
- Handlers auto-discovered by Scrutor assembly scanning: `HTP.App/Core/Extensions/DI/MediatorExtenstions.cs:15-38`

## Additional Documentation

- [Architectural patterns](../.agents/docs/architectural_patterns.md) — CQRS, Result pattern, decorators, auth flow
