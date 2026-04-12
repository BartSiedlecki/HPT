using FluentValidation;

namespace HTP.App.Core.Settings;

public class AppSettingsValidator : AbstractValidator<AppSettings>
{
    public AppSettingsValidator()
    {
        RuleFor(settings => settings.AppName)
            .NotEmpty();

        RuleFor(settings => settings.ApiFullAddress)
            .NotEmpty();

        RuleFor(settings => settings.ClientFullAddress)
            .NotEmpty();
    }
}