namespace TecX.Unity.Literals
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Unity.Groups;

    public class LiteralParametersStrategy : BuilderStrategy
    {
        private readonly ResolverConventionCollection conventions;

        public LiteralParametersStrategy(ResolverConventionCollection conventions)
        {
            this.conventions = conventions;
        }

        public override void PreBuildUp(IBuilderContext context)
        {
            IPolicyList resolverPolicyDestination;

            IConstructorSelectorPolicy ctorSelector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);
            SelectedConstructor selectedConstructor;

            if (ctorSelector != null &&
                (selectedConstructor = ctorSelector.SelectConstructor(context, resolverPolicyDestination)) != null)
            {
                var keys = selectedConstructor.GetParameterKeys();
                var parameters = selectedConstructor.Constructor.GetParameters();

                for (int i = 0; i < keys.Length; i++)
                {
                    var resolver = this.conventions.CreateResolver(context, parameters[i]);

                    if (resolver != null)
                    {
                        resolverPolicyDestination.Set<IDependencyResolverPolicy>(resolver, keys[i]);
                    }
                }

                resolverPolicyDestination.Set<IConstructorSelectorPolicy>(new SelectedConstructorCache(selectedConstructor), context.BuildKey);
            }
        }
    }
}