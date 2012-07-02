namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Literals;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class LiteralParametersExtensionFixture
    {
        [TestMethod]
        public void CanResolveLiteralParamUsingAppSettings()
        {
            var container = new UnityContainer().WithDefaultConventionsForLiteralParameters();

            var foo = container.Resolve<TakesPrimitiveParameter>();

            Assert.AreEqual(123, foo.Abc);
        }

        [TestMethod]
        public void ResolverOverridesStilWork()
        {
            var container = new UnityContainer().WithDefaultConventionsForLiteralParameters();

            var foo = container.Resolve<TakesPrimitiveParameter>(new ParameterOverride("abc", 456));

            Assert.AreEqual(456, foo.Abc);
        }

        [TestMethod]
        public void CanResolveConnectionString()
        {
            var container = new UnityContainer().WithDefaultConventionsForLiteralParameters();

            var foo = container.Resolve<TakesConnectionStringParameter>();

            Assert.AreEqual(@"Data Source=localhost;Initial Catalog=Contoso;Integrated Security=True", foo.AbcConnectionString);
        }
    }
}
