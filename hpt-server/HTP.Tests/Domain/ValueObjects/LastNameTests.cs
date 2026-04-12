using FluentAssertions;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain.ValueObjects;

public class LastNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("         ")]
    public void Create_ShouldFail_WhenNameIsEmptyOrWhiteSpace(string? lastName)
    {
        // arrange
        // act
        var result = LastName.Create(lastName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(LastNameErrors.Empty);
    }

    [Theory]
    [InlineData("Kow@lski")]
    [InlineData("Tomaszewski1")]
    [InlineData("Jankowki_")]
    [InlineData("(Nieznane)")]
    public void Create_ShouldFail_WhenLastNameContainsNonLetters(string? lastName)
    {
        // arrange
        // act
        var result = LastName.Create(lastName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(LastNameErrors.InvalidFormat);
    }

    [Theory]
    [InlineData("J")]
    [InlineData("a")]
    public void Create_ShouldFail_WhenLastNameMinLengthNotMet(string? lastName)
    {
        // arrange
        // act
        var result = LastName.Create(lastName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(LastNameErrors.MinLengthNotMet);
    }

    [Fact]
    public void Create_ShouldFail_WhenLastNameExceedMaxLength()
    {
        // arrange
        string lastName = new string('a', FieldLengths.LastName.MaxLength + 1);

        // act
        var result = LastName.Create(lastName);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(LastNameErrors.MaxLengthExceeded);
    }

    [Theory]
    [InlineData("test", "Test")]
    [InlineData("michalak", "Michalak")]
    [InlineData("TOMASZEWSKI", "Tomaszewski")]
    [InlineData("RYŚ", "Ryś")]
    public void Create_ShouldCapitalize_LastName(string lastName, string expected)
    {
        // arrange
        // act
        var result = LastName.Create(lastName);

        // assert
        result.IsSuccess.Should().Be(true);
        result.Value.Value.Should().Be(expected);
    }
}
