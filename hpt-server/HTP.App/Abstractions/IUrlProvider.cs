
namespace HTP.App.Abstractions;

public interface IUrlProvider
{
    string GetSetUserPasswordUrl(Guid userId, string encodedToken);
}
