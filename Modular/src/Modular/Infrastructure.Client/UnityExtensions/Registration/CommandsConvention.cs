namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using System.Windows.Input;
    using Microsoft.Practices.Unity;

    public class CommandsConvention : RegistrationConvention
    {
        protected override bool IsMatch(Type type)
        {
            return typeof(ICommand).IsAssignableFrom(type);
        }

        protected override void Register(IUnityContainer container, Type type)
        {
            container.RegisterType(typeof(ICommand), type, StringHelper.FirstCharacterToLowerInvariant(type.Name));
        }
    }
}