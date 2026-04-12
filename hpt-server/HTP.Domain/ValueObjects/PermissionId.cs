using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Errors;

namespace HTP.Domain.ValueObjects;

public sealed record PermissionId
{
    public string Value { get; private set; } = null!;

    private PermissionId(string id) => Value = id;

    private PermissionId() { }

    public static Result<PermissionId> Create(string? permissionId)
    {
        if (string.IsNullOrWhiteSpace(permissionId))
            return Result.Failure<PermissionId>(PermissionIdErrors.EmptyId);

        if (permissionId.Length > FieldLengths.PermissionId.MaxLength)
            return Result.Failure<PermissionId>(PermissionIdErrors.MaxLengthExceeded);

        return Result.Success(new PermissionId(permissionId));
    }

    public static implicit operator string(PermissionId id)
        => id.Value;
}