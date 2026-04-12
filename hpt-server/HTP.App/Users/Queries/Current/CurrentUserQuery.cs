using HTP.App.Core.Abstractions.Mediator;
using HTP.App.Users.Dtos;

namespace HTP.App.Users.Queries.Current;

public sealed record CurrentUserQuery : IQuery<LoggedUserDto>;
