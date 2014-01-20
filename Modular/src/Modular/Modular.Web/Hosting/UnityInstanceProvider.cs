using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;

namespace Modular.Web.Hosting
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer container;
        private readonly Type serviceType;

        public UnityInstanceProvider(IUnityContainer container, Type serviceType)
        {
            Contract.Requires(container != null);
            Contract.Requires(serviceType != null);

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

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            object serviceInstance = this.Container.Resolve(this.ServiceType);

            return serviceInstance;
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            this.Container.Teardown(instance);
        }
    }
}
