namespace TecX.Unity.ContextualBinding.Test
{
    using TecX.Unity.ContextualBinding.Test.TestObjects;

    public abstract class Given_Instance : Given_ContainerWithContextualBindingExtension
    {
        protected MyParameterLessClass instance;

        protected override void Given()
        {
            base.Given();

            instance = new MyParameterLessClass();
        }
    }
}