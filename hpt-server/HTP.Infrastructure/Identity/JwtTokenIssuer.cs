using HPT.SharedKernel;
using HTP.App.Abstractions.Authentication;
using HTP.Domain.Entities.Users;

namespace HTP.Infrastructure.Identity;

internal class JwtTokenIssuer(
    ITokenProvider tokenProvider,
    IRefreshTokenService refreshTokenService) : ITokenIssuer
{
    public async Task<Result<AuthTokens>> IssueAsync(User user, IReadOnlyCollection<string> roles, IReadOnlyCollection<string> permissions, CancellationToken ct)
    {
        var accessToken = tokenProvider.GenerateJwtToken(user, roles, permissions);
        if (accessToken == null) 
            return Result.Failure<AuthTokens>(Error.Internal("Access token generation failed"));
        
        var refreshToken = await refreshTokenService.CreateAsync(user, ct);
        if (accessToken == null)
            return Result.Failure<AuthTokens>(Error.Internal("Refresh token generation failed"));

        var authTokens = new AuthTokens(accessToken, refreshToken);

        return authTokens;
    }
}
