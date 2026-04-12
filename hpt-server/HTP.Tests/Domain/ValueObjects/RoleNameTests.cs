using FluentAssertions;
using HPT.SharedKernel.Constants;
using HTP.Domain.Entities.Roles.ValueObjects;
using HTP.Domain.Errors;

namespace HTP.UnitTests.Domain.ValueObjects;

public class RoleNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    [InlineData("    ")]
    public void Create_ShouldFail_WhenNameIsNullOrWhiteSpace(string? roleName)
    {
        // arrange
        // act
        var result = RoleName.Create(roleName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(RoleNameErrors.Empty);
    }

    [Fact]
    public void Create_ShouldFail_WhenNameExceedMaxLength()
    {
        // arrange
        var toLongName = new string('x', FieldLengths.Role.NameMaxLength + 1);

        // act
        var result = RoleName.Create(toLongName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(RoleNameErrors.MaxLengthExceeded);
    }
}
