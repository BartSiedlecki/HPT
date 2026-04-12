using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;

namespace HTP.Domain.ValueObjects;

public sealed record LastName
{
    public string Value { get; } = null!;

    private LastName(string lastName) => Value = lastName;

    public static Result<LastName> Create(string? lastName)
    {
        var trimmedLastName = lastName?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedLastName))
            return Result.Failure<LastName>(LastNameErrors.Empty);

        if (trimmedLastName.Length < FieldLengths.LastName.MinLength)
            return Result.Failure<LastName>(LastNameErrors.MinLengthNotMet);

        if (trimmedLastName.Length > FieldLengths.LastName.MaxLength)
            return Result.Failure<LastName>(LastNameErrors.MaxLengthExceeded);

        if (!trimmedLastName.All(char.IsLetter))
            return Result.Failure<LastName>(LastNameErrors.InvalidFormat);

        var capitalized = Capitalize(trimmedLastName);

        return Result.Success(new LastName(capitalized));
    }

    private static string Capitalize(string text)
    {
        return char.ToUpperInvariant(text[0]) + text.Substring(1).ToLowerInvariant();
    }
}
