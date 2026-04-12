using HTP.App.Core.Abstractions.Mediator;

namespace HTP.App.Auth.CreateByAdmin;

public record CreateUserByAdminCommand(
    string FirstName,
    string LastName,
    string Email,
    IEnumerable<string> Roles) : ICommand<CreateUserByAdminResponse>;
