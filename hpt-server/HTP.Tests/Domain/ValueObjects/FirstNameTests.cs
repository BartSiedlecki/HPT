using FluentAssertions;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain.ValueObjects;

public class FirstNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("         ")]
    public void Create_ShouldFail_WhenNameIsEmptyOrWhiteSpace(string? firstName)
    {
        // arrange
        // act
        var result = FirstName.Create(firstName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(FirstNameErrors.Empty);
    }

    [Theory]
    [InlineData("J@n")]
    [InlineData("Jan223")]
    [InlineData("Tomasz!")]
    [InlineData("1gor")]
    [InlineData("Ada m")]
    public void Create_ShouldFail_WhenFirstNameContainsNonLetters(string? firstName)
    {
        // arrange
        // act
        var result = FirstName.Create(firstName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(FirstNameErrors.InvalidFormat);
    }

    [Theory]
    [InlineData("J")]
    [InlineData("a")]
    public void Create_ShouldFail_WhenFirstNameMinLengthNotMet(string? firstName)
    {
        // arrange
        // act
        var result = FirstName.Create(firstName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(FirstNameErrors.MinLengthNotMet);
    }

    [Fact]
    public void Create_ShouldFail_WhenFirstNameExceedMaxLength()
    {
        // arrange
        string firstName = new string('a', FieldLengths.FirstName.MaxLength + 1);

        // act
        var result = FirstName.Create(firstName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(FirstNameErrors.MaxLengthExceeded);
    }

    [Theory]
    [InlineData("test", "Test")]
    [InlineData("jan", "Jan")]
    [InlineData("TOMASZ", "Tomasz")]
    [InlineData("aDam", "Adam")]
    public void Create_ShouldCapitalize_FirstName(string firstName, string expected)
    {
        // arrange
        // act
        var result = FirstName.Create(firstName);

        // assert
        result.IsSuccess.Should().Be(true);
        result.Value.Value.Should().Be(expected);
    }
}
