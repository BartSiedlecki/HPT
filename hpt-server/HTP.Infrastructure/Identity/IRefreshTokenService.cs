using HTP.App.Abstractions.Authentication;
using HTP.Domain.Entities.RefreshTokens;
using HTP.Domain.Entities.Users;

namespace HTP.Infrastructure.Identity;

public interface IRefreshTokenService
{
    Task<RefreshToken> CreateAsync(User user, CancellationToken ct);
}
