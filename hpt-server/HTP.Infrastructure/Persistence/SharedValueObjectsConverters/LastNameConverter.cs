using HTP.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HTP.Infrastructure.Persistence.SharedValueObjectsConverters;

internal sealed class LastNameConverter : ValueConverter<LastName, string>
{
    public LastNameConverter() : base(
        lastName => lastName.Value,
        value => LastName.Create(value).Value)
    {
    }
}
