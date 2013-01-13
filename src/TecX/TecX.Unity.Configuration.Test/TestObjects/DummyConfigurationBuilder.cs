namespace TecX.Unity.Configuration.Test.TestObjects
{
    internal class DummyConfigurationBuilder : ConfigurationBuilder
    {
        public DummyConfigurationBuilder(DummyExtension extension)
        {
            this.AddExpression(cfg => cfg.AddExtension(extension));
        }
    }
}