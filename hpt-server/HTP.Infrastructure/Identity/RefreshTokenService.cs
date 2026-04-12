using HPT.SharedKernel.Abstractions;
using HTP.App.Abstractions.Authentication;
using HTP.App.Core.Settings;
using HTP.App.Users.Services.Repositories;
using HTP.Domain.Entities.RefreshTokens;
using HTP.Domain.Entities.Users;
using Microsoft.Extensions.Options;

namespace HTP.Infrastructure.Identity;

public sealed class RefreshTokenService(
    ITokenProvider tokenProvider,
    IDateTimeProvider clock,
    IOptions<JwtSettings> options,
    IRefreshTokenRepository repository) : IRefreshTokenService
{
    public async Task<RefreshToken> CreateAsync(User user, CancellationToken ct)
    {
        var value = tokenProvider.GenerateRefreshToken();
        var expiration = clock.DateTimeOffsetUtcNow
            .AddMinutes(options.Value.RefreshTokenExpirationMinutes);

        var refreshToken = RefreshToken.Create(value, expiration, user.Id);

        repository.Add(refreshToken);

        return refreshToken;
    }
}
