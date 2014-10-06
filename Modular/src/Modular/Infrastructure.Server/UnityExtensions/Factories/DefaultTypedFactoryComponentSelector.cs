namespace Infrastructure.UnityExtensions.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Microsoft.Practices.Unity;

    public class DefaultTypedFactoryComponentSelector : ITypedFactoryComponentSelector
    {
        public TypedFactoryComponent SelectComponent(MethodInfo method, Type type, object[] arguments)
        {
            Contract.Requires(method != null);
            Contract.Requires(type != null);

            var componentName = this.GetComponentName(method, arguments);
            var componentType = this.GetComponentType(method, arguments);
            var additionalArguments = this.GetArguments(method, arguments);

            return this.BuildFactoryComponent(method, componentName, componentType, additionalArguments);
        }

        protected virtual TypedFactoryComponent BuildFactoryComponent(MethodInfo method, string nameToBuild, Type typeToBuild, ResolverOverride[] additionalArguments)
        {
            Type itemType;
            if (TypeHelper.TryGetCompatibleCollectionItemType(typeToBuild, out itemType))
            {
                Type componentCollectionType = typeof(TypedFactoryComponentCollection<>).MakeGenericType(itemType);

                TypedFactoryComponent component = (TypedFactoryComponent)Activator.CreateInstance(componentCollectionType, typeToBuild, additionalArguments);

                return component;
            }

            return new TypedFactoryComponent(typeToBuild, nameToBuild, additionalArguments);
        }

        protected virtual ResolverOverride[] GetArguments(MethodInfo method, object[] arguments)
        {
            Contract.Requires(method != null);

            arguments = arguments ?? new object[0];

            List<ResolverOverride> parameters = new List<ResolverOverride>();

            var input = method.GetParameters();

            for (int i = 0; i < input.Length; i++)
            {
                parameters.Add(new ParameterOverride(input[i].Name, arguments[i]));

                string propertyName = StringHelper.FirstCharacterToUpperInvariant(input[i].Name);

                parameters.Add(new PropertyOverride(propertyName, arguments[i]));
            }

            return parameters.ToArray();
        }

        protected virtual string GetComponentName(MethodInfo method, object[] arguments)
        {
            Contract.Requires(method != null);

            string componentName = null;

            if (method.Name.StartsWith("Get"))
            {
                componentName = method.Name.Substring(3);
            }

            return componentName;
        }

        protected virtual Type GetComponentType(MethodInfo method, object[] arguments)
        {
            Contract.Requires(method != null);

            return method.ReturnType;
        }
    }
}