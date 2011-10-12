using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace TecX.Unity.Configuration.Test.TestObjects
{
    using System;

    internal class DummyRegistry : Registry
    {
        public DummyRegistry(DummyExtension extension)
        {
            this.AddAction(() => Container.AddExtension(extension));
        }
    }

    internal class DummyExtension : UnityContainerExtension
    {
        public event EventHandler Initialized = delegate { };

        protected override void Initialize()
        {
            Initialized(this, EventArgs.Empty);
        }
    }
}