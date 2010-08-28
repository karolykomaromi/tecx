using System;

using Microsoft.Practices.ObjectBuilder2;

using TecX.Common.Extensions.Error;

namespace TecX.ServiceModel.AutoMagic
{
    /// <summary>
    /// Policy used to build the wcf proxy from the interface using wcf auto-discovery
    /// </summary>
    public class AutoDiscoveryBuildPlanPolicy : IBuildPlanPolicy
    {
        #region Properties

        /// <summary>
        /// Gets or sets the scopes used for auto discovery
        /// </summary>
        /// <value>The scopes.</value>
        public Uri[] Scopes { get; set; }


        #endregion Properties

        ////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoDiscoveryBuildPlanPolicy"/> class
        /// </summary>
        public AutoDiscoveryBuildPlanPolicy(params Uri[] scopes)
        {
            Scopes = scopes;
        }

        #region Implementation of IBuildPlanPolicy

        /// <summary>
        /// Creates an instance of this build plan's type, or fills in the existing type if passed in.
        /// </summary>
        /// <param name="context">Context used to build up the object.</param>
        public void BuildUp(IBuilderContext context)
        {
            if (context.Existing == null)
            {
                Type typeToConstruct = context.BuildKey.Type;
                if (typeToConstruct == null)
                {
                    throw new ArgumentException("Cannot extract argument from buildKey", "context.BuildKey.Type")
                        .WithAdditionalInfo("buildKey", context.BuildKey);
                }

                //delegate creation of the channel proxy
                context.Existing = WcfServiceHelper.CreateWcfChannelProxy(typeToConstruct, Scopes);
            }
        }

        #endregion Implementation of IBuildPlanPolicy
    }
}