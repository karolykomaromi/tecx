namespace Hydra.Test.Composition
{
    using FluentValidation;
    using Hydra.Composition;
    using Hydra.Features.Registration;
    using Microsoft.Practices.Unity;
    using Xunit;

    public class FluentValidationConfigurationTests
    {
        [Fact]
        public void Should_Resolve_Correct_Validator()
        {
            IUnityContainer container = new UnityContainer().AddNewExtension<FluentValidationConfiguration>();

            IValidatorFactory sut = container.Resolve<IValidatorFactory>();

            IValidator<RegistrationViewModel> actual = sut.GetValidator<RegistrationViewModel>();

            Assert.NotNull(actual);

            Assert.IsType<RegistrationViewModelValidator>(actual);
        }
    }
}
