namespace TecX.Unity.ContextualBinding.Test
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;
    using TecX.Unity.Tracking;

    [TestClass]
    public class When_ContextualMappingUsesInformationFromCtx : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void Act()
        {
            Request.StaticRequestContext["someKey"] = 123;

            Predicate<IRequest> matches = request =>
                {
                    if ((int)request.RequestContext["someKey"] == 123)
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