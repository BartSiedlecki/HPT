using HTP.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HTP.Infrastructure.Persistence.Permissions.Converters;

internal sealed class PermissionIdConvereter : ValueConverter<PermissionId, string> 
{
    public PermissionIdConvereter() : base(
        id => id.Value,
        value => PermissionId.Create(value).Value)
    {
    }
}
