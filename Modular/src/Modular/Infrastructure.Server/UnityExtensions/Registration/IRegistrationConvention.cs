namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using Microsoft.Practices.Unity;

    public interface IRegistrationConvention
    {
        void RegisterOnMatch(IUnityContainer container, Type type);
    }
}