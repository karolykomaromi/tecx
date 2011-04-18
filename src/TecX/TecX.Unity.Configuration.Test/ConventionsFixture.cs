using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.TestTools;
using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Test.TestObjects;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Test
{
    public abstract class Given_AContainer : ArrangeActAssert
    {
        protected IUnityContainer _container;

        protected override void Arrange()
        {
            _container = new UnityContainer();
        }
    }

    [TestClass]
    public class When_ApplyingFindAllTypesConvention : Given_AContainer
    {
        private List<IMyInterface> _all;

        protected override void Act()
        {
            _container.Configure(r =>
            r.Scan(x =>
            {
                x.With(new FindAllTypesConvention(typeof(IMyInterface)));

                x.AssemblyContainingType<IMyInterface>();

                x.Exclude(t => t.Name == "MyClassWithCtorParams");
            }));

            _all = _container
                        .ResolveAll<IMyInterface>()
                        .OrderBy(i => i.GetType().Name)
                        .ToList();
        }

        [TestMethod]
        public void Then_RegistersImplementationsOfInterfaceExpectExcluded()
        {
            Assert.AreEqual(2, _all.Count);
            Assert.IsInstanceOfType(_all[0], typeof(MyClass));
            Assert.IsInstanceOfType(_all[1], typeof(MyOtherClass));
        }
    }

    [TestClass]
    public class When_ApplyingFirstInterfaceConvention : Given_AContainer
    {
        private List<IMyInterface> _all;
        private List<IAnotherInterface> _others;

        protected override void Act()
        {
            _container.Configure(r =>
                r.Scan(s =>
                {
                    s.RegisterConcreteTypesAgainstTheFirstInterface();

                    s.AssemblyContainingType<IMyInterface>();

                    s.Exclude(t => t.Name == "MyClassWithCtorParams");
                }));

            _all = _container
                        .ResolveAll<IMyInterface>()
                        .OrderBy(i => i.GetType().Name)
                        .ToList();

            _others = _container
                        .ResolveAll<IAnotherInterface>()
                        .ToList();
        }

        [TestMethod]
        public void Then_RegistersImplementationsOfInterfaceExpectExcluded()
        {
            Assert.AreEqual(2, _all.Count);
            Assert.IsInstanceOfType(_all[0], typeof(MyClass));
            Assert.IsInstanceOfType(_all[1], typeof(MyOtherClass));

            Assert.AreEqual(1, _others.Count);
            Assert.IsInstanceOfType(_others[0], typeof(ClassThatImplementsAnotherInterface));
        }
    }

    [TestClass]
    public class ConventionsFixture
    {
        [TestMethod]
        public void CanApplyFindRegistriesConvention()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                r.Scan(s =>
                {
                    s.With(new FindRegistriesConvention());

                    s.AssemblyContainingType(typeof(RegistrySubClass));
                }));

            IRepository<int> repository = container.Resolve<IRepository<int>>();

            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void CanApplyImplementsIInterfaceNameConvention()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType(typeof(IInterfaceName));

                    s.With(new ImplementsIInterfaceNameConvention());
                }));

            IInterfaceName result = container.ResolveAll<IInterfaceName>().Single();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanApplySingleImplementationOfInterfaceConvention()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                r.Scan(s =>
                {
                    s.AssemblyContainingType(typeof(IInterfaceName));

                    s.SingleImplementationsOfInterface();
                }));

            IInterfaceName result = container.ResolveAll<IInterfaceName>().Single();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CanApplyFindAllTypesConvetion()
        {
            IUnityContainer container = new UnityContainer();

            Registry registry = new Registry();

            registry.Scan(s =>
                {
                    var convention = new FindAllTypesConvention(typeof(IMyInterface));

                    convention.NameBy(t => t.FullName + "123");

                    s.With(convention);

                    s.AssemblyContainingType<IMyInterface>();

                    s.Exclude(t => t == typeof(MyClassWithCtorParams));
                });

            container.AddExtension(registry);

            var results = container.ResolveAll<IMyInterface>();

            Assert.IsNotNull(results);
            Assert.AreEqual(2, results.Count());
        }
    }
}
