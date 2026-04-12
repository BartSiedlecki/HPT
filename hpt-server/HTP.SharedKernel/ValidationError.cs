namespace HPT.SharedKernel;

public record class ValidationError : Error
{
    public ValidationError(IEnumerable<FieldValidationError> errors)
        : base(
            "Validation.General",
            "One or more validation errors occurred",
            ErrorType.Validation)
    {
        Errors = errors
            .GroupBy(e => ToCamelCase(e.Field))
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => new
                {
                    e.TranslationPhrase,
                    e.Message,
                    e.Args
                }).ToArray<object?>()
            );
    }

    public Dictionary<string, object?[]> Errors { get; }

    private static string ToCamelCase(string input) =>
        string.IsNullOrEmpty(input)
            ? input
            : char.ToLowerInvariant(input[0]) + input[1..];
}
