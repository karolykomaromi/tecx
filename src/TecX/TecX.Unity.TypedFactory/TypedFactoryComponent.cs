namespace TecX.Unity.TypedFactory
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class TypedFactoryComponent
    {
        private readonly string nameToBuild;

        private readonly Type typeToBuild;

        private readonly ParameterOverrides additionalArguments;

        public TypedFactoryComponent(Type typeToBuild, string nameToBuild, ParameterOverrides additionalArguments)
            : this(typeToBuild, additionalArguments)
        {
            if (nameToBuild == string.Empty)
            {
                nameToBuild = null;
            }

            this.nameToBuild = nameToBuild;
        }

        protected TypedFactoryComponent(Type typeToBuild, ParameterOverrides additionalArguments)
        {
            Guard.AssertNotNull(typeToBuild, "typeToBuild");
            Guard.AssertNotNull(additionalArguments, "additionalArguments");

            this.typeToBuild = typeToBuild;
            this.additionalArguments = additionalArguments;
        }

        public string NameToBuild
        {
            get
            {
                return this.nameToBuild;
            }
        }

        public Type TypeToBuild
        {
            get
            {
                return this.typeToBuild;
            }
        }

        public ParameterOverrides AdditionalArguments
        {
            get
            {
                return this.additionalArguments;
            }
        }

        public virtual object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            object resolved = container.Resolve(this.TypeToBuild, this.NameToBuild, this.AdditionalArguments);

            return resolved;
        }
    }
}