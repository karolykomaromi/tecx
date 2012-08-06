namespace TecX.Unity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Injection;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class ArgumentMatchingConventionFixture
    {
        [TestMethod]
        public void CanMatchStringAsMappingName()
        {
            var convention = new StringAsMappingNameMatchingConvention();

            Assert.IsTrue(convention.Matches(new ConstructorParameter("1", "abc"), new FakeParameterInfo("abc", typeof(IFoo))));
        }

        [TestMethod]
        public void WontMatchStringAsMappingNameForPrimitiveParam()
        {
            var convention = new StringAsMappingNameMatchingConvention();

            Assert.IsFalse(convention.Matches(new ConstructorParameter("1", "abc"), new FakeParameterInfo("abc", typeof(int))));
        }

        [TestMethod]
        public void CanMatchSpecifiedName()
        {
            var convention = new SpecifiedNameMatchingConvention();

            Assert.IsTrue(convention.Matches(new ConstructorParameter("1", "abc"), new FakeParameterInfo("abc", typeof(string))));
        }

        [TestMethod]
        public void CanMatchByTypeName()
        {
            var convention = new ByTypeMatchingConvention();

            var argument = new ConstructorParameter(new NamingConventionTestObject());

            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("NamingConventionTestObject", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("ConventionTestObject", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("TestObject", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("Object", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("SomeBaseClass", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("BaseClass", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("Class", typeof(NamingConventionTestObject))));
            Assert.IsTrue(convention.Matches(argument, new FakeParameterInfo("Foo", typeof(NamingConventionTestObject))));
        }

        [TestMethod]
        public void CanMatchConnectionString()
        {
            var convention = new ConnectionStringMatchingConvention();

            Assert.IsTrue(convention.Matches(new ConstructorParameter("1"), new FakeParameterInfo("abcConnectionString", typeof(string))));
        }

        [TestMethod]
        public void CanMatchFileNames()
        {
            var convention = new FileNameMatchingConvention();

            Assert.IsTrue(convention.Matches(new ConstructorParameter("1"), new FakeParameterInfo("file", typeof(string))));
            Assert.IsTrue(convention.Matches(new ConstructorParameter("1"), new FakeParameterInfo("fileName", typeof(string))));
            Assert.IsTrue(convention.Matches(new ConstructorParameter("1"), new FakeParameterInfo("path", typeof(string))));
        }
    }
}
