using HPT.SharedKernel;

namespace HTP.App.Users;

public interface IUserContext
{
    Result<ApplicationUser> GetCurrentUser();
    Result<Guid> GetUserId();
    bool IsAuthenticated { get; }
}
