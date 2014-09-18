namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    [DebuggerDisplay("Tag={Tag}")]
    public class DisposableExtension : UnityContainerExtension, IDisposable
    {
        private readonly Tags tags;
        private readonly BuildTreeTracker tracker;

        public DisposableExtension()
            : this(Tags.Random)
        {
        }

        public DisposableExtension(Tags tags)
        {
            Contract.Requires(tags != null);

            this.tags = tags;
            this.tracker = new BuildTreeTracker();
        }

        public string Tag
        {
            get { return this.Tracker.Tag; }
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
            this.Tracker.Tag = this.tags.GetTag(this.Container);

            this.Context.Strategies.Add(this.Tracker, UnityBuildStage.PreCreation);

            this.Context.ChildContainerCreated += this.OnChildContainerCreated;
        }

        private void OnChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.AddExtension(new DisposableExtension(this.tags));
        }
    }
}