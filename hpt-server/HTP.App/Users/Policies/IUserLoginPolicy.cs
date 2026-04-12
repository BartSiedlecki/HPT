using HPT.SharedKernel;
using HTP.Domain.Entities.Users;

namespace HTP.App.Users.Policies;

public interface IUserLoginPolicy
{
    Result Validate(User user);
}
