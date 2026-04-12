using HPT.SharedKernel;
using HTP.App.Abstractions.Authentication;
using HTP.App.Abstractions.Repositories.Read;
using HTP.App.Core.Abstractions;
using HTP.App.Core.Abstractions.Mediator;
using HTP.App.Errors;
using HTP.App.Users.Dtos;
using HTP.App.Users.Policies;
using HTP.Domain.Users;

namespace HTP.App.Auth.Login;

public class LoginUserCommandHandler(
    IIdentityService identityService,
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IUserLoginPolicy loginPolicy,
    ITokenIssuer tokenIssuer,
    IUnitOfWork unitOfWork)
    : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<Result<LoginUserResponse>> Handle(
    LoginUserCommand command,
    CancellationToken ct)
    {
        var loginResult = await identityService.LoginAsync(command.Login, command.Password, ct);
        if (loginResult.IsFailure)
            return Result.Failure<LoginUserResponse>(loginResult.Error);

        var userId = loginResult.Value;
       
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null)
            return Result.Failure<LoginUserResponse>(
                AuthApplicationErrors.InvalidCredentials);

        var policyResult = loginPolicy.Validate(user);
        if (policyResult.IsFailure)
            return Result.Failure<LoginUserResponse>(policyResult.Error);

        var roles = await userRoleRepository.GetUserRolesAsync(userId, ct);
        var permissions = await userRoleRepository.GetRolesPermissionsAsync(roles, ct);

        var tokensIssueResult = await tokenIssuer.IssueAsync(user, roles.ToList(), permissions, ct);
        if (tokensIssueResult.IsFailure)
            return Result.Failure<LoginUserResponse>(AuthApplicationErrors.AuthFailed);

        await unitOfWork.SaveChangesAsync(ct);

        return new LoginUserResponse(
            AuthTokens: tokensIssueResult.Value,
            User: new LoggedUserDto(user.Id, user.FirstName.Value, user.LastName.Value, user.Email, roles, permissions));
    }
}
