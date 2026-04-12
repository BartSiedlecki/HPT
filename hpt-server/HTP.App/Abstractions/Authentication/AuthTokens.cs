using HTP.Domain.Entities.RefreshTokens;

namespace HTP.App.Abstractions.Authentication;

public sealed record AuthTokens(
    string AccessToken,
    RefreshToken RefreshToken);