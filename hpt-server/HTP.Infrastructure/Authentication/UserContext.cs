using HTP.App.Users;
using HTP.App.Users.Authentication;
using HPT.SharedKernel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HTP.Domain.ValueObjects;

namespace HTP.Infrastructure.Authentication;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public bool IsAuthenticated => 
        httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public Result<ApplicationUser> GetCurrentUser()
    {
        var user = httpContextAccessor.HttpContext?.User;

        if (user is null || !IsAuthenticated)
            return Result.Failure<ApplicationUser>(AuthErrors.UnathenticatedUser);

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Result.Failure<ApplicationUser>(AuthErrors.UnathenticatedUser);

        var emailResult = Email.Create(user.FindFirstValue(ClaimTypes.Email));
        if (emailResult.IsFailure)
            return Result.Failure<ApplicationUser>(AuthErrors.UnathenticatedUser);

        var roles = user.FindAll(ClaimTypes.Role)
                        .Select(r => r.Value)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var permissions = user.FindAll(CustomClaimTypes.Permissions)
                        .Select(r => r.Value)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var appUser = new ApplicationUser
        {
            UserId = userId,
            Email = emailResult.Value,
            Roles = roles.ToList(),
            Permissions = permissions
        };

        return Result.Success(appUser);
    }

    public Result<Guid> GetUserId()
    {
        if (!IsAuthenticated)
            return Result.Failure<Guid>(AuthErrors.UnathenticatedUser);

        var userIdClaim = httpContextAccessor.HttpContext?.User
            .FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Result.Failure<Guid>(AuthErrors.UnathenticatedUser);

        return Result.Success(userId);
    }
}
