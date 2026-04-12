namespace HTP.App.Core.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    public required string SecretKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }

    public required int ExpirationMinutes { get; init; }
    public required int RefreshTokenExpirationMinutes { get; init; }
}
