# HTP.Tests — Unit Tests

## Framework

xUnit 2.9.3 + FluentAssertions 8.9.0

## Scope

Tests only **Domain** and **SharedKernel** layers — no infrastructure, no database, no DI container. Unit tests operate on pure domain objects directly.

## Run

```
dotnet test hpt-server/HTP.Tests/
```

## Structure

Directory layout mirrors the domain structure:

```
HTP.Tests/
  Domain/
    ValueObjects/      # Value object validation tests
```

- **Namespace**: `HTP.UnitTests.{Layer}.{Subfolder}` (e.g., `HTP.UnitTests.Domain.ValueObjects`)
- **Class naming**: `{ClassUnderTest}Tests` (e.g., `FirstNameTests`, `RoleTests`)

## Conventions

### Method naming

```
{Method}_Should{ExpectedBehavior}_When{Condition}
```

Examples:
- `Create_ShouldFail_WhenNameIsEmptyOrWhiteSpace`
- `Create_ShouldCapitalize_FirstName`
- `Grant_ShouldAddPermission_WhenPermissionIsNew`
- `Grant_ShouldRaiseDomainEvent_WhenPermissionChanged`

### Arrange-Act-Assert

Every test must use `// arrange`, `// act`, `// assert` comments (lowercase). When arrange is empty, keep the comment:

```csharp
[Theory]
[InlineData(null)]
[InlineData("")]
public void Create_ShouldFail_WhenNameIsEmptyOrWhiteSpace(string? firstName)
{
    // arrange
    // act
    var result = FirstName.Create(firstName);

    // assert
    result.IsFailure.Should().Be(true);
    result.Error.Should().Be(FirstNameErrors.Empty);
}
```

### Test attributes

- `[Fact]` — single test case
- `[Theory]` + `[InlineData]` — parametrized test cases

### Assertions

Use **FluentAssertions** for all new tests:

```csharp
result.IsFailure.Should().Be(true);
result.Error.Should().Be(SomeErrors.SomeError);
result.Value.Value.Should().Be("Expected");
createdRole.Should().NotBeNull();
role.RolePermissions.Should().HaveCount(2);
role.DomainEvents.Should().Contain(new SomeDomainEvent(...));
role.DomainEvents.Should().BeEmpty();
```

Do **not** use xUnit `Assert.*` in new tests.

### Result pattern

Domain factory methods return `Result<T>`. Test both paths:

- **Failure**: `result.IsFailure.Should().Be(true)` + verify `result.Error` matches a specific error constant
- **Success**: `result.IsSuccess.Should().Be(true)` + verify `result.Value` properties

### Domain events

- Verify events are raised: `entity.DomainEvents.Should().Contain(new SomeDomainEvent(...))`
- Clear events between steps: `entity.ClearDomainEvents()`
- Verify no events: `entity.DomainEvents.Should().BeEmpty()`

### Test data helpers

Use private `CreateValid{Entity}()` factory methods within the test class for reusable valid objects:

```csharp
private Role CreateValidRole()
{
    var roleName = RoleName.Create("Admin").Value;
    var createdAt = DateTimeOffset.UtcNow;
    var userId = Guid.NewGuid();

    return Role.Create(roleName, createdAt, userId);
}
```

### No mocking

Unit tests work with pure domain objects. No mocking libraries are used.
