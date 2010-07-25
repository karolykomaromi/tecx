using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.ServiceModel.Unity
{
    /// <summary>
    /// Resolves services using Unity 
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        #region Fields

        private readonly IUnityContainer _container;
        private readonly Type _serviceType;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityInstanceProvider"/> class.
        /// </summary>
        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(serviceType, "serviceType");

            _container = container;
            _serviceType = serviceType;
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Implementation of IInstanceProvider

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>The service object.</returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            object serviceInstance = _container.Resolve(_serviceType);

            return serviceInstance;
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>A user-defined service object.</returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance != null)
                _container.Teardown(instance);
        }

        #endregion Implementation of IInstanceProvider
    }
}