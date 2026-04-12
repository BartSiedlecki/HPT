using HPT.SharedKernel.Abstractions;
using HTP.App.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HTP.Infrastructure.Persistence;


internal class UnitOfWork(WriteDbContext writeDbContext, IDateTimeProvider clock) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        // ApplyAuditInfo(writeDbContext, clock);
        await writeDbContext.SaveChangesAsync(ct);
    }

    //private static void ApplyAuditInfo(WriteDbContext dataContext, IDateTimeProvider clock)
    //{
    //    foreach (var entry in dataContext.ChangeTracker.Entries<IAuditableEntity>())
    //    {
    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.CreatedAt = clock.DateTimeOffsetUtcNow;
    //        }

    //        if (entry.State == EntityState.Modified)
    //        {
    //            entry.Entity.UpdatedAt = clock.DateTimeOffsetUtcNow;
    //        }
    //    }
    //}
}

