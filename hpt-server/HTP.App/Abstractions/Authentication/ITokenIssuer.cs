using HPT.SharedKernel;
using HTP.Domain.Entities.Users;

namespace HTP.App.Abstractions.Authentication;

public interface ITokenIssuer
{
    Task<Result<AuthTokens>> IssueAsync(
        User user,
        IReadOnlyCollection<string> roles,
        IReadOnlyCollection<string> permissions,
        CancellationToken ct);
}
