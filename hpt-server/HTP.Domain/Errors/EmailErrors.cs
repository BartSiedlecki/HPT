using HPT.SharedKernel;
using HPT.SharedKernel.Constants;

namespace HTP.Domain.Errors;

public static class EmailErrors
{
    public static Error EmptyEmail => Error.Problem(ErrorCodes.Email.Validation.Empty);

    public static Error MaxLengthExceeded => Error.Problem(ErrorCodes.Email.Validation.MaximumLengthExceeded);

    public static Error InvalidEmailFormat => Error.Problem(ErrorCodes.Email.Validation.InvalidFormat);
}