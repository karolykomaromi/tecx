using Microsoft.Practices.ObjectBuilder2;

using Remotion.Mixins;
using Remotion.Reflection;

using TecX.Common;

namespace TecX.Unity.Mixin
{
    public class MixinObjectFactoryBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly ParamList _paramList;

        public MixinObjectFactoryBuildPlanPolicy(ParamList paramList)
        {
            Guard.AssertNotNull(paramList, "paramList");

            _paramList = paramList;
        }

        public void BuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (context.Existing != null)
            {
                return;
            }

            var mixinTarget = ObjectFactory.Create(context.BuildKey.Type, _paramList);

            context.Existing = mixinTarget;
        }
    }
}