namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using Infrastructure.Options;
    using Microsoft.Practices.Unity;

    public class OptionsConvention : RegistrationConvention
    {
        protected override void Register(IUnityContainer container, Type type)
        {
            container.RegisterType(typeof(IOptions), type, StringHelper.FirstCharacterToLowerInvariant(type.Name), new ContainerControlledLifetimeManager());
        }

        protected override bool IsMatch(Type type)
        {
            return typeof(IOptions).IsAssignableFrom(type);
        }
    }
}
