namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;

    public abstract class Given_ContainerAndBuilder : GivenWhenThen
    {
        protected IUnityContainer container;

        protected ConfigurationBuilder builder;

        protected override void Given()
        {
            container = new UnityContainer();

            builder = new ConfigurationBuilder();
        }

        protected override void When()
        {
            container.AddExtension(builder);
        }
    }
}