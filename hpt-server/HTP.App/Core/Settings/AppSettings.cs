
namespace HTP.App.Core.Settings;

public class AppSettings
{
    public const string SectionName = "App";
    public string AppName { get; init; } = null!;
    public string ApiFullAddress { get; init; } = null!;
    public string ClientFullAddress { get; init; } = null!;
}
