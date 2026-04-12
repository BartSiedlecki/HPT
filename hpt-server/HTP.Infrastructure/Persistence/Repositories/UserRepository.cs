using HTP.Domain.Entities.Users;
using HTP.Domain.Users;
using HTP.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository synchronizing Domain.User with Infrastructure.AppIdentityUser.
/// Maintains consistency between domain logic (Users_Domain) and authentication (Users_Identity).
/// </summary>
public class UserRepository(WriteDbContext writeDbContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        writeDbContext.Users.Add(user);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await writeDbContext.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken ct = default)
    {
        return await writeDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
    
    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken ct = default)
    {
        return await writeDbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);
    }

    //private readonly ApplicationDbContext _context;
    //private readonly UserManager<AppIdentityUser> _userManager;

    //public UserRepository(
    //    ApplicationDbContext context,
    //    UserManager<AppIdentityUser> userManager)
    //{
    //    _context = context;
    //    _userManager = userManager;
    //}

    //public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    //{
    //    return await _context.DomainUsers
    //        .FirstOrDefaultAsync(u => u.Id == id, ct);
    //}

    //public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    //{
    //    return await _context.DomainUsers
    //        .FirstOrDefaultAsync(u => u.Email == email, ct);
    //}

    //public async Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken ct = default)
    //{
    //    return await _context.DomainUsers
    //        .ToListAsync(ct);
    //}

    //public async Task AddAsync(User user, CancellationToken ct = default)
    //{
    //    await _context.DomainUsers.AddAsync(user, ct);

    //    var identityUser = user.ToIdentityUser();
    //    identityUser.PasswordHash = user.PasswordHash;

    //    await _userManager.CreateAsync(identityUser);

    //    await _context.SaveChangesAsync(ct);
    //}

    //public async Task UpdateAsync(User user, CancellationToken ct = default)
    //{
    //    _context.DomainUsers.Update(user);

    //    var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
    //    if (identityUser != null)
    //    {
    //        identityUser.SyncFromDomain(user);
    //        await _userManager.UpdateAsync(identityUser);
    //    }

    //    await _context.SaveChangesAsync(ct);
    //}

    //public async Task DeleteAsync(User user, CancellationToken ct = default)
    //{
    //    _context.DomainUsers.Remove(user);

    //    var identityUser = await _userManager.FindByIdAsync(user.Id.ToString());
    //    if (identityUser != null)
    //    {
    //        await _userManager.DeleteAsync(identityUser);
    //    }

    //    await _context.SaveChangesAsync(ct);
    //}

    //public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    //{
    //    return await _context.DomainUsers
    //        .AnyAsync(u => u.Email == email, ct);
    //}

}
