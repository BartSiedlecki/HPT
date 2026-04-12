using HTP.App.Abstractions.Authentication;
using HTP.App.Users.Dtos;

namespace HTP.App.Auth.Login;

public sealed record LoginUserResponse(AuthTokens AuthTokens, LoggedUserDto User);
