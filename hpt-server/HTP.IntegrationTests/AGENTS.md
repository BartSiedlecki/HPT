# HTP.IntegrationTests — Integration Tests

## Framework

xUnit 2.9.3 + FluentAssertions 8.9.0 + Testcontainers.PostgreSql 4.11.0 + Microsoft.AspNetCore.Mvc.Testing

## Scope

Tests the full command/query pipeline through real infrastructure — dispatching commands, hitting a real PostgreSQL database, and verifying persistence.

## Run

```
dotnet test hpt-server/HTP.IntegrationTests/
```

Requires **Docker** running (Testcontainers spins up a PostgreSQL 17 container).

## Structure

```
HTP.IntegrationTests/
  Auth/                         # Auth feature tests
  Helpers/
    RandomData.cs               # Test data generators
  BaseIntegrationTest.cs        # Abstract base class for all tests
  IntegrationTestWebAppFactory.cs  # WebApplicationFactory + Testcontainers setup
```

- **Namespace**: `HTP.IntegrationTests.{Feature}` (e.g., `HTP.IntegrationTests.Auth`)
- **Directory**: Organized by feature/domain area
- **Class naming**: `{CommandOrFeature}Tests` (e.g., `CreateUserByAdminCommandTests`)

## Test infrastructure

### Base class

All integration test classes must extend `BaseIntegrationTest`:

```csharp
public class MyCommandTests : BaseIntegrationTest
{
    public MyCommandTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }
}
```

`BaseIntegrationTest` provides:
- `Dispatcher` — sends commands/queries through the full pipeline
- `WriteDbContext` / `ReadDbContext` — direct database access
- `GetRequiredService<T>()` — resolve any registered service

### WebApplicationFactory

`IntegrationTestWebAppFactory` implements `IClassFixture` and `IAsyncLifetime`:
- Starts a PostgreSQL 17 container via Testcontainers
- Replaces `WriteDbContext`, `ReadDbContext`, `AppIdentityDbContext` with test container connection
- Auto-runs EF Core migrations on startup

## Conventions

### Method naming

Same as unit tests:

```
{Method}_Should{ExpectedBehavior}_When{Condition}
```

Examples:
- `Create_ShouldSuccess_WhenCredentialsAreValid`
- `Create_ShouldFail_WhenEmailAlreadyExists`
- `Create_ShouldPersistDomainUser_InDatabase`

### Arrange-Act-Assert

Every test uses `// arrange`, `// act`, `// assert` comments (lowercase):

```csharp
[Fact]
public async Task Create_ShouldFail_WhenEmailAlreadyExists()
{
    // arrange
    var email = RandomData.UniqueEmail;
    CreateUserByAdminCommand command = CreateValidCommand(email);
    CreateUserByAdminCommand duplicatedEmailCommand = CreateValidCommand(email);

    // act
    var result = await Dispatcher.Send(command);
    var secondResult = await Dispatcher.Send(duplicatedEmailCommand);

    // assert
    result.IsSuccess.Should().Be(true);
    secondResult.IsSuccess.Should().Be(false);
    secondResult.Error.Should().Be(UserApplicationErrors.EmailAlreadyExists);
}
```

### Async

All integration tests must be `async Task` (not `void`).

### Test attributes

- `[Fact]` — single test case
- `[Theory]` + `[InlineData]` — parametrized test cases

### Assertions

Use **FluentAssertions** for all new tests. Do not use xUnit `Assert.*`.

### Dispatching commands

Execute commands through the full pipeline via `Dispatcher.Send()`:

```csharp
var result = await Dispatcher.Send(command);
```

### Database verification

After a command succeeds, verify persistence by resolving repositories or services:

```csharp
var userRepository = GetRequiredService<IUserRepository>();
var userInDb = await userRepository.GetByEmailAsync(emailVo);
userInDb.Should().NotBeNull();
```

### Error assertions

Verify specific error types from domain or application error constants:

```csharp
result.Error.Should().Be(FirstNameErrors.InvalidFormat);
result.Error.Should().Be(UserApplicationErrors.EmailAlreadyExists);
```

### Test data

- **Unique emails**: Use `RandomData.UniqueEmail` (generates `jan.{guid}@mail.com`)
- **Valid commands**: Private `CreateValidCommand()` factory methods with optional parameter overrides:

```csharp
private CreateUserByAdminCommand CreateValidCommand(string? email = null)
{
    return new CreateUserByAdminCommand(
        "jan",
        "kowalski",
        email ?? RandomData.UniqueEmail,
        []
    );
}
```

### No mocking

Integration tests use real infrastructure. No mocking — services are resolved from the DI container backed by a real PostgreSQL container.
