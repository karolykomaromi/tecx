namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;
    using TecX.Unity.Configuration;

    public abstract class Given_BuilderAndContainerWithContextualBindingExtension : GivenWhenThen
    {
        protected IUnityContainer container;

        protected ConfigurationBuilder builder;

        protected override void Given()
        {
            this.container = new UnityContainer();

            this.container.AddNewExtension<ContextualBindingExtension>();

            this.builder = new ConfigurationBuilder();
        }

        protected override void When()
        {
            this.container.AddExtension(this.builder);
        }
    }
}