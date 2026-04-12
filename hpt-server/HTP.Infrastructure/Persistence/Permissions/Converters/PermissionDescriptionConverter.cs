using HTP.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HTP.Infrastructure.Persistence.Permissions.Converters;

internal sealed class PermissionDescriptionConverter : ValueConverter<PermissionDescription?, string?>
{
    public PermissionDescriptionConverter()
        : base(
            description => description == null ? null : description.Value,
            value => PermissionDescription.Create(value).Value)
    {
    }
}
