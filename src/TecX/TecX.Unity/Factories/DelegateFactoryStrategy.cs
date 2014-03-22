namespace TecX.Unity.Factories
{
    using System;
    using Microsoft.Practices.ObjectBuilder2;

    public class DelegateFactoryStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            if (typeof(Delegate).IsAssignableFrom(context.BuildKey.Type))
            {
                IBuildPlanPolicy policy = context.Policies.Get<IBuildPlanPolicy>(context.BuildKey, false);

                if (policy == null)
                {
                    context.Policies.Set<IBuildPlanPolicy>(new DelegateFactoryBuildPlanPolicy(context.BuildKey.Type), context.BuildKey);
                }
            }
        }
    }
}