namespace TecX.Unity.ContextualBinding.Test
{
    using TecX.Unity.ContextualBinding.Test.TestObjects;

    public abstract class Given_BuilderAndInstance : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected MyParameterLessClass instance;

        protected override void Arrange()
        {
            base.Arrange();

            instance = new MyParameterLessClass();
        }
    }
}