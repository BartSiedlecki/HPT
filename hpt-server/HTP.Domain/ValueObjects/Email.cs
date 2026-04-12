using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;
using System.Net.Mail;

namespace HTP.Domain.ValueObjects;

public readonly record struct Email
{
    public string Value { get; } = null!;

    private Email(string email) => Value = email;

    public static Result<Email> Create(string? email)
    {
        var fixedInput = email?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(fixedInput))
            return Result.Failure<Email>(EmailErrors.EmptyEmail);

        if (fixedInput.Length > FieldLengths.Email.MaxLength)
            return Result.Failure<Email>(EmailErrors.MaxLengthExceeded);

        if (!IsValid(fixedInput))
            return Result.Failure<Email>(EmailErrors.InvalidEmailFormat);

        return Result.Success(new Email(fixedInput));
    }

    public static implicit operator string(Email email)
        => email.Value;

    private static bool IsValid(string email)
    {
        try
        {
            var emailAddress = new MailAddress(email);
            return emailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

