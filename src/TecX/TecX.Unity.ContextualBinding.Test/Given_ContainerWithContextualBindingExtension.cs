namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.Unity;

    using TecX.TestTools;

    public abstract class Given_ContainerWithContextualBindingExtension : GivenWhenThen
    {
        protected IUnityContainer container;

        protected override void Given()
        {
            container = new UnityContainer();

            container.AddNewExtension<ContextualBindingExtension>();
        }
    }
}