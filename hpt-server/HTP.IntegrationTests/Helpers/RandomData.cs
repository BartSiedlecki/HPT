using HTP.Domain.ValueObjects;

namespace HTP.IntegrationTests.Helpers;

internal static class RandomData
{
    public static Email UniqueEmail => Email.Create($"jan.{Guid.NewGuid():N}@mail.com").Value;
}
