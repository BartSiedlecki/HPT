using HTP.App.Core.Abstractions.Mediator;

namespace HTP.App.Auth.Login;

public record LoginUserCommand(
    string Login,
    string Password) : ICommand<LoginUserResponse>;
