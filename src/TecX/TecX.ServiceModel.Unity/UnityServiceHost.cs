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
        private readonly IUnityContainer _container;

        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Guard.AssertNotNull(container, "container");

            _container = container;
        }

        public IUnityContainer Container
        {
            get
            {
                return _container;
            }
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                UnityServiceBehavior unityServiceBehavior = new UnityServiceBehavior(Container);

                Description.Behaviors.Add(unityServiceBehavior);
            }

            base.OnOpening();
        }
    }
}