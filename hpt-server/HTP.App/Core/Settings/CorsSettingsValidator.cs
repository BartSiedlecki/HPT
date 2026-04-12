
using FluentValidation;

namespace HTP.App.Core.Settings;

public sealed class CorsSettingsValidator : AbstractValidator<CorsSettings>
{
    public CorsSettingsValidator()
    {
        RuleFor(x => x.AllowedOrigins)
            .NotEmpty()
            .Must(x => x.Length > 0)
            .WithMessage("At least one CORS origin must be specified.");
    }
}
