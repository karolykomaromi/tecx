namespace Hydra.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using NHibernate.Bytecode;

    public class UnityObjectsFactory : IObjectsFactory
    {
        private readonly IUnityContainer container;

        public UnityObjectsFactory(IUnityContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public object CreateInstance(Type type)
        {
            return this.container.Resolve(type);
        }

        public object CreateInstance(Type type, bool nonPublic)
        {
            return this.container.Resolve(type);
        }

        public object CreateInstance(Type type, params object[] ctorArgs)
        {
            var parameters =
                (ctorArgs ?? Enumerable.Empty<object>()).Select(arg => new ParameterByTypeResolverOverride(arg)).ToArray<ResolverOverride>();

            return this.container.Resolve(type, parameters);
        }
    }
}