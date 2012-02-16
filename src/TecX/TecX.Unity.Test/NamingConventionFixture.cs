namespace TecX.Unity.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Injection;
    using TecX.Unity.Test.TestObjects;

    [TestClass]
    public class NamingConventionFixture
    {
        [TestMethod]
        public void CanMatchSpecifiedName()
        {
            var convention = new SpecifiedNameConvention("MyClass");

            Assert.IsTrue(convention.NameMatches("myClass"));
        }

        [TestMethod]
        public void CanMatchByTypeName()
        {
            var convention = NamingConvention.CreateForType(typeof(NamingConventionTestObject));

            Assert.IsTrue(convention.NameMatches("NamingConventionTestObject"));
            Assert.IsTrue(convention.NameMatches("ConventionTestObject"));
            Assert.IsTrue(convention.NameMatches("TestObject"));
            Assert.IsTrue(convention.NameMatches("Object"));
            Assert.IsTrue(convention.NameMatches("SomeBaseClass"));
            Assert.IsTrue(convention.NameMatches("BaseClass"));
            Assert.IsTrue(convention.NameMatches("Class"));
            Assert.IsTrue(convention.NameMatches("Foo"));
        }

        [TestMethod]
        public void CanMatchConnectionString()
        {
            var convention = NamingConvention.CreateForType(typeof(string));

            Assert.IsTrue(convention.NameMatches("connectionString"));
            Assert.IsTrue(convention.NameMatches("file"));
            Assert.IsTrue(convention.NameMatches("fileName"));
            Assert.IsTrue(convention.NameMatches("path"));
        }
    }
}
