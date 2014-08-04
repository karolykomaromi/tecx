namespace TecX.Unity.ContextualBinding.Test
{
    using System;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding.Test.TestObjects;
    using TecX.Unity.Tracking;

    [TestClass]
    public class When_RegisteringWithParentNamespaceCondition : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void Act()
        {
            Predicate<IRequest> predicate = request =>
                {
                    IRequest current = Request.Current;

                    while (current.ParentRequest != null)
                    {
                        current = current.ParentRequest;
                    }

                    bool isMatch = current.BuildKey.Type.Namespace.EndsWith("TestObjects");
                    return isMatch;
                };

            container.RegisterType<IMyInterface, MyOtherClass>(predicate);
        }

        [TestMethod]
        public void Then_ResolvesProperly()
        {
            container.Resolve<ParentWithDependency>();
        }
    }
}