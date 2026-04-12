using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;

namespace HTP.Domain.Entities.Roles.ValueObjects;

public sealed record RoleName
{
    public string Value { get; private set; } = null!;
    private RoleName(string value) => Value = value;

    private RoleName() { }

    public static Result<RoleName> Create(string? value)
    {
        var trimmedInput = value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(trimmedInput))
            return Result.Failure<RoleName>(RoleNameErrors.Empty);

        if (trimmedInput.Length > FieldLengths.Role.NameMaxLength)
            return Result.Failure<RoleName>(RoleNameErrors.MaxLengthExceeded);

        return new RoleName(trimmedInput);
    }

    public static implicit operator string(RoleName id)
        => id.Value;
}
