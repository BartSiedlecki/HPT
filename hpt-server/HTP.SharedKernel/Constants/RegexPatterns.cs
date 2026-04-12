namespace HPT.SharedKernel.Constants;

public static class RegexPatterns
{
    public const string HexColor =
        @"^#[0-9A-Fa-f]{6}$";

    // lowercase, uppercase, digit, non alphanumeric
    public const string Password =
    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$";
}
