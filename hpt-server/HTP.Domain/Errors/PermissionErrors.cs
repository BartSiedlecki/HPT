using HPT.SharedKernel;
using HPT.SharedKernel.Constants;

namespace HTP.Domain.Errors;

public class PermissionErrors
{
    public static Error DescriptionMaxLengthExceeded 
        => Error.Internal($"Permission description exceeds {FieldLengths.Permission.DescriptionMaxLength} characters.");
}
