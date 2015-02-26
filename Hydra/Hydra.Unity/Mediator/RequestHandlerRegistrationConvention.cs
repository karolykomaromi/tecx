namespace Hydra.Unity.Mediator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Hydra.Infrastructure.Mediator;
    using Hydra.Infrastructure.Reflection;
    using Microsoft.Practices.Unity;

    public class RequestHandlerRegistrationConvention : RegistrationConvention
    {
        public override IEnumerable<Type> GetTypes()
        {
            Type[] types = AllTypes
                .FromHydraAssemblies()
                .Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(IRequestHandler<,>)) &&
                            !TypeHelper.IsOpenGeneric(t))
                .ToArray();

            return types;
        }

        public override Func<Type, IEnumerable<Type>> GetFromTypes()
        {
            return WithMappings.FromAllInterfaces;
        }

        public override Func<Type, string> GetName()
        {
            // weberse 2015-02-26 there should be exactly one handler per request type and response. more than one implementation
            // should cause an exception
            return _ => string.Empty;
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