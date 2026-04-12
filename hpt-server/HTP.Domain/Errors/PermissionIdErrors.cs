using HPT.SharedKernel;
using HPT.SharedKernel.Constants;

namespace HTP.Domain.Errors;

public class PermissionIdErrors
{
    public static Error EmptyId => Error.Internal("Permission Id cannot be empty.");
    public static Error MaxLengthExceeded => Error.Internal($"Permission Id exceeds {FieldLengths.PermissionId.MaxLength} characters.");
}
