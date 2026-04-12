using HTP.Domain.Entities.Users;
using HTP.Domain.ValueObjects;

namespace HTP.Domain.Users;

public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken ct = default);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct = default);
    //Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken ct = default);
    //Task UpdateAsync(User user, CancellationToken ct = default);
    //Task DeleteAsync(User user, CancellationToken ct = default);
}
