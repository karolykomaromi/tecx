namespace TecX.Unity.Mixin
{
    using System;
    using System.Collections;
    using System.Linq;

    using Microsoft.Practices.ObjectBuilder2;

    using Remotion.Mixins;
    using Remotion.Mixins.Context;
    using Remotion.Reflection;

    using TecX.Common;

    public class MixinStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            // is the currently requested type registered in the mixinconfiguration?
            ClassContext cc = MixinConfiguration.ActiveConfiguration.GetContext(context.BuildKey.Type);

            // if so...
            if (cc != null)
            {
                // try to figure out which constructor on the mixins target type should be called
                IPolicyList resolverPolicyDestination;
                IConstructorSelectorPolicy selector = context.Policies.Get<IConstructorSelectorPolicy>(context.BuildKey, out resolverPolicyDestination);

                SelectedConstructor ctor = selector.SelectConstructor(context, resolverPolicyDestination);

                if (ctor == null)
                {
                    return;
                }

                // resolve all dependencies for that ctor
                ArrayList parameterValues = new ArrayList();

                string[] parameterKeys = ctor.GetParameterKeys();

                foreach (string key in parameterKeys)
                {
                    IDependencyResolverPolicy resolver = resolverPolicyDestination.Get<IDependencyResolverPolicy>(key);

                    object resolvedParam = resolver.Resolve(context);

                    parameterValues.Add(resolvedParam);
                }

                // put them in a paramlist for the mixin objectfactory
                Type[] parameterTypes = ctor.Constructor.GetParameters().Select(p => p.ParameterType).ToArray();

                ParamList paramList = ParamList.CreateDynamic(parameterTypes, parameterValues.ToArray());

                // put a policy in the unity pipeline that will use the mixin objectfactory to create
                // the requested object
                IBuildPlanPolicy policy = new MixinObjectFactoryBuildPlanPolicy(paramList);

                context.Policies.Set<IBuildPlanPolicy>(policy, context.BuildKey);
            }
        }
    }
}