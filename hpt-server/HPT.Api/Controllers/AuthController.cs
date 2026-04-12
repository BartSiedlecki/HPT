using HPT.Api.Extensions;
using HPT.Api.Infrastructure;
using HPT.SharedKernel.Abstractions;
using HTP.App.Auth.CreateByAdmin;
using HTP.App.Auth.Login;
using HTP.App.Core.Abstractions.Mediator;
using HTP.App.Core.Settings;
using HTP.App.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HPT.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IDispatcher dispatcher,
    IOptions<JwtSettings> jwtSettings,
    IDateTimeProvider dateTimeProvider,
    IHostEnvironment hostEnvironment) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoggedUserDto>> Login([FromBody] LoginUserCommand command, CancellationToken ct = default)
    {
       var result = await dispatcher.Send(command, ct);

        return result.Match(
            onSuccess: response =>
            {
                Response.AppendAuthCookies(response.AuthTokens.AccessToken, response.AuthTokens.RefreshToken.Token, jwtSettings, dateTimeProvider);

                if(hostEnvironment.IsDevelopment())
                {
                    Response.Headers["X-Access-Token"] = response.AuthTokens.AccessToken;
                }
                return Ok(response);
            }, 
        onFailure: CustomResults.Problem);
    }

    // TODO [Authorize]
    [HttpPost("create-user-by-admin")]
    public async Task<ActionResult<string>> RegisterByAdmin([FromBody] CreateUserByAdminCommand command, CancellationToken ct = default)
    {
        var result = await dispatcher.Send(command);

        return result.Match(
            onSuccess: response =>
            {
                return Ok(response);
            },
        onFailure: CustomResults.Problem);
    }
}
