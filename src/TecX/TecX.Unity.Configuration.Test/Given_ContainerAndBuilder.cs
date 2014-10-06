namespace TecX.Unity.Configuration.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;

    public abstract class Given_ContainerAndBuilder : ArrangeActAssert
    {
        protected IUnityContainer container;

        protected ConfigurationBuilder builder;

        protected override void Arrange()
        {
            this.container = new UnityContainer();

            this.builder = new ConfigurationBuilder();
        }

        protected override void Act()
        {
            this.container.AddExtension(this.builder);
        }
    }
}