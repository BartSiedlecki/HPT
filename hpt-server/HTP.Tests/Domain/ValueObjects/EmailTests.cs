using FluentAssertions;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;
using HTP.Domain.ValueObjects;

namespace HTP.UnitTests.Domain.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("         ")]
    public void Create_ShouldFail_WhenEmailIsEmptyOrWhiteSpace(string? email)
    {
        // arrange
        // act
        var result = Email.Create(email);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(EmailErrors.EmptyEmail);
    }

    [Fact]
    public void Create_ShouldFail_WhenEmailExceedsMaxLength()
    {
        // arrange
        var email = new string('a', FieldLengths.Email.MaxLength) + "@test.com";

        // act
        var result = Email.Create(email);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(EmailErrors.MaxLengthExceeded);
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("missing@")]
    [InlineData("@missing.com")]
    [InlineData("double@@at.com")]
    [InlineData("no spaces@test.com")]
    public void Create_ShouldFail_WhenEmailFormatIsInvalid(string email)
    {
        // arrange
        // act
        var result = Email.Create(email);

        // assert
        result.IsFailure.Should().Be(true);
        result.Error.Should().Be(EmailErrors.InvalidEmailFormat);
    }

    [Theory]
    [InlineData("john@example.com")]
    [InlineData("user.name@domain.org")]
    [InlineData("test+tag@gmail.com")]
    public void Create_ShouldSucceed_WhenEmailIsValid(string email)
    {
        // arrange
        // act
        var result = Email.Create(email);

        // assert
        result.IsSuccess.Should().Be(true);
        result.Value.Value.Should().Be(email);
    }

    [Theory]
    [InlineData("  john@example.com  ", "john@example.com")]
    [InlineData("John@Example.COM", "john@example.com")]
    [InlineData("  TEST@GMAIL.COM  ", "test@gmail.com")]
    public void Create_ShouldTrimAndLowercase_WhenEmailIsValid(string email, string expected)
    {
        // arrange
        // act
        var result = Email.Create(email);

        // assert
        result.IsSuccess.Should().Be(true);
        result.Value.Value.Should().Be(expected);
    }
}
