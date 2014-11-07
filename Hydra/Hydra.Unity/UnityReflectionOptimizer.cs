namespace Hydra.Unity
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;
    using NHibernate.Bytecode.Lightweight;
    using NHibernate.Properties;

    public class UnityReflectionOptimizer : ReflectionOptimizer
    {
        private readonly IUnityContainer container;

        public UnityReflectionOptimizer(IUnityContainer container, Type mappedType, IGetter[] getters, ISetter[] setters)
            : base(mappedType, getters, setters)
        {
            Contract.Requires(container != null);

            this.container = container;
        }

        public override object CreateInstance()
        {
            return this.container.Resolve(this.mappedType);
        }

        protected override void ThrowExceptionForNoDefaultCtor(Type type)
        {
        }
    }
}