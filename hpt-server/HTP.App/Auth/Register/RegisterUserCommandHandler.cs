using HPT.SharedKernel;
using HTP.App.Core.Abstractions.Mediator;

namespace HTP.App.Auth.Register;

internal class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    public Task<Result> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
