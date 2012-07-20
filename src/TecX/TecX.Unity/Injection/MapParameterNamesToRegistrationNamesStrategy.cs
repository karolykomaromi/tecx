namespace TecX.Unity.Injection
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class MapParameterNamesToRegistrationNamesStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.BuildKey, "context.BuildKey");
            Guard.AssertNotNull(context.Policies, "context.Policies");

            if (context.Policies.Get<IMapParameterNameToRegistrationNamePolicy>(context.BuildKey) == null)
            {
                return;
            }

            IPolicyList resolverPolicyDestination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);

            var selectedConstructor = selector.SelectConstructor(context, resolverPolicyDestination);

            if (selectedConstructor == null)
            {
                return;
            }

            var parameters = selectedConstructor.Constructor.GetParameters();

            var parameterKeys = selectedConstructor.GetParameterKeys();

            for (int i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;

                if (parameterType.IsAbstract || parameterType.IsInterface)
                {
                    IDependencyResolverPolicy resolverPolicy = new NamedTypeDependencyResolverPolicy(parameterType, parameters[i].Name);
                    context.Policies.Set<IDependencyResolverPolicy>(resolverPolicy, parameterKeys[i]);
                }
            }

            resolverPolicyDestination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
        }
    }
}