namespace TecX.ServiceModel.Unity
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    /// <summary>
    /// Resolves services using Unity 
    /// </summary>
    public class UnityInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer container;
        private readonly Type serviceType;

        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            Guard.AssertNotNull(container, "container");
            Guard.AssertNotNull(serviceType, "serviceType");

            this.container = container;
            this.serviceType = serviceType;
        }

        public IUnityContainer Container
        {
            get
            {
                return this.container;
            }
        }

        public Type ServiceType
        {
            get
            {
                return this.serviceType;
            }
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            object serviceInstance = this.Container.Resolve(this.ServiceType);

            return serviceInstance;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            if (instance != null)
            {
                this.Container.Teardown(instance);
            }
        }
    }
}