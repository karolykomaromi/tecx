using System;
using System.ServiceModel;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.ServiceModel.Unity
{
    /// <summary>
    /// A ServiceHost that uses Unity to resolve dependencies
    /// </summary>
    public class UnityServiceHost : ServiceHost
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly Type _serviceType;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHost"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IUnityContainer"/> used
        /// to resolve dependencies</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(serviceType, "serviceType");

            _container = container;
            _serviceType = serviceType;
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Overrides of ServiceHost

        /// <summary>
        /// Invoked during the transition of a communication object into the opening state.
        /// </summary>
        protected override void OnOpening()
        {
            UnityServiceBehavior unityServiceBehavior = new UnityServiceBehavior(_container, _serviceType);

            Description.Behaviors.Add(unityServiceBehavior);

            base.OnOpening();
        }

        #endregion Overrides of ServiceHost
    }
}