namespace HPT.SharedKernel;

public sealed record FieldValidationError(
    string Field,
    string TranslationPhrase,
    string Message,
    Dictionary<string, object>? Args = null
);