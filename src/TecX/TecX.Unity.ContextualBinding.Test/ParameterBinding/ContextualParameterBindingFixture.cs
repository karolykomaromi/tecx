namespace TecX.Unity.ContextualBinding.Test.ParameterBinding
{
    using System;
    using System.Configuration;

    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.ContextualBinding;
    using TecX.Unity.ContextualBinding.Test.TestObjects;

    [TestClass]
    public class ContextualParameterBindingFixture
    {
        [TestMethod]
        public void CanBindParameterOverride()
        {
            var container = new UnityContainer();

            container.AddNewExtension<ContextualBinding>();

            container.RegisterType<IMyService, WritesToDatabaseService>(
                new InjectionConstructor("1"),
                new DestinationDependentConnection("http://localhost/service", "2"));

            OperationContext.Current = new OperationContext { IncomingMessageHeaders = new MessageHeaders { To = new Uri("http://localhost/service") } };

            IMyService service = container.Resolve<IMyService>();

            Assert.AreEqual("Data Source=(local);Initial Catalog=Db2;Integrated Security=True", service.ConnectionString);

            OperationContext.Current = null;

            service = container.Resolve<IMyService>();
            Assert.AreEqual("1", service.ConnectionString);
        }

        [TestMethod]
        public void CanOverrideMultipleParameters()
        {
            var container = new UnityContainer();

            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");

            container.LoadConfiguration(section);

            OperationContext.Current = new OperationContext { IncomingMessageHeaders = new MessageHeaders { To = new Uri("http://localhost/service") } };

            IFoo foo = container.Resolve<IFoo>();

            Assert.IsInstanceOfType(foo, typeof(DbFoo));

            Assert.AreEqual("Data Source=(local);Initial Catalog=Db3;Integrated Security=True", ((DbFoo)foo).ConnectionString);
            Assert.AreEqual("Data Source=(local);Initial Catalog=Db4;Integrated Security=True", ((DbFoo)foo).ConnectionString2);

            OperationContext.Current = null;

            foo = container.Resolve<IFoo>();

            Assert.IsInstanceOfType(foo, typeof(DbFoo));

            Assert.AreEqual("Data Source=(local);Initial Catalog=Db1;Integrated Security=True", ((DbFoo)foo).ConnectionString);
            Assert.AreEqual("Data Source=(local);Initial Catalog=Db2;Integrated Security=True", ((DbFoo)foo).ConnectionString2);
        }

        [TestMethod]
        public void CanBindParameterInScope()
        {
            var container = new UnityContainer();

            container.AddNewExtension<ContextualBinding>();

            container.RegisterType<SomeClass>(
                new InjectionConstructor("1"),
                new ContextualParameter(
                    request =>
                    {
                        object foo = request["foo"];
                        return foo != null && (bool)foo;
                    },
                        "foo",
                        "2"));

            SomeClass sut;

            using (new ContextScope(container, new ContextInfo("foo", true)))
            {
                sut = container.Resolve<SomeClass>();

                Assert.AreEqual("2", sut.Foo);
            }

            sut = container.Resolve<SomeClass>();

            Assert.AreEqual("1", sut.Foo);
        }
    }
}