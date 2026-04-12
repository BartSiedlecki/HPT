using HPT.SharedKernel;
using HTP.App.Errors;
using HTP.Domain.Entities.Users;

namespace HTP.App.Users.Policies;

internal sealed class UserLoginPolicy : IUserLoginPolicy
{
    public Result Validate(User user)
    {
        if (user.IsBlocked)
            return Result.Failure(AuthApplicationErrors.UserBlocked);

        if (user.ActivatedAt is null)
            return Result.Failure(AuthApplicationErrors.AccountInactive);

        return Result.Success();
    }
}
