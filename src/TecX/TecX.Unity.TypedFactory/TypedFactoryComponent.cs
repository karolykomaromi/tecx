using System;
using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class TypedFactoryComponent
    {
        private readonly string _nameToBuild;

        private readonly Type _typeToBuild;

        private readonly ParameterOverrides _additionalArguments;

        public string NameToBuild
        {
            get
            {
                return _nameToBuild;
            }
        }

        public Type TypeToBuild
        {
            get
            {
                return _typeToBuild;
            }
        }

        public ParameterOverrides AdditionalArguments
        {
            get
            {
                return _additionalArguments;
            }
        }

        public TypedFactoryComponent(Type typeToBuild, string nameToBuild, ParameterOverrides additionalArguments)
            : this(typeToBuild, additionalArguments)
        {
            if (nameToBuild == string.Empty)
            {
                nameToBuild = null;
            }

            this._nameToBuild = nameToBuild;
        }

        protected TypedFactoryComponent(Type typeToBuild, ParameterOverrides additionalArguments)
        {
            Guard.AssertNotNull(typeToBuild, "typeToBuild");
            Guard.AssertNotNull(additionalArguments, "additionalArguments");

            this._typeToBuild = typeToBuild;
            _additionalArguments = additionalArguments;
        }

        public virtual object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            object resolved = container.Resolve(this.TypeToBuild, this.NameToBuild, AdditionalArguments);

            return resolved;
        }
    }
}