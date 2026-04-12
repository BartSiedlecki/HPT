using HPT.SharedKernel;
using HPT.SharedKernel.Constants;
using HTP.Domain.Entities.Roles.ValueObjects;

namespace HTP.Domain.Errors;

public static class RoleNameErrors
{
    public readonly static Error Empty = Error.Validation(ErrorCodes.Role.Validation.EmptyName, // todo check commands and results
        fieldErrors: new Dictionary<string, string[]>
        {
            { nameof(RoleName), [] }
        });

    public readonly static Error MaxLengthExceeded = Error.Validation(ErrorCodes.Role.Validation.NameMaximumLengthExceeded(FieldLengths.Role.NameMaxLength), // todo check commands and results
        fieldErrors: new Dictionary<string, string[]>
        {
            { nameof(RoleName), [] }
        });
}
