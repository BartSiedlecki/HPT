using HPT.SharedKernel;
using HTP.App.Abstractions.Repositories.Read;
using HTP.App.Core.Abstractions.Mediator;
using HTP.App.Users.Dtos;
using HTP.App.Users.Errors;
using HTP.App.Users.Policies;
using HTP.Domain.Users;

namespace HTP.App.Users.Queries.Current;

/// <summary>
/// Handler retrieving current logged user information.
/// Returns LoggedUserDto with user data, roles, and permissions.
/// </summary>
internal class CurrentUserQueryHandler(
    IUserContext userContext,
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IUserLoginPolicy loginPolicy) : IQueryHandler<CurrentUserQuery, LoggedUserDto>
{
    public async Task<Result<LoggedUserDto>> Handle(CurrentUserQuery query, CancellationToken ct)
    {
        var userContextResult = userContext.GetCurrentUser();
        if (userContextResult.IsFailure)
            return Result.Failure<LoggedUserDto>(userContextResult.Error);

        var user = await userRepository.GetByIdAsync(userContextResult.Value.UserId, ct);
        if (user is null)
            return Result.Failure<LoggedUserDto>(UserApplicationErrors.UserNotFound);

        var policyResult = loginPolicy.Validate(user);
        if (policyResult.IsFailure)
            return Result.Failure<LoggedUserDto>(policyResult.Error);

        var roles = await userRoleRepository.GetUserRolesAsync(user.Id, ct);

        var permissions = await userRoleRepository.GetRolesPermissionsAsync(roles, ct);

        var response = new LoggedUserDto(
            Id: user.Id,
            FirstName: user.FirstName.Value,
            LastName: user.LastName.Value,
            Email: user.Email!,
            Roles: roles.ToList(),
            Permissions: permissions.ToList()
        );

        return Result.Success(response);
    }
}
