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

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.tracker.DisposeAllTrees();
            }
        }

        protected override void Initialize()
        {
            this.Context.Strategies.Add(this.tracker, UnityBuildStage.PreCreation);
        }
    }
}