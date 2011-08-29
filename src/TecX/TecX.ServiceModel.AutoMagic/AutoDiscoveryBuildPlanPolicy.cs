using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common.Extensions.Error;
using TecX.Common;

namespace TecX.ServiceModel.AutoMagic
{
    /// <summary>
    /// Policy used to build the wcf proxy from the interface using wcf auto-discovery
    /// </summary>
    public class AutoDiscoveryBuildPlanPolicy : IBuildPlanPolicy
    {
        /// <summary>
        /// Stores the scopes used for auto discovery
        /// </summary>
        private readonly Uri[] _scopes;


        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDiscoveryBuildPlanPolicy"/> class
        /// </summary>
        public AutoDiscoveryBuildPlanPolicy(params Uri[] scopes)
        {
            _scopes = scopes;
        }

        /// <summary>
        /// Creates an instance of this build plan's type, or fills in the existing type if passed in.
        /// </summary>
        /// <param name="context">Context used to build up the object.</param>
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
                context.Existing = WcfServiceHelper.CreateWcfChannelProxy(typeToConstruct, _scopes);
            }
        }
    }
}