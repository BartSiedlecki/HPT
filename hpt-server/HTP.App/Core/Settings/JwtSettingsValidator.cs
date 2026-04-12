
using FluentValidation;

namespace HTP.App.Core.Settings;

public sealed class JwtSettingsValidator : AbstractValidator<JwtSettings>
{
    public JwtSettingsValidator()
    {
        RuleFor(x => x.SecretKey)
            .NotEmpty();

        RuleFor(x => x.Issuer)
            .NotEmpty();

        RuleFor(x => x.Audience)
            .NotEmpty();

        RuleFor(x => x.ExpirationMinutes)
            .GreaterThan(0);

        RuleFor(x => x.RefreshTokenExpirationMinutes)
            .GreaterThan(0);
    }
}
