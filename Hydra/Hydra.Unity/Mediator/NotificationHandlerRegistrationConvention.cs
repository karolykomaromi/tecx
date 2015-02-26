namespace Hydra.Unity.Mediator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Infrastructure.Reflection;
    using Microsoft.Practices.Unity;

    public class NotificationHandlerRegistrationConvention : RegistrationConvention
    {
        public override IEnumerable<Type> GetTypes()
        {
            return AllTypes.FromHydraAssemblies()
                .Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(INotificationHandler<>)) &&
                            !TypeHelper.IsOpenGeneric(t));
        }

        public override Func<Type, IEnumerable<Type>> GetFromTypes()
        {
            return WithMappings.FromAllInterfaces;
        }

        public override Func<Type, string> GetName()
        {
            return t => t.Name.Replace("Notification", string.Empty).Replace("Handler", string.Empty);
        }

        public override Func<Type, LifetimeManager> GetLifetimeManager()
        {
            return WithLifetime.Transient;
        }

        public override Func<Type, IEnumerable<InjectionMember>> GetInjectionMembers()
        {
            return t => new InjectionMember[0];
        }
    }
}