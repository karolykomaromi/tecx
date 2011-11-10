namespace TecX.Unity.Mixin
{
    using Microsoft.Practices.ObjectBuilder2;

    using Remotion.Mixins;
    using Remotion.Reflection;

    using TecX.Common;

    public class MixinObjectFactoryBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly ParamList paramList;

        public MixinObjectFactoryBuildPlanPolicy(ParamList paramList)
        {
            Guard.AssertNotNull(paramList, "paramList");

            this.paramList = paramList;
        }

        public void BuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (context.Existing != null)
            {
                return;
            }

            var mixinTarget = ObjectFactory.Create(context.BuildKey.Type, this.paramList);

            context.Existing = mixinTarget;
        }
    }
}