
using HTP.App.Users.Services.Repositories;
using HTP.Domain.Entities.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence.RefreshTokens.Write;

internal sealed class RefreshTokenRepository(WriteDbContext writeDbContext) : IRefreshTokenRepository
{
    public void Add(RefreshToken token)
    {
        writeDbContext.RefreshTokens.Add(token);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        return await writeDbContext.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshToken);
    }

    public void Remove(RefreshToken token)
    {
        writeDbContext.RefreshTokens.Remove(token);
    }
}
