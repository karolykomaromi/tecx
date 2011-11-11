namespace TecX.Unity.Groups
{
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
                ScopedMapping scopedMapping = policy.Usings.FirstOrDefault(u => u.From == parameters[i].ParameterType);

                if (scopedMapping != null)
                {
                    var resolverPolicy = new NamedTypeDependencyResolverPolicy(scopedMapping.From, scopedMapping.Name);

                    context.Policies.Set<IDependencyResolverPolicy>(resolverPolicy, parameterKeys[i]);
                }
            }

            resolverPolicyDestination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
        }
    }
}