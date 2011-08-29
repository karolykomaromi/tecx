using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.ServiceModel.AutoMagic
{
    public class AppConfigBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly string _endpointConfigName;

        public AppConfigBuildPlanPolicy(string endpointConfigName)
        {
            Guard.AssertNotNull(endpointConfigName, "endpointConfigName");

            _endpointConfigName = endpointConfigName;
        }

        public void BuildUp(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            if (context.Existing == null)
            {
                Type typeToConstruct = context.BuildKey.Type;
                if (typeToConstruct == null)
                {
                    throw new ArgumentException("Cannot extract argument from buildKey", "context.BuildKey.Type")
                        .WithAdditionalInfo("buildKey", context.BuildKey);
                }

                // delegate creation of the channel proxy
                context.Existing = WcfServiceHelper.CreateWcfChannelProxy(typeToConstruct, _endpointConfigName);
            }
        }
    }
}
