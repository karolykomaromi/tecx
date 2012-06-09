namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;
    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ContextualMappingUsesInformationFromCtx : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.Configure<IContextualBinding>()["someKey"] = 123;

            Predicate<IBindingContext, IBuilderContext> matches = (bindingContext, builderContext) =>
                {
                    if ((int)bindingContext["someKey"] == 123)
                    {
                        return true;
                    }

                    return false;
                };

            container.RegisterType<IMyInterface, MyParameterLessClass>(matches);
        }

        [TestMethod]
        public void Then_PullsInformationCorrectlyFromContext()
        {
            var myClass = container.Resolve<BindingNamespaceTest>();

            Assert.IsNotNull(myClass);
        }
    }
}