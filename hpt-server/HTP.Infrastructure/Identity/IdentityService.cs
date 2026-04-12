using HPT.SharedKernel;
using HPT.SharedKernel.Abstractions;
using HTP.App.Abstractions.Authentication;
using HTP.App.Errors;
using HTP.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace HTP.Infrastructure.Identity;

internal sealed class IdentityService(
    UserManager<AppIdentityUser> userManager,
    SignInManager<AppIdentityUser> signInManager,
    IDateTimeProvider clock) : IIdentityService
{
    public async Task<Result<Guid>> LoginAsync(string email, string password, CancellationToken ct)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<Guid>(AuthApplicationErrors.InvalidCredentials);

        if (await userManager.IsLockedOutAsync(user))
        {
            var end = await userManager.GetLockoutEndDateAsync(user);
            var minutes = end.HasValue
                ? (int)Math.Ceiling((end.Value - clock.DateTimeUtcNow).TotalMinutes)
                : 5;

            return Result.Failure<Guid>(AuthApplicationErrors.PasswordLockedOut(minutes));
        }

        var result = await signInManager.CheckPasswordSignInAsync(
            user, password, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            var remaining = await userManager.GetAccessFailedCountAsync(user);

            if (remaining <= 5)
                return Result.Failure<Guid>(AuthApplicationErrors.InvalidCredentialsWithAttempts(remaining));

            return Result.Failure<Guid>(AuthApplicationErrors.InvalidCredentials);
        }

        return Result.Success(user.Id);
    }

    public async Task<Result<string>> GeneratePasswordResetTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await FindByDomainIdAsync(userId, ct);
        if (user is null)
            return Result.Failure<string>(AuthApplicationErrors.UserRegistrationFailed);

        try
        {
            var rawToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var encoded = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));
            return Result<string>.Success(encoded);
        }
        catch
        {
            return Result.Failure<string>(Error.Internal("Cannot generate password reset token"));
        }
    }

    public async Task<Result<Guid>> CreateUserAsync(Email email, Guid domainUserId, CancellationToken ct)
    {
        AppIdentityUser newUser = new AppIdentityUser()
        {
            UserName = email.Value,
            Email = email.Value,
            EmailConfirmed = false,
            DomainUserId = domainUserId
        };

        var result = await userManager.CreateAsync(newUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return Result.Failure<Guid>(Error.Internal($"Create user failed with error: {errors}"));
        }

        return newUser.Id;
    }

    private Task<AppIdentityUser?> FindByIdAsync(Guid userId, CancellationToken ct) =>
            userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

    private Task<AppIdentityUser?> FindByDomainIdAsync(Guid userId, CancellationToken ct) =>
            userManager.Users.FirstOrDefaultAsync(u => u.DomainUserId == userId, ct);
}

//internal sealed class IdentityService(UserManager<AppIdentityUser> userManager) : IIdentityService
//{


//    public async Task<Result<bool>> CheckUserLockedAsync(Email email, CancellationToken ct = default)
//    {
//        var user = await FindByEmailAsync(email, ct);
//        if (user is null)
//            return Result.Failure<bool>(AuthApplicationErrors.InvalidCredentials);

//        var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
//        var now = DateTimeOffset.UtcNow;

//        if (lockoutEnd.HasValue && lockoutEnd.Value > now)
//            return Result.Success(true);

//        if (lockoutEnd.HasValue && lockoutEnd.Value <= now)
//        {
//            await userManager.ResetAccessFailedCountAsync(user);
//            await userManager.SetLockoutEndDateAsync(user, null);
//        }

//        return Result.Success(false);
//    }

//    public async Task<Result<DateTimeOffset?>> GetLockoutEndDateAsync(Email email, CancellationToken ct = default)
//    {
//        var user = await FindByEmailAsync(email, ct);
//        if (user is null)
//            return Result.Failure<DateTimeOffset?>(AuthApplicationErrors.InvalidCredentials);

//        var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);

//        if (!lockoutEnd.HasValue)
//            return Result.Failure<DateTimeOffset?>(Error.Internal("User is not locked out"));

//        return Result.Success<DateTimeOffset?>(lockoutEnd.Value);
//    }

//    public async Task<Result> CheckPasswordAsync(Email email, string password, CancellationToken ct = default)
//    {
//        var user = await FindByEmailAsync(email, ct);
//        if (user is null)
//            return Result.Failure(AuthApplicationErrors.InvalidCredentials);

//        var ok = await userManager.CheckPasswordAsync(user, password);
//        if (ok)
//        {
//            await userManager.ResetAccessFailedCountAsync(user);
//            return Result.Success();
//        }

//        await userManager.AccessFailedAsync(user);
//        return Result.Failure(AuthApplicationErrors.InvalidCredentials);
//    }

//    public async Task<Result<int>> GetRemainingAttemptsAsync(Email emailAddress, CancellationToken ct)
//    {
//        var user = await FindByEmailAsync(emailAddress, ct);
//        if (user is null)
//            return Result.Failure<int>(AuthApplicationErrors.InvalidCredentials);

//        int maxAttempts = userManager.Options.Lockout.MaxFailedAccessAttempts;
//        int failedAttempts = await userManager.GetAccessFailedCountAsync(user);
//        int remainingAttempts = maxAttempts - failedAttempts;

//        return Result.Success(remainingAttempts);
//    }

//    public async Task<Result> VerifyPasswordRecoveryTokenAsync(
//        Guid userId, string token, CancellationToken ct = default)
//    {
//        var user = await FindByIdAsync(userId, ct);
//        if (user is null)
//            return Result.Failure(AuthApplicationErrors.InvalidCredentials);

//        string rawToken;
//        try
//        {
//            rawToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
//        }
//        catch
//        {
//            return Result.Failure(AuthApplicationErrors.InvalidPasswordRecoveryToken);
//        }

//        var provider = userManager.Options.Tokens.PasswordResetTokenProvider;
//        var ok = await userManager.VerifyUserTokenAsync(user, provider, "ResetPassword", rawToken);

//        return ok ? Result.Success() : Result.Failure(AuthApplicationErrors.InvalidPasswordRecoveryToken);
//    }

//    public async Task<Result> ResetPasswordAsync(
//        Guid userId, string token, string newPassword, CancellationToken ct = default)
//    {
//        var user = await FindByIdAsync(userId, ct);
//        if (user is null)
//            return Result.Failure(AuthApplicationErrors.InvalidCredentials);

//        string rawToken;
//        try
//        {
//            rawToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
//        }
//        catch
//        {
//            return Result.Failure(AuthApplicationErrors.InvalidPasswordRecoveryToken);
//        }

//        var res = await userManager.ResetPasswordAsync(user, rawToken, newPassword);
//        if (res.Succeeded) return Result.Success();

//        return Result.Failure(AuthApplicationErrors.PasswordChangeFailed);
//    }

//    public async Task<Result> ChangePasswordAsync(
//        Guid userId, string currentPassword, string newPassword, CancellationToken ct = default)
//    {
//        var user = await FindByIdAsync(userId, ct);
//        if (user is null)
//            return Result.Failure(AuthApplicationErrors.InvalidCredentials);

//        var res = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
//        if (res.Succeeded) return Result.Success();

//        return Result.Failure(AuthApplicationErrors.PasswordChangeFailed);
//    }

//    private Task<AppIdentityUser?> FindByIdAsync(Guid userId, CancellationToken ct) =>
//        userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, ct);

//    private Task<AppIdentityUser?> FindByEmailAsync(Email email, CancellationToken ct) =>
//        userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.Value.ToUpperInvariant(), ct);
//}
