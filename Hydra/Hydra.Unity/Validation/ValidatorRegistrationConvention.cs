namespace Hydra.Unity.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentValidation;
    using Hydra.Infrastructure.Reflection;
    using Microsoft.Practices.Unity;

    public class ValidatorRegistrationConvention : RegistrationConvention
    {
        public override IEnumerable<Type> GetTypes()
        {
            return AllTypes.FromHydraAssemblies()
                .Where(t => TypeHelper.ImplementsOpenGenericInterface(t, typeof(IValidator<>)));
        }

        public override Func<Type, IEnumerable<Type>> GetFromTypes()
        {
            return type => type.GetInterfaces().Where(i => TypeHelper.IsClosedVersionOfOpenGeneric(i, typeof(IValidator<>)));
        }

        public override Func<Type, string> GetName()
        {
            return WithName.Default;
        }

        public override Func<Type, LifetimeManager> GetLifetimeManager()
        {
            return WithLifetime.Transient;
        }

        public override Func<Type, IEnumerable<InjectionMember>> GetInjectionMembers()
        {
            return WithInjectionMembers.None;
        }
    }
}