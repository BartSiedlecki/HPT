using HPT.SharedKernel.Abstractions;
using HTP.Domain.Entities.Roles.ValueObjects;
using HTP.Domain.ValueObjects;

namespace HTP.Domain.Entities.Roles.Events;

public sealed record RolePermissionsChangedDomainEvents(Guid RoleId, RoleName RoleName, PermissionId PermissionId) : IDomainEvent;
