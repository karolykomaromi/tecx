namespace Hydra.Features.Registration
{
    using FluentValidation;

    public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
    {
        public RegistrationViewModelValidator()
        {
            this.RuleFor(x => x.Email).NotNull().EmailAddress();
            this.RuleFor(x => x.Password).NotNull().Length(6, int.MaxValue);
            this.RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(Properties.Resources.PasswordNotEqual);
        }
    }
}