namespace TecX.Unity.Enums
{
    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    public class EnumStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");
            Guard.AssertNotNull(context.BuildKey, "context.BuildKey");
            Guard.AssertNotNull(context.BuildKey.Type, "context.BuildKey.Type");

            if (context.BuildKey.Type.IsEnum && context.Existing == null)
            {
                IBuildPlanPolicy policy = new EnumPolicy(context.BuildKey.Type);

                context.Policies.Set(policy, context.BuildKey);
            }
        }
    }
}