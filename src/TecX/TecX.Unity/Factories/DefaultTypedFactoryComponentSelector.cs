namespace TecX.Unity.Factories
{
    using System;
    using System.Reflection;

    using Microsoft.Practices.Unity;

    using TecX.Common;
    using TecX.Common.Extensions.Primitives;

    public class DefaultTypedFactoryComponentSelector : ITypedFactoryComponentSelector
    {
        public TypedFactoryComponent SelectComponent(MethodInfo method, Type type, object[] arguments)
        {
            Guard.AssertNotNull(method, "method");
            Guard.AssertNotNull(arguments, "arguments");

            var componentName = this.GetComponentName(method, arguments);
            var componentType = this.GetComponentType(method, arguments);
            var additionalArguments = this.GetArguments(method, arguments);

            return this.BuildFactoryComponent(method, componentName, componentType, additionalArguments);
        }

        protected virtual TypedFactoryComponent BuildFactoryComponent(
            MethodInfo method,
            string nameToBuild,
            Type typeToBuild,
            ResolverOverrides additionalArguments)
        {
            Type itemType;
            if (typeToBuild.TryGetCompatibleCollectionItemType(out itemType))
            {
                TypedFactoryComponent tfc = (TypedFactoryComponent)Activator.CreateInstance(
                                                typeof(TypedFactoryComponentCollection<>).MakeGenericType(itemType),
                                                typeToBuild,
                                                additionalArguments);

                return tfc;
            }

            return new TypedFactoryComponent(typeToBuild, nameToBuild, additionalArguments);
        }

        protected virtual ResolverOverrides GetArguments(MethodInfo method, object[] arguments)
        {
            Guard.AssertNotNull(method, "method");
            Guard.AssertNotNull(arguments, "arguments");

            var parameters = new ResolverOverrides();

            var input = method.GetParameters();

            for (int i = 0; i < input.Length; i++)
            {
                parameters.Add(new ParameterOverride(input[i].Name, arguments[i]));

                string propertyName = input[i].Name.ToUpper(0, 1);

                parameters.Add(new PropertyOverride(propertyName, arguments[i]));
            }

            return parameters;
        }

        protected virtual string GetComponentName(MethodInfo method, object[] arguments)
        {
            Guard.AssertNotNull(method, "method");

            string componentName = null;

            if (method.Name.StartsWith("Get"))
            {
                componentName = method.Name.Substring("Get".Length);
            }

            return componentName;
        }

        protected virtual Type GetComponentType(MethodInfo method, object[] arguments)
        {
            Guard.AssertNotNull(method, "method");

            return method.ReturnType;
        }
    }
}