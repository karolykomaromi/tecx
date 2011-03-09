using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Unity.Configuration.Common;
using TecX.Unity.Configuration.Conventions;
using TecX.Unity.Configuration.Test.TestObjects;

namespace TecX.Unity.Configuration.Test
{
    [TestClass]
    public class ConventionsFixture
    {
        [TestMethod]
        public void CanApplyFindAllTypesConvention()
        {
            IUnityContainer container = new UnityContainer();

            RegistrationGraph graph = new RegistrationGraph();

            graph.Configure(r =>
                r.Scan(x =>
                {
                    x.With(new FindAllTypesConvention(typeof(IMyInterface)));

                    x.AssemblyContainingType<IMyInterface>();

                    x.Exclude(t => t.Name == "MyClassWithCtorParams");
                }));

            graph.Configure(container);

            IEnumerable<IMyInterface> allResults = container.ResolveAll<IMyInterface>();

            Assert.AreEqual(2, allResults.Count());
        }
    }
}
