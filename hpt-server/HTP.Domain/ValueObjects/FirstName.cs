using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;

namespace HTP.Domain.ValueObjects;

public sealed record FirstName
{
    public string Value { get; } = null!;

    private FirstName(string firstName) => Value = firstName;

    public static Result<FirstName> Create(string? firstName)
    {
        var trimmedFirstName = firstName?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedFirstName))
            return Result.Failure<FirstName>(FirstNameErrors.Empty);

        if (trimmedFirstName.Length < FieldLengths.FirstName.MinLength)
            return Result.Failure<FirstName>(FirstNameErrors.MinLengthNotMet);

        if (trimmedFirstName.Length > FieldLengths.FirstName.MaxLength)
            return Result.Failure<FirstName>(FirstNameErrors.MaxLengthExceeded);

        if (!trimmedFirstName.All(char.IsLetter))
            return Result.Failure<FirstName>(FirstNameErrors.InvalidFormat);

        var capitalized = Capitalize(trimmedFirstName);

        return Result.Success(new FirstName(capitalized));
    }

    private static string Capitalize(string text)
    {
        return char.ToUpperInvariant(text[0]) + text.Substring(1).ToLowerInvariant();
    }
}
