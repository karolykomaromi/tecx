namespace TecX.ServiceModel.Unity.AutoMagic
{
    using System;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Common.Extensions.Error;

    public class ConfigFileBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly string endpointConfigName;

        public ConfigFileBuildPlanPolicy(string endpointConfigName)
        {
            Guard.AssertNotNull(endpointConfigName, "endpointConfigName");

            this.endpointConfigName = endpointConfigName;
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
                context.Existing = WcfServiceHelper.CreateWcfChannelProxy(typeToConstruct, this.endpointConfigName);
            }
        }
    }
}
