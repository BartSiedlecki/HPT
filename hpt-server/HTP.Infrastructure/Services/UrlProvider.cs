using HTP.App.Abstractions;
using HTP.App.Core.Settings;
using Microsoft.Extensions.Options;

namespace HTP.Infrastructure.Services;

internal sealed class UrlProvider(IOptions<AppSettings> appSettings) : IUrlProvider
{
    public string GetSetUserPasswordUrl(Guid userId, string encodedToken)
    {
        return $"{appSettings.Value.ClientFullAddress}/auth/reset-password?userId={userId}&token={encodedToken}";
    }
}
