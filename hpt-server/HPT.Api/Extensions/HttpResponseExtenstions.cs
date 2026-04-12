using HPT.SharedKernel.Abstractions;
using HTP.App.Core.Settings;
using Microsoft.Extensions.Options;

namespace HPT.Api.Extensions;

public static class HttpResponseExtenstions
{
    public static void AppendAuthCookies(
        this HttpResponse response,
        string accessToken,
        string refreshToken,
        IOptions<JwtSettings> jwtSettings,
        IDateTimeProvider clock)
    {
        var accessCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = clock.DateTimeOffsetUtcNow.AddMinutes(jwtSettings.Value.ExpirationMinutes)
        };

        var refreshCookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = clock.DateTimeOffsetUtcNow.AddMinutes(jwtSettings.Value.RefreshTokenExpirationMinutes)
        };

        response.Cookies.Append("Bearer", accessToken, accessCookieOptions);
        response.Cookies.Append("RefreshToken", refreshToken, refreshCookieOptions);
    }

    public static void RemoveAuthCookies(this HttpResponse response)
    {
        response.Cookies.Delete("Bearer");
        response.Cookies.Delete("RefreshToken");
    }
}
