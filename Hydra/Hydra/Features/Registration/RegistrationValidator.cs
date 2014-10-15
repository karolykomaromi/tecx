namespace Hydra.Features.Registration
{
    using FluentValidation;

    public class RegistrationValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationValidator()
        {
            this.RuleFor(x => x.Email).NotNull().EmailAddress();
            this.RuleFor(x => x.Password).NotNull().Length(6, int.MaxValue);
            this.RuleFor(x => x.ConfirmPassword).NotNull().Length(6, int.MaxValue).Equal(x => x.Password);
        }
    }
}