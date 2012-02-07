namespace TecX.Unity.ContextualBinding.Test
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common;
    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class When_RegisteringWithParentNamespaceCondition : Given_BuilderAndContainerWithContextualBindingExtension
    {
        protected override void When()
        {
            Predicate<IBindingContext, IBuilderContext> condition = (bindingCtx, builderCtx) =>
                {
                    bool isMatch = bindingCtx.CurrentBuildNode.RootNode.BuildKey.Type.Namespace.EndsWith("TestObjects");
                    return isMatch;
                };

            container.RegisterType<IMyInterface, MyOtherClass>(condition);
        }

        [TestMethod]
        public void Then_ResolvesProperly()
        {
            container.Resolve<ParentWithDependency>();
        }
    }
}