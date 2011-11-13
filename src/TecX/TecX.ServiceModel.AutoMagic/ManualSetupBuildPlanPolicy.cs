namespace TecX.ServiceModel.AutoMagic
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;
    using TecX.Common.Extensions.Error;

    public class ManualSetupBuildPlanPolicy : IBuildPlanPolicy
    {
        private readonly EndpointAddress address;
        private readonly Binding binding;

        public ManualSetupBuildPlanPolicy(EndpointAddress address, Binding binding)
        {
            Guard.AssertNotNull(address, "address");
            Guard.AssertNotNull(binding, "binding");

            this.address = address;
            this.binding = binding;
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
                context.Existing = WcfServiceHelper.CreateWcfChannelProxy(typeToConstruct, this.address, this.binding);
            }
        }
    }
}
