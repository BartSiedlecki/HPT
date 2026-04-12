using HTP.App.Auth.Login;
using HTP.App.Core.Abstractions.Mediator;

namespace HTP.App.Users.Commands.Refresh;

public sealed record RefreshUserCommand(string RefreshToken)
    : ICommand<LoginUserResponse>;