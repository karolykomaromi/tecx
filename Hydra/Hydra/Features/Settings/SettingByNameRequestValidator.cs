namespace Hydra.Features.Settings
{
    using FluentValidation;

    public class SettingByNameRequestValidator : AbstractValidator<SettingByNameRequest>
    {
        public SettingByNameRequestValidator()
        {
            this.RuleFor(x => x.Name).NotEmpty();
        }
    }
}