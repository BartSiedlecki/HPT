namespace HTP.UnitTests.Domain.ValueObjects;

using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;
using HTP.Domain.ValueObjects;

public class PermissionIdTests
{
    [Fact]
    public void Create_ShouldFail_WhenIdIsEmpty()
    {
        // Arrange
        var permissionId = string.Empty;

        // Act
        var result = PermissionId.Create(permissionId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(PermissionIdErrors.EmptyId, result.Error);
    }

    [Fact]
    public void Create_ShouldFail_WhenIdExceedsMaxLength()
    {
        // Arrange
        var permissionId = new string('x', FieldLengths.PermissionId.MaxLength + 1);

        // Act
        var result = PermissionId.Create(permissionId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(PermissionIdErrors.MaxLengthExceeded, result.Error);
    }

    [Theory]
    [InlineData("roles.read")]
    [InlineData("users.read.all")]
    [InlineData("projects.delete")]
    [InlineData("projects.delete.own")]
    [InlineData("projects.update.all")]
    public void Create_ShouldSucceed_WhenIdIsCorrect(string permissionId)
    {
        // Arrange
        // Act
        var result = PermissionId.Create(permissionId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
    }
}
