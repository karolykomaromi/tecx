namespace TecX.Unity.ContextualBinding.Test
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_ContextualMappingUsesInformationFromCtx : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            container.Configure<ContextualBinding>()["someKey"] = 123;

            Predicate<IRequest> matches = request =>
                {
                    if ((int)request["someKey"] == 123)
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