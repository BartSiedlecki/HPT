using HPT.SharedKernel;
using HPT.SharedKernel.Constants;

namespace HTP.Domain.Errors
{
    public static class LastNameErrors
    {
        public static Error Empty => Error.Problem(ErrorCodes.LastName.Validation.Empty);
        public static Error InvalidFormat => Error.Problem(ErrorCodes.LastName.Validation.InvalidFormat);
        public static Error MinLengthNotMet => Error.Problem(ErrorCodes.LastName.Validation.MinLengthNotMet);
        public static Error MaxLengthExceeded => Error.Problem(ErrorCodes.LastName.Validation.MaxLengthExceeded);
    }
}
