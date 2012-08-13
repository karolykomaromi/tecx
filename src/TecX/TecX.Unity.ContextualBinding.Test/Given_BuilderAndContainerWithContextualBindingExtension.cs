namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;
    using TecX.Unity.Configuration;

    public abstract class Given_BuilderAndContainerWithContextualBindingExtension : ArrangeActAssert
    {
        protected IUnityContainer container;

        protected ConfigurationBuilder builder;

        protected override void Arrange()
        {
            this.container = new UnityContainer();

            this.container.AddNewExtension<ContextualBinding>();

            this.builder = new ConfigurationBuilder();
        }

        protected override void Act()
        {
            this.container.AddExtension(this.builder);
        }
    }
}