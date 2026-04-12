
using HTP.App.Core.Abstractions.Mediator;

namespace HTP.App.Auth.Register;

public sealed record RegisterUserCommand(string Login): ICommand;
