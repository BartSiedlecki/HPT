using HTP.Domain.Entities.Roles.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HTP.Infrastructure.Persistence.SharedValueObjectsConverters;

internal class RoleNameConverter : ValueConverter<RoleName, string>
{
    public RoleNameConverter() : base(
        v => v.Value,
        v => RoleName.Create(v).Value)
    {
    }
}
