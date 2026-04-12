//using HPT.SharedKernel;
//using HPT.SharedKernel.Abstractions;
//using HTP.App.Core.Abstractions.Mediator;
//using HTP.App.Errors;
//using HTP.App.Users.Commands.Login;
//using HTP.App.Users.Dtos;
//using HTP.App.Users.Services.Repositories;

//namespace HTP.App.Users.Commands.Refresh;

//internal sealed class RefreshUserCommandHandler(
//    IRefreshTokenRepository refreshTokenRepository,
//    IUserRoleRepository userRoleRepository,
//    IDateTimeProvider clock,
//    ITokenProvider tokenProvider,
//    IOptions<JwtSettings> jwtSettings,
//    IUnitOfWork unitOfWork)
//    : ICommandHandler<RefreshUserCommand, LoginUserResponse>
//{
//    public async Task<Result<LoginUserResponse>> Handle(RefreshUserCommand command, CancellationToken ct)
//    {
//        var refreshToken = await refreshTokenRepository.GetByTokenIncludingUser(command.RefreshToken, ct);


//        if (refreshToken is null || refreshToken.ExpiredOnUtc < clock.DateTimeOffsetUtcNow)
//            return Result.Failure<LoginUserResponse>(AuthApplicationErrors.InvalidRefreshToken);

//        if (refreshToken.User.Blocked)
//            return Result.Failure<LoginUserResponse>(AuthApplicationErrors.UserBlocked);

//        var isPasswordSet = refreshToken.User.FirstPasswordSetAt is not null;
//        if (!isPasswordSet)
//            Result.Failure<LoginUserResponse>(AuthApplicationErrors.AccountInactive);

//        var roles = await userRoleRepository.GetRolesAsync(refreshToken.User);
//        var permissions = await userRoleRepository.GetRolesPermissionsAsync(roles);

//        var newAccessToken = tokenProvider.GenerateJwtToken(refreshToken.User, roles, permissions);

//        refreshToken.UpdateRefreshToken(
//            newToken: tokenProvider.GenerateRefreshToken(),
//            expiredOnUtc: clock.DateTimeOffsetUtcNow.AddMinutes(jwtSettings.Value.RefreshTokenExpirationMinutes)
//        );

//        await unitOfWork.SaveChangesAsync(ct);

//        return new LoginUserResponse(
//            Token: newAccessToken,
//            RefreshToken: refreshToken.Token,
//            User: new LoggedUserDto(
//                Id: refreshToken.User.Id,
//                FirstName: refreshToken.User.FirstName,
//                LastName: refreshToken.User.LastName,
//                Email: refreshToken.User.Email!,
//                Roles: roles.ToList(),
//                Permissions: permissions.ToList()
//            ));
//    }
//}

