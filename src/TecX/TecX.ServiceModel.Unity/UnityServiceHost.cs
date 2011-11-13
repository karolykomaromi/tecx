namespace TecX.ServiceModel.Unity
{
    using System;
    using System.ServiceModel;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    /// <summary>
    /// A ServiceHost that uses Unity to resolve dependencies
    /// </summary>
    public class UnityServiceHost : ServiceHost
    {
        private readonly IUnityContainer container;

        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Guard.AssertNotNull(container, "container");

            this.container = container;
        }

        public IUnityContainer Container
        {
            get
            {
                return this.container;
            }
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                UnityServiceBehavior unityServiceBehavior = new UnityServiceBehavior(this.Container);

                Description.Behaviors.Add(unityServiceBehavior);
            }

            base.OnOpening();
        }
    }
}