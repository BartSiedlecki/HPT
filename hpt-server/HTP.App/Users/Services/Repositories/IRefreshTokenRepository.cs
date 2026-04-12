using HTP.App.Abstractions.Authentication;
using HTP.Domain.Entities.RefreshTokens;

namespace HTP.App.Users.Services.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string refreshToken, CancellationToken ct);
    void Remove(RefreshToken token);
}