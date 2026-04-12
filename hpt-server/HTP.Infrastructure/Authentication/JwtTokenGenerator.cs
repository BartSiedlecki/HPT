using HPT.SharedKernel.Abstractions;
using HTP.App.Abstractions.Authentication;
using HTP.App.Core.Settings;
using HTP.App.Users.Authentication;
using HTP.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HTP.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IOptions<JwtSettings> jwtOptions, 
    IDateTimeProvider clock) : ITokenProvider
{
    public string GenerateJwtToken(User user, IEnumerable<string> userRoles, IEnumerable<string> permissionIds)
    {
        var secretKey = jwtOptions.Value.SecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
           {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
            };

        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var pId in permissionIds.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct())
            claims.Add(new Claim(CustomClaimTypes.Permissions, pId));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = clock.DateTimeUtcNow.AddMinutes(jwtOptions.Value.ExpirationMinutes),
            Issuer = jwtOptions.Value.Issuer,
            Audience = jwtOptions.Value.Audience,
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}
