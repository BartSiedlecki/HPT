using HPT.SharedKernel;

namespace HPT.SharedKernel.Abstractions;

/// <summary>
/// Unit of Work pattern for managing database transactions.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
