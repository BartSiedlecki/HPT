
using FluentValidation;
using HPT.SharedKernel.Constants;

namespace HTP.App.Core.Settings;

public sealed class SeederSettingsValidator : AbstractValidator<SeederSettings>
{
    public SeederSettingsValidator()
    {
        RuleFor(x => x.AdminLogin)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.AdminPassword)
            .NotEmpty()
            .MinimumLength(FieldLengths.Password.MinLength)
            .MaximumLength(FieldLengths.Password.MaxLength)
            .Matches(RegexPatterns.Password);
    }
}
