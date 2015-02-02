namespace TecX.Unity.Configuration.TestObjects
{
    public class DummyConfigurationBuilder : ConfigurationBuilder
    {
        public DummyConfigurationBuilder()
        {
            this.For<IFoo>().Use<Foo>();
        }
    }
}