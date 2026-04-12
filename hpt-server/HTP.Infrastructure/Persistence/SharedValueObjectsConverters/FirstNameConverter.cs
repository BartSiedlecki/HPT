using HTP.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace HTP.Infrastructure.Persistence.SharedValueObjectsConverters;

internal sealed class FirstNameConverter : ValueConverter<FirstName, string>
{
    public FirstNameConverter() : base(
        firstName => firstName.Value,
        value => FirstName.Create(value).Value)
    {
    }
}
