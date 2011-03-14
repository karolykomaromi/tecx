using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Test.TestObjects;
using TecX.Unity.Configuration.Extensions;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class ConventionsFixture
    {
        [TestMethod]
        public void CanApplyFindAllTypesConvention()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                r.Scan(x =>
                {
                    x.With(new FindAllTypesConvention(typeof(IMyInterface)));

                    x.AssemblyContainingType<IMyInterface>();

                    x.Exclude(t => t.Name == "MyClassWithCtorParams");
                }));

            IEnumerable<IMyInterface> allResults = container.ResolveAll<IMyInterface>();

            Assert.AreEqual(2, allResults.Count());
        }

        [TestMethod]
        public void CanApplyFirstInterfaceConvention()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                r.Scan(s =>
                {
                    s.RegisterConcreteTypesAgainstTheFirstInterface();

                    s.AssemblyContainingType<IMyInterface>();

                    s.Exclude(t => t.Name == "MyClassWithCtorParams");
                }));

            IEnumerable<IMyInterface> results = container.ResolveAll<IMyInterface>();

            Assert.AreEqual(2, results.Count());

            IEnumerable<IAnotherInterface> others = container.ResolveAll<IAnotherInterface>();

            Assert.AreEqual(1, others.Count());
        }

        [TestMethod]
        public void CanApplyGenericConnectionConvention()
        {
            IUnityContainer container = new UnityContainer();

            container.Configure(r =>
                {
                    var convention = new GenericConnectionConvention(typeof(IRepository<>));

                    r.Scan(s =>
                        {
                            s.With(convention);
                            s.AssemblyContainingType(typeof(IRepository<>));
                        });
                });

            var result = container.ResolveAll<IRepository<string>>();
        }
    }
}
