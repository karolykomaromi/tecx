namespace Hydra.Unity.Infrastructure.Reflection
{
    using System;
    using System.Collections.Generic;
    using Hydra.Infrastructure.Reflection;
    using Microsoft.Practices.Unity.InterceptionExtension;

    public class UnityDuckTypeGenerator : IDuckTypeGenerator
    {
        public T Duck<T>(object duck) where T : class
        {
            IEnumerable<IInterceptionBehavior> interceptionBehaviors = new[] { new StrictDuckTypeBehavior(duck) };

            IEnumerable<Type> additionalInterfaces = new[] { typeof(T) };

            return (T)Intercept.NewInstanceWithAdditionalInterfaces(
                typeof(object),
                new VirtualMethodInterceptor(),
                interceptionBehaviors,
                additionalInterfaces);
        }
    }
}