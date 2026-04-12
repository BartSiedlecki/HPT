using HTP.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HTP.Infrastructure.Persistence.SharedValueObjectsConverters;

internal sealed class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter() : base(
        email => email.Value,
        value => Email.Create(value).Value)
    {
    }
}
