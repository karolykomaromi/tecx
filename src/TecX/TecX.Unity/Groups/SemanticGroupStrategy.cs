namespace TecX.Unity.Groups
{
    using System;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class SemanticGroupStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            IPolicyList resolverPolicyDestination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);

            var selectedConstructor = selector.SelectConstructor(context, resolverPolicyDestination);

            if (selectedConstructor == null)
            {
                return;
            }

            ISemanticGroupPolicy policy = context.Policies.Get<ISemanticGroupPolicy>(context.BuildKey);

            if (policy == null)
            {
                return;
            }

            var parameters = selectedConstructor.Constructor.GetParameters();

            var parameterKeys = selectedConstructor.GetParameterKeys();

            for (int i = 0; i < parameters.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;

                ScopedMapping scopedMapping = policy.ScopedMappings.FirstOrDefault(sm => sm.From == parameterType);

                if (scopedMapping != null)
                {
                    IDependencyResolverPolicy resolverPolicy = new NamedTypeDependencyResolverPolicy(scopedMapping.From, scopedMapping.Name);
                    context.Policies.Set<IDependencyResolverPolicy>(resolverPolicy, parameterKeys[i]);
                    continue;
                }

                if (parameterType.IsGenericType)
                {
                    Type openGeneric = parameterType.GetGenericTypeDefinition();

                    scopedMapping = policy.ScopedMappings.FirstOrDefault(sm => sm.From == openGeneric);

                    if (scopedMapping != null)
                    {
                        Type closedGeneric = scopedMapping.To.MakeGenericType(parameterType.GetGenericArguments());

                        IDependencyResolverPolicy resolverPolicy = new NamedTypeDependencyResolverPolicy(closedGeneric, scopedMapping.Name);
                        context.Policies.Set<IDependencyResolverPolicy>(resolverPolicy, parameterKeys[i]);
                    }
                }
            }

            resolverPolicyDestination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
        }
    }
}