using System;
using System.Reflection;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.TypedFactory
{
    public class DefaultTypedFactoryComponentSelector : ITypedFactoryComponentSelector
    {
        public TypedFactoryComponent SelectComponent(MethodInfo method, Type type, object[] arguments)
        {
            Guard.AssertNotNull(method, "method");
            Guard.AssertNotNull(arguments, "arguments");

            var componentName = GetComponentName(method, arguments);
            var componentType = GetComponentType(method, arguments);
            var additionalArguments = GetArguments(method, arguments);

            return BuildFactoryComponent(method, componentName, componentType, additionalArguments);
        }

        protected virtual TypedFactoryComponent BuildFactoryComponent(MethodInfo method,
                                                                      string nameToBuild,
                                                                      Type typeToBuild,
                                                                      ParameterOverrides additionalArguments)
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

        protected virtual ParameterOverrides GetArguments(MethodInfo method, object[] arguments)
        {
            var parameters = new ParameterOverrides();

            var input = method.GetParameters();

            for (int i = 0; i < input.Length; i++)
            {
                parameters.Add(input[i].Name, arguments[i]);
            }

            return parameters;
        }

        protected virtual string GetComponentName(MethodInfo method, object[] arguments)
        {
            string componentName = null;

            if (method.Name.StartsWith("Get"))
            {
                componentName = method.Name.Substring("Get".Length);
            }

            return componentName;
        }

        protected virtual Type GetComponentType(MethodInfo method, object[] arguments)
        {
            return method.ReturnType;
        }
    }
}