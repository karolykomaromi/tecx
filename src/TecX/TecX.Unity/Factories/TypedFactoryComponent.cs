namespace TecX.Unity.Factories
{
    using System;
    using Microsoft.Practices.Unity;
    using TecX.Common;

    public class TypedFactoryComponent
    {
        private readonly string nameToBuild;

        private readonly Type typeToBuild;

        private readonly ResolverOverride[] additionalArguments;

        public TypedFactoryComponent(Type typeToBuild, string nameToBuild, ResolverOverride[] additionalArguments)
            : this(typeToBuild, additionalArguments)
        {
            if (nameToBuild == string.Empty)
            {
                nameToBuild = null;
            }

            this.nameToBuild = nameToBuild;
        }

        protected TypedFactoryComponent(Type typeToBuild, ResolverOverride[] additionalArguments)
        {
            Guard.AssertNotNull(typeToBuild, "typeToBuild");
            Guard.AssertNotNull(additionalArguments, "additionalArguments");

            this.typeToBuild = typeToBuild;
            this.additionalArguments = additionalArguments;
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
            Guard.AssertNotNull(container, "container");

            object resolved = container.Resolve(this.TypeToBuild, this.NameToBuild, this.AdditionalArguments);

            return resolved;
        }
    }
}