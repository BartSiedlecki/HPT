using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;

namespace HTP.Domain.ValueObjects;

public sealed record PermissionDescription
{
    public string Value { get; private set; } = null!;

    private PermissionDescription(string value) => Value = value;

    private PermissionDescription() { }

    public static Result<PermissionDescription?> Create(string? description)
    {
        var trimmedValue = description?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedValue))
            return Result.Success<PermissionDescription?>(null);

        if (trimmedValue.Length > FieldLengths.Permission.DescriptionMaxLength)
            return Result.Failure<PermissionDescription?>(PermissionErrors.DescriptionMaxLengthExceeded);

        return Result.Success<PermissionDescription?>(new PermissionDescription(trimmedValue));
    }

    public static implicit operator string(PermissionDescription description)
        => description.Value;
}
