namespace Infrastructure.UnityExtensions.Injection
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class MapToRegistrationNamesStrategy : BuilderStrategy
    {
        public static bool IsClassNotStringAndTypeNameDoesntStartWithParamName(Type parameterType, string paramName)
        {
            bool isMatch = parameterType.IsClass && parameterType != typeof(string) && !parameterType.Name.StartsWith(paramName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }

        public static bool IsInterfaceAndTypeNameDoesntStartWithIPlusParamName(Type parameterType, string paramName)
        {
            bool isMatch = parameterType.IsInterface && !parameterType.Name.StartsWith("I" + paramName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }

        public static bool IsAbstractAndTypeNameDoesntStartWithParamName(Type parameterType, string paramName)
        {
            bool isMatch = !parameterType.IsInterface && parameterType.IsAbstract && !parameterType.Name.StartsWith(paramName, StringComparison.OrdinalIgnoreCase);

            return isMatch;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.BuildKey != null);
            Contract.Requires(context.Policies != null);

            if (context.Policies.Get<IMapToRegistrationNamePolicy>(context.BuildKey) == null)
            {
                return;
            }

            IPolicyList resolverPolicyDestination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);

            if (selector == null)
            {
                return;
            }

            var selectedConstructor = selector.SelectConstructor(context, resolverPolicyDestination);

            if (selectedConstructor == null)
            {
                return;
            }

            ParameterInfo[] parameters = selectedConstructor.Constructor.GetParameters();

            string[] parameterKeys = selectedConstructor.GetParameterKeys();

            for (int i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;
                string paramName = parameters[i].Name;

                if (IsAbstractAndTypeNameDoesntStartWithParamName(parameterType, paramName) ||
                    IsInterfaceAndTypeNameDoesntStartWithIPlusParamName(parameterType, paramName) ||
                    IsClassNotStringAndTypeNameDoesntStartWithParamName(parameterType, paramName))
                {
                    IDependencyResolverPolicy resolverPolicy = new NamedTypeDependencyResolverPolicy(parameterType, parameters[i].Name);
                    context.Policies.Set<IDependencyResolverPolicy>(resolverPolicy, parameterKeys[i]);
                }
            }

            resolverPolicyDestination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
        }
    }
}