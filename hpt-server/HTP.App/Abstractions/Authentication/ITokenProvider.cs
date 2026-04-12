using HTP.Domain.Entities.Users;

namespace HTP.App.Abstractions.Authentication;

public interface ITokenProvider
{
    string GenerateJwtToken(User user, IEnumerable<string> userRoles, IEnumerable<string> permissionIds);
    string GenerateRefreshToken();
}
