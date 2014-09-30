namespace Hydra.Models
{
    using FluentValidation.Attributes;

    [Validator(typeof(RegistrationValidator))]
    public class Registration
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public bool? RememberMe { get; set; }
    }
}