using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain.ValueObjects;

public class PermissionDescriptionTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("       ")]
    public void Create_ShouldReturnNull_WhenDescriptionIsNullOrWhiteSpace(string? description)
    {
        //arrange
        //act
        var result = PermissionDescription.Create(description);

        //assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        Assert.Null(result.Value);
    }

    [Fact]
    public void Create_ShouldFail_WhenDescriptionExceedsMaxLength()
    {
        // arrange
        var tooLongLength = FieldLengths.Permission.DescriptionMaxLength + 1;
        var descriptionText = new string('x', tooLongLength);

        // act
        var result = PermissionDescription.Create(descriptionText);

        // assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(PermissionErrors.DescriptionMaxLengthExceeded, result.Error);
    }
}
