namespace HTP.App.Users.Dtos;

public sealed record LoggedUserDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    IEnumerable<string> Roles,
    IEnumerable<string> Permissions);
