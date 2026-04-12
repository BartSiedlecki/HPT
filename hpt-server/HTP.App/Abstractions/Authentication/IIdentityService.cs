using HPT.SharedKernel;
using HTP.Domain.ValueObjects;

namespace HTP.App.Abstractions.Authentication;

public interface IIdentityService
{
    Task<Result<Guid>> CreateUserAsync(Email email, Guid domainUserId, CancellationToken ct);
    Task<Result<Guid>> LoginAsync(string email, string password, CancellationToken ct);
    Task<Result<string>> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default);
}
