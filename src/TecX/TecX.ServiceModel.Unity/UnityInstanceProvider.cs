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
        private readonly IUnityContainer _container;
        private readonly Type _serviceType;

        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(serviceType, "serviceType");

            _container = container;
            _serviceType = serviceType;
        }

        public IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        public Type ServiceType
        {
            get
            {
                return _serviceType;
            }
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            object serviceInstance = Container.Resolve(ServiceType);

            return serviceInstance;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance != null)
            {
                Container.Teardown(instance);
            }
        }
    }
}