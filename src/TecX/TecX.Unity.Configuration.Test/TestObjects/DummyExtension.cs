namespace TecX.Unity.Configuration.Test.TestObjects
{
    using System;

    using Microsoft.Practices.Unity;

    internal class DummyExtension : UnityContainerExtension
    {
        public event EventHandler Initialized = delegate { };

        protected override void Initialize()
        {
            this.Initialized(this, EventArgs.Empty);
        }
    }
}