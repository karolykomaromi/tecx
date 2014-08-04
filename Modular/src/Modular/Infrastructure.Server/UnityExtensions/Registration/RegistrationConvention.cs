namespace Infrastructure.UnityExtensions.Registration
{
    using System;
    using Microsoft.Practices.Unity;

    public abstract class RegistrationConvention : IRegistrationConvention
    {
        public void RegisterOnMatch(IUnityContainer container, Type type)
        {
            if (this.IsMatch(type))
            {
                this.Register(container, type);
            }
        }

        protected abstract void Register(IUnityContainer container, Type type);

        protected abstract bool IsMatch(Type type);
    }
}