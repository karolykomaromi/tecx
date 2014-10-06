namespace Modular.Web.Hosting
{
    using System;
    using System.Diagnostics.Contracts;
    using System.ServiceModel;
    using Microsoft.Practices.Unity;

    public class UnityServiceHost : ServiceHost
    {
        private readonly IUnityContainer container;

        public UnityServiceHost(IUnityContainer container, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            Contract.Requires(container != null);

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
            if (this.Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                UnityServiceBehavior unityServiceBehavior = new UnityServiceBehavior(this.Container);

                this.Description.Behaviors.Add(unityServiceBehavior);
            }

            base.OnOpening();
        }
    }
}