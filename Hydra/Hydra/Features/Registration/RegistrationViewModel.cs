namespace Hydra.Features.Registration
{
    using FluentValidation.Attributes;

    [Validator(typeof(RegistrationViewModelValidator))]
    public class RegistrationViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool? RememberMe { get; set; }
    }
}