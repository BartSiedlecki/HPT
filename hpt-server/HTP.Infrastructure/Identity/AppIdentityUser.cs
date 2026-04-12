using Microsoft.AspNetCore.Identity;

namespace HTP.Infrastructure.Identity;

/// <summary>
/// Infrastructure Identity User - used for authentication and authorization purposes.
/// It is linked to the domain user via the DomainUserId property.
/// </summary>
public class AppIdentityUser : IdentityUser<Guid>
{
    public Guid DomainUserId { get; set; }
}
