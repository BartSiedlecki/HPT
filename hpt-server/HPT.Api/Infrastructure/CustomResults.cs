using HPT.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace HPT.Api.Infrastructure;

public static class CustomResults
{
    public static ActionResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot return a problem response for a successful result.");
        }

        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(result.Error),
            Detail = GetDetail(result.Error),
            Type = GetType(result.Error.Type),
            Status = GetStatusCode(result.Error.Type)
        };

        var extensions = GetExtensionss(result);
        if (extensions is not null)
        {
            foreach (var (key, value) in extensions)
            {
                problemDetails.Extensions[key] = value;
            }
        }

        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }

    private static string GetTitle(Error error) =>
        error.Type switch
        {
            ErrorType.Validation => error.Code,
            ErrorType.Problem => error.Code,
            ErrorType.NotFound => error.Code,
            ErrorType.Conflict => error.Code,
            ErrorType.Unauthorized => error.Code,
            ErrorType.Forbidden => error.Code,
            ErrorType.ToManyRequests => error.Code,
            _ => "Server Failure"
        };

    private static string GetDetail(Error error) =>
        error.Type switch
        {
            ErrorType.Validation => error.Message,
            ErrorType.Problem => error.Message,
            ErrorType.NotFound => error.Message,
            ErrorType.Conflict => error.Message,
            ErrorType.Unauthorized => error.Message,
            ErrorType.Forbidden => error.Message,
            ErrorType.ToManyRequests => error.Message,
            _ => "An unexpected error occurred"
        };

    private static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
            ErrorType.ToManyRequests => "https://datatracker.ietf.org/doc/html/rfc6585#section-4",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.ToManyRequests => StatusCodes.Status429TooManyRequests,
            _ => StatusCodes.Status500InternalServerError
        };

    private static Dictionary<string, object?>? GetExtensionss(Result result)
    {
        if (result.Error is ValidationError validationError)
        {
            return new Dictionary<string, object?>
            {
                { "errors", validationError.Errors }
            };
        }

        if (result.Error.FieldErrors is not null)
        {
            var camelCaseErrors = result.Error.FieldErrors.ToDictionary(
                pair => char.ToLowerInvariant(pair.Key[0]) + pair.Key[1..],
                pair => (object?)pair.Value
            );

            return new Dictionary<string, object?>
            {
                { "errors", camelCaseErrors }
            };
        }

        if (result.Error.Args is not null)
        {
            var camelCaseArgs = result.Error.Args.ToDictionary(
               pair => char.ToLowerInvariant(pair.Key[0]) + pair.Key[1..],
               pair => (object?)pair.Value
           );

            return new Dictionary<string, object?>
            {
                { "args", camelCaseArgs }
            };
        }

        return null;
    }
}
