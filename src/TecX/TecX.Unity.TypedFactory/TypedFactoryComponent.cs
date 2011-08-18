using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class TypedFactoryComponent
    {
        private readonly string _componentName;

        private readonly Type _componentType;

        private readonly ParameterOverrides _additionalArguments;

        public string ComponentName
        {
            get
            {
                return _componentName;
            }
        }

        public Type ComponentType
        {
            get
            {
                return _componentType;
            }
        }

        public ParameterOverrides AdditionalArguments
        {
            get
            {
                return _additionalArguments;
            }
        }

        public TypedFactoryComponent(string componentName, Type componentType, ParameterOverrides additionalArguments)
        {
            Guard.AssertNotNull(componentType, "componentType");
            Guard.AssertNotNull(additionalArguments, "additionalArguments");

            if(componentName == string.Empty)
            {
                componentName = null;
            }

            _componentName = componentName;
            _componentType = componentType;
            _additionalArguments = additionalArguments;
        }

        public virtual object Resolve(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            object resolved = container.Resolve(ComponentType, ComponentName, AdditionalArguments);

            return resolved;
        }
    }
}