namespace TecX.Unity.Test
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Injection;

    [TestClass]
    public class NamedPropertyOverrideFixture
    {
        [TestMethod]
        public void CanSpecifyPropertyOverrideByName()
        {
            var container = new UnityContainer();

            container.RegisterType<Person>(new InjectionProperty("Foo"));
            container.RegisterType<IFoo, One>("1");
            container.RegisterType<IFoo, Two>("2");
            container.RegisterType<IFoo, Three>("3");

            Person person = container.Resolve<Person>(new NamedPropertyOverride("Foo", "1"));

            Assert.IsNotNull(person.Foo);
            Assert.IsInstanceOfType(person.Foo, typeof(One));
        }

        public class Person
        {
            public IFoo Foo { get; set; }
        }

        public interface IFoo
        {
        }

        public class One : IFoo
        {
        }

        public class Two : IFoo
        {
        }

        public class Three : IFoo
        {
        }
    }
}
