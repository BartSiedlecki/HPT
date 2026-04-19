# CLAUDE.md — HTP.IntegrationTests

## Framework

xUnit 2.9.3 + FluentAssertions 8.9.0 + Testcontainers (PostgreSQL)

## Scope

Tests **application layer commands and queries** against a real PostgreSQL database spun up in Docker via Testcontainers. No HTTP client — commands are dispatched directly through `IDispatcher`. Both `WriteDbContext` and `ReadDbContext` are wired to the container database.

## Infrastructure

- `IntegrationTestWebAppFactory` — extends `WebApplicationFactory<Program>`, starts a `PostgreSqlContainer`, replaces `WriteDbContext`/`ReadDbContext`/`AppIdentityDbContext` with container-backed instances, and runs EF migrations on startup.
- `BaseIntegrationTest` — base class for all test classes. Exposes `Dispatcher`, `WriteDbContext`, `ReadDbContext`, and `GetRequiredService<T>()`. Inherit from it and inject the factory via `IClassFixture<IntegrationTestWebAppFactory>`.

## Run

```bash
dotnet test hpt-server/HTP.IntegrationTests/
```

Docker must be running — Testcontainers starts a `postgres:17` container automatically.

## Conventions

### Naming

- **Namespace**: `HTP.IntegrationTests.{Feature}` (e.g. `HTP.IntegrationTests.Auth`)
- **Class**: `{CommandOrQuery}Tests`
- **Method**: `{Method}_Should{ExpectedBehavior}_When{Condition}`

### Structure

Same `// arrange`, `// act`, `// assert` comments as unit tests (lowercase, always present):

```csharp
[Fact]
public async Task Create_ShouldSuccess_WhenCredentialsAreValid()
{
    // arrange
    var command = CreateValidCommand();

    // act
    var result = await Dispatcher.Send(command);

    // assert
    result.IsSuccess.Should().Be(true);
}
```

### Assertions

Use **FluentAssertions** only — never `Assert.*`.

### Test data

Use `RandomData.UniqueEmail` for unique emails (generates a `Guid`-based address). Add helpers to `Helpers/RandomData.cs` as needed.

Use private `CreateValid{Command}()` factory methods within test classes for reusable valid inputs:

```csharp
private CreateUserByAdminCommand CreateValidCommand(string? email = null)
{
    return new CreateUserByAdminCommand("jan", "kowalski", email ?? RandomData.UniqueEmail, []);
}
```

### Verifying persistence

Resolve repositories or `UserManager<AppIdentityUser>` via `GetRequiredService<T>()` to assert database state after a command:

```csharp
var userRepository = GetRequiredService<IUserRepository>();
var userInDb = await userRepository.GetByEmailAsync(emailVo);
userInDb.Should().NotBeNull();
```
