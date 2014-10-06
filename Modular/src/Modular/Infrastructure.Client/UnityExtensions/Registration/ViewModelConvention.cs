namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using Infrastructure.Options;
    using Infrastructure.UnityExtensions.Injection;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.Unity;

    public class ViewModelConvention : RegistrationConvention
    {
        protected override void Register(IUnityContainer container, Type type)
        {
            container.RegisterType(type, new SmartConstructor());
        }

        protected override bool IsMatch(Type type)
        {
            bool isMatch = typeof(ViewModel).IsAssignableFrom(type) &&
                           type != typeof(ViewModel) &&
                           !typeof(IOptions).IsAssignableFrom(type);

            return isMatch;
        }
    }
}