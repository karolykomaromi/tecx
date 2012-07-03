namespace TecX.Unity.Literals
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
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
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.BuildKey, "context.BuildKey");
            Guard.AssertNotNull(context.BuildKey.Type, "context.BuildKey.Type");

            if (context.BuildKey.Type.IsPrimitive)
            {
                return;
            }

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

            IMethodSelectorPolicy methodSelector = context.Policies.Get<IMethodSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);
            IEnumerable<SelectedMethod> selectedMethods;

            if (methodSelector != null &&
                (selectedMethods = methodSelector.SelectMethods(context, resolverPolicyDestination)) != null)
            {
                selectedMethods = selectedMethods.ToArray();

                if (!selectedMethods.Any())
                {
                    return;
                }

                foreach (SelectedMethod method in selectedMethods)
                {
                    var keys = method.GetParameterKeys();
                    var parameters = method.Method.GetParameters();

                    for (int i = 0; i < keys.Length; i++)
                    {
                        var resolver = this.conventions.CreateResolver(context, parameters[i]);

                        if (resolver != null)
                        {
                            resolverPolicyDestination.Set<IDependencyResolverPolicy>(resolver, keys[i]);
                        }
                    }
                }

                resolverPolicyDestination.Set<IMethodSelectorPolicy>(new SelectedMethodsCache(selectedMethods), context.BuildKey);
            }
        }
    }
}