namespace HTP.Infrastructure.Persistence.Users.Read;

public sealed record UserReadModel(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    bool IsBlocked,
    DateTime CreatedAt,
    DateTimeOffset? ActivatedAt);