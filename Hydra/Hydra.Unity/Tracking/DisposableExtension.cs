namespace Hydra.Unity.Tracking
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    [DebuggerDisplay("Tag={Tag}")]
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
            int level = 0;

            IUnityContainer parent = this.Container.Parent;
            while (parent != null)
            {
                level++;
                parent = parent.Parent;
            }

            this.Tracker.Tag = "level" + level.ToString(CultureInfo.InvariantCulture);

            this.Context.Strategies.Add(this.Tracker, UnityBuildStage.PreCreation);

            this.Context.ChildContainerCreated += this.OnChildContainerCreated;
        }

        private void OnChildContainerCreated(object sender, ChildContainerCreatedEventArgs e)
        {
            e.ChildContainer.AddNewExtension<DisposableExtension>();
        }
    }
}