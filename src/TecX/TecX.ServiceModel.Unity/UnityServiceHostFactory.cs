using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using TecX.Common;

namespace TecX.ServiceModel.Unity
{
    /// <summary>
    /// ServiceHostFactory that can be used by IIS to host a service configured with
    /// Unity
    /// </summary>
    public class UnityServiceHostFactory : ServiceHostFactory
    {
        #region Fields

        private readonly IUnityContainer _container;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        public IUnityContainer Container
        {
            get { return _container; }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////
        
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityServiceHostFactory"/> class
        /// </summary>
        /// <remarks>
        /// Creates an unconfigured <see cref="IUnityContainer"/> which must be initialized in derived classes
        /// or by any other mechanism that I just can't come up with right now...
        /// </remarks>
        public UnityServiceHostFactory()
        {
            _container = new UnityContainer();
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Overrides of ServiceHostFactory

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host.</param>
        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            Guard.AssertNotNull(serviceType, "serviceType");

            UnityServiceHost host = new UnityServiceHost(Container, serviceType, baseAddresses);

            return host;
        }

        #endregion Overrides of ServiceHostFactory
    }
}