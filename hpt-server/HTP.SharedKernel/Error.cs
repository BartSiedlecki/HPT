using HPT.SharedKernel.Constants;

namespace HPT.SharedKernel;

public record Error(
    string Code,
    string Message,
    ErrorType Type,
    IReadOnlyDictionary<string, string[]>? FieldErrors = null,
    IReadOnlyDictionary<string, object?>? Args = null)
{
    public static Error None => new(string.Empty, string.Empty, ErrorType.Failure);


    public static readonly Error NullValue = new(
    "General.Null",
    "Null value was provided",
    ErrorType.Failure);

    public static Error Failure(string code, string message)
        => new(code, message, ErrorType.Failure);

    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);
    public static Error NotFound(ErrorCode errorCode)
        => new(errorCode.Code, errorCode.Message, ErrorType.NotFound);

    public static Error Problem(string code, string message, IReadOnlyDictionary<string, string[]>? fieldErrors = null)
        => new(code, message, ErrorType.Problem, fieldErrors);

    public static Error Problem(ErrorCode errorCode, IReadOnlyDictionary<string, string[]>? fieldErrors = null)
        => new(errorCode.Code, errorCode.Message, ErrorType.Problem, fieldErrors);

    public static Error Conflict(string code, string message)
        => new(code, message, ErrorType.Conflict);

    public static Error Conflict(ErrorCode errorCode)
        => new(errorCode.Code, errorCode.Message, ErrorType.Conflict);

    public static Error Unauthorized(ErrorCode errorCode, IReadOnlyDictionary<string, string[]>? fieldErrors = null, IReadOnlyDictionary<string, object?>? args = null)
        => new(errorCode.Code, errorCode.Message, ErrorType.Unauthorized, Args: args);

    public static Error Unauthorized(string code, string message, IReadOnlyDictionary<string, string[]>? fieldErrors = null, IReadOnlyDictionary<string, object?>? args = null)
        => new(code, message, ErrorType.Unauthorized, Args: args);

    public static Error Forbidden(string code, string message)
       => new(code, message, ErrorType.Forbidden);

    public static Error Forbidden(ErrorCode errorCode)
       => new(errorCode.Code, errorCode.Message, ErrorType.Forbidden);

    public static Error ToManyRequests(string code, string message, IReadOnlyDictionary<string, object?>? args = null)
       => new(code, message, ErrorType.ToManyRequests, Args: args);

    public static Error Validation(ErrorCode errorCode, IReadOnlyDictionary<string, string[]>? fieldErrors = null)
        => new(errorCode.Code, errorCode.Message, ErrorType.Validation, fieldErrors);

    public static Error Validation(string code, string message, IReadOnlyDictionary<string, string[]>? fieldErrors = null)
        => new(code, message, ErrorType.Validation, fieldErrors);

    public static Error Internal(string message)
       => new("Internal.Error", message, ErrorType.Internal);
}
