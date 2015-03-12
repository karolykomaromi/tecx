namespace Hydra.Unity.Tracking
{
    using System;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class DisposableExtension : UnityContainerExtension, IDisposable
    {
        private readonly BuildTreeTracker tracker;

        public DisposableExtension()
        {
            this.tracker = new BuildTreeTracker();
        }

        public BuildTreeTracker Tracker
        {
            get { return this.tracker; }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Tracker.Dispose();
            }
        }

        protected override void Initialize()
        {
            this.Context.Strategies.Add(this.Tracker, UnityBuildStage.PreCreation);

            this.Context.ChildContainerCreated += OnChildContainerCreated;
        }

        private static void OnChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.AddExtension(new DisposableExtension());
        }
    }
}