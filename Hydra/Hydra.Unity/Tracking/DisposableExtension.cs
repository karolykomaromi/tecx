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

        public string Tag
        {
            get { return this.Tracker.Tag; }
            set { this.Tracker.Tag = value; }
        }

        public BuildTreeTracker Tracker
        {
            get { return tracker; }
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
        }
    }
}