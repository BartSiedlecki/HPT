using HPT.SharedKernel.Abstractions;
using HTP.Domain.ValueObjects;

namespace HTP.Domain.Entities.Users;

public record UserCreatedDomainEvent(Guid UserId, string Email) : IDomainEvent;

public record UserProfileUpdatedDomainEvent(Guid UserId, FirstName FirstName, LastName LastName) : IDomainEvent;

public record UserDeactivatedDomainEvent(Guid UserId) : IDomainEvent;

public record UserActivatedDomainEvent(Guid UserId) : IDomainEvent;

public record UserPasswordChangedDomainEvent(Guid UserId) : IDomainEvent;
