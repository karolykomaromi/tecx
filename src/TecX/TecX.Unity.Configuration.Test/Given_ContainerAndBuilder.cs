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
            this.container = new UnityContainer();

            this.builder = new ConfigurationBuilder();
        }

        protected override void When()
        {
            this.container.AddExtension(this.builder);
        }
    }
}