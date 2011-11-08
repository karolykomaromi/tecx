namespace TecX.Unity.Groups
{
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class SemanticGroupStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            IPolicyList resolverPolicyDestination;
            IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);

            var ctor = selector.SelectConstructor(context, resolverPolicyDestination);

            if (ctor == null)
            {
                return;
            }

            ISemanticGroupPolicy policy = context.Policies.Get<ISemanticGroupPolicy>(context.BuildKey);

            if (policy == null)
            {
                return;
            }

            var parameters = ctor.Constructor.GetParameters();

            var parameterKeys = ctor.GetParameterKeys();

            for (int i = 0; i < parameters.Length; i++)
            {
                var @using = policy.Usings.FirstOrDefault(u => u.From == parameters[i].ParameterType);

                if (@using != null)
                {
                    var resolverPolicy = new NamedTypeDependencyResolverPolicy(@using.From, @using.Name);

                    context.Policies.Set<IDependencyResolverPolicy>(resolverPolicy, parameterKeys[i]);
                }
            }

            resolverPolicyDestination.Set<IConstructorSelectorPolicy>(
                new ReadonlyConstructorSelectorPolicy(ctor), context.BuildKey);
        }
    }
}