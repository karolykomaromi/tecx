using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace TecX.Unity.Configuration.Test.TestObjects
{
    using System;

    internal class DummyConfigurationBuilder : ConfigurationBuilder
    {
        public DummyConfigurationBuilder(DummyExtension extension)
        {
            this.AddExpression(cfg => cfg.AddModification(container => container.AddExtension(extension)));
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