
namespace HTP.App.Core.Settings;

public class CorsSettings
{
    public const string SectionName = "Cors";

    public required string[] AllowedOrigins { get; set; }
}
