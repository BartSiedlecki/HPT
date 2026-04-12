using HPT.SharedKernel;
using HPT.SharedKernel.Constants;

namespace HTP.Domain.Errors;

public static class FirstNameErrors
{
    public static Error Empty => Error.Problem(ErrorCodes.FirstName.Validation.Empty);
    public static Error InvalidFormat => Error.Problem(ErrorCodes.FirstName.Validation.InvalidFormat);
    public static Error MinLengthNotMet => Error.Problem(ErrorCodes.FirstName.Validation.MinLengthNotMet);
    public static Error MaxLengthExceeded => Error.Problem(ErrorCodes.FirstName.Validation.MaxLengthExceeded);
}
