namespace TecX.Unity.Configuration.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingFindAllImplementationsConvention : Given_ContainerAndBuilder
    {
        private List<IMyInterface> all;

        protected override void Arrange()
        {
            base.Arrange();

            builder.Scan(x =>
                {
                    x.With(new FindAllImplementationsConvention(typeof(IMyInterface)));

                    x.AssemblyContainingType<IMyInterface>();

                    x.ExcludeType<MyClassWithCtorParams>();
                });
        }

        [TestMethod]
        public void Then_RegistersImplementationsOfInterfaceExpectExcluded()
        {
            all = container
                .ResolveAll<IMyInterface>()
                .OrderBy(i => i.GetType().Name)
                .ToList();

            Assert.AreEqual(2, this.all.Count);
            Assert.IsInstanceOfType(this.all[0], typeof(MyClass));
            Assert.IsInstanceOfType(this.all[1], typeof(MyOtherClass));
        }
    }
}