namespace Infrastructure.UnityExtensions.Factories
{
    using System;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;

    public class TypedFactoryComponent
    {
        private readonly string nameToBuild;

        private readonly Type typeToBuild;

        private readonly ResolverOverride[] additionalArguments;

        public TypedFactoryComponent(Type typeToBuild, string nameToBuild, params ResolverOverride[] additionalArguments)
            : this(typeToBuild, additionalArguments)
        {
            if (nameToBuild == string.Empty)
            {
                nameToBuild = null;
            }

            this.nameToBuild = nameToBuild;
        }

        protected TypedFactoryComponent(Type typeToBuild, params ResolverOverride[] additionalArguments)
        {
            this.typeToBuild = typeToBuild;
            this.additionalArguments = additionalArguments ?? new ResolverOverride[0];
        }

        public string NameToBuild
        {
            get { return this.nameToBuild; }
        }

        public Type TypeToBuild
        {
            get { return this.typeToBuild; }
        }

        public ResolverOverride[] AdditionalArguments
        {
            get { return this.additionalArguments; }
        }

        public virtual object Resolve(IUnityContainer container)
        {
            Contract.Requires(container != null);

            object resolved = container.Resolve(this.TypeToBuild, this.NameToBuild, this.AdditionalArguments);

            return resolved;
        }
    }
}