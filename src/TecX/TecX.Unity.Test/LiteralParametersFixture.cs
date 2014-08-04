namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Literals;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class LiteralParametersFixture
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

        [TestMethod]
        public void CanResolveMethodParameter()
        {
            var container = new UnityContainer()
                                    .WithDefaultConventionsForLiteralParameters()
                                    .RegisterType<MethodTakesPrimitiveParameter>(new InjectionMethod("InjectionGoesHere", typeof(int)));

            var foo = container.Resolve<MethodTakesPrimitiveParameter>();

            Assert.AreEqual(123, foo.Abc);
        }

        [TestMethod]
        public void CanInjectProperty()
        {
            var container = new UnityContainer()
                                    .WithDefaultConventionsForLiteralParameters()
                                    .RegisterType<PropertyTakesPrimitiveParameter>(new InjectionProperty("Abc"));

            var foo = container.Resolve<PropertyTakesPrimitiveParameter>();

            Assert.AreEqual(123, foo.Abc);
        }
    }
}
