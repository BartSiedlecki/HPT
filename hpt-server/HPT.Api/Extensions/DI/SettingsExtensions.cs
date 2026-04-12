using HTP.App.Core.Settings;

namespace HPT.Api.Extensions.DI;

public static class SettingsExtensions
{
    public static void AddSettings(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<AppSettings>()
          .BindConfiguration(AppSettings.SectionName)
          .ValidateFluently()
          .ValidateOnStart();

        builder.Services.AddOptions<CorsSettings>()
          .BindConfiguration(CorsSettings.SectionName)
          .ValidateFluently()
          .ValidateOnStart();

        builder.Services.AddOptions<JwtSettings>()
          .BindConfiguration(JwtSettings.SectionName)
          .ValidateFluently()
          .ValidateOnStart();

        builder.Services.AddOptions<SeederSettings>()
          .BindConfiguration(SeederSettings.SectionName)
          .ValidateFluently()
          .ValidateOnStart();

    }
}
