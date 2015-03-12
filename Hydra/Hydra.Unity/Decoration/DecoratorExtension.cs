namespace Hydra.Unity.Decoration
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class DecoratorExtension : UnityContainerExtension, IDisposable
    {
        private IDisposable subscription;

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected override void Initialize()
        {
            var observer = Observable.FromEventPattern<RegisterEventArgs>(
                handler =>
                {
                    this.Context.Registering += handler;
                },
                handler =>
                {
                    this.Context.Registering -= handler;
                });

            var strategy = new DecoratorStrategy();

            this.subscription = observer.Where(IsMappedFromInterface).Subscribe(strategy);

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);

            this.Context.ChildContainerCreated += OnChildContainerCreated;
        }

        private static void OnChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.AddExtension(new DecoratorExtension());
        }

        private static bool IsMappedFromInterface(EventPattern<RegisterEventArgs> e)
        {
            bool isMappedFromInterface = e.EventArgs.TypeFrom != null && e.EventArgs.TypeFrom.IsInterface;

            return isMappedFromInterface;
        }

        private void Dispose(bool disposing)
        {
            if (disposing && this.subscription != null)
            {
                this.subscription.Dispose();
                this.subscription = null;
            }
        }
    }
}
