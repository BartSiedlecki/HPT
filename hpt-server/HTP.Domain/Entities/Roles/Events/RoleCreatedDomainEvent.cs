using HPT.SharedKernel.Abstractions;
using HTP.Domain.Entities.Roles.ValueObjects;

namespace HTP.Domain.Entities.Roles.Events;

public sealed record RoleCreatedDomainEvent(Guid RoleId, RoleName RoleName) : IDomainEvent;
