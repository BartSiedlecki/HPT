namespace HPT.SharedKernel.Abstractions;

/// <summary>
/// Service for generating application URLs (e.g., password reset links).
/// </summary>
public interface IUrlProvider
{
    string GetSetPasswordUrl(Guid userId, string token);
    string GetEmailConfirmationUrl(Guid userId, string token);
}
