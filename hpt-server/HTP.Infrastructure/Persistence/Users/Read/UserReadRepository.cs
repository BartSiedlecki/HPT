using HTP.App.Abstractions.Repositories.Read;
using HTP.Domain.Entities.Users;

namespace HTP.Infrastructure.Persistence.Users.Read;

internal sealed class UserReadRepository(ReadDbContext readDbContext) : IUserReadRepository
{
    //public async Task<UserReadModel?> GetByIdAsync(Guid userId, CancellationToken ct = default)
    //{
    //    return await readDbContext.Users.FindAsync(userId, ct);

    //    throw new NotImplementedException();
    //}
}
