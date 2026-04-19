using HPT.SharedKernel;
using HPT.SharedKernel.Abstractions;
using HTP.App.Abstractions;
using HTP.App.Abstractions.Authentication;
using HTP.App.Abstractions.Repositories.Read;
using HTP.App.Auth.CreateByAdmin;
using HTP.App.Core.Abstractions;
using HTP.App.Core.Abstractions.Mediator;
using HTP.App.Users.Errors;
using HTP.Domain.Entities.Users;
using HTP.Domain.Users;
using HTP.Domain.ValueObjects;

namespace HTP.App.Users.Commands.CreateUser;

/// <summary>
/// Handler creating new user by administrator.
/// Creates Domain.User, generates password reset token, and assigns roles.
/// </summary>
internal sealed class CreateUserByAdminCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IIdentityService identityService,
    IUrlProvider urlProvider,
    IDateTimeProvider clock,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateUserByAdminCommand, CreateUserByAdminResponse>
{
    public async Task<Result<CreateUserByAdminResponse>> Handle(CreateUserByAdminCommand command, CancellationToken ct)
    {
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
            return Result.Failure<CreateUserByAdminResponse>(emailResult.Error);
        Email email = emailResult.Value;

        var firstNameResult = FirstName.Create(command.FirstName);
        if (firstNameResult.IsFailure)
            return Result.Failure<CreateUserByAdminResponse>(firstNameResult.Error);
        FirstName firstName = firstNameResult.Value;

        var lastNameResult = LastName.Create(command.LastName);
        if (lastNameResult.IsFailure)
            return Result.Failure<CreateUserByAdminResponse>(lastNameResult.Error);
        LastName lastName = lastNameResult.Value;

        var userExists = await userRepository.ExistsByEmailAsync(email, ct);
        if (userExists)
            return Result.Failure<CreateUserByAdminResponse>(UserApplicationErrors.EmailAlreadyExists);

        User user = User.Create(firstName, lastName, email, clock.DateTimeOffsetUtcNow);

        await userRepository.AddAsync(user, ct);
        var createIdentityUserResult = await identityService.CreateUserAsync(email, domainUserId: user.Id, ct);
        if (createIdentityUserResult.IsFailure)
            return Result.Failure<CreateUserByAdminResponse>(createIdentityUserResult.Error);

        // Generate password reset token
        var tokenResult = await identityService.GeneratePasswordResetTokenAsync(user.Id, ct);
        if (tokenResult.IsFailure)
            return Result.Failure<CreateUserByAdminResponse>(tokenResult.Error);

        var url = urlProvider.GetSetUserPasswordUrl(user.Id, tokenResult.Value);

        // Assign roles
        foreach (var roleName in command.Roles)
        {
            var role = await userRoleRepository.GetRoleByNameAsync(roleName, ct);
            if (role is null)
                return Result.Failure<CreateUserByAdminResponse>(UserApplicationErrors.RoleNotFound(roleName));

            user.AddRole(role.Id);
        }

        await unitOfWork.SaveChangesAsync(ct);

        var response = new CreateUserByAdminResponse(url);
        return Result.Success(response);
    }
}
