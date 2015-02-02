namespace TecX.Unity.Configuration.Test
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingFirstInterfaceConvention : Given_ContainerAndBuilder
    {
        private List<IMyInterface> all;
        private List<IAnotherInterface> others;

        protected override void Arrange()
        {
            base.Arrange();

            builder.Scan(s =>
                {
                    s.RegisterConcreteTypesAgainstTheFirstInterface();

                    s.AssemblyContainingType<IMyInterface>();

                    s.ExcludeType<MyClassWithCtorParams>();
                });
        }

        [TestMethod]
        public void Then_RegistersImplementationsOfInterfaceExpectExcluded()
        {
            all = container
                .ResolveAll<IMyInterface>()
                .OrderBy(i => i.GetType().Name)
                .ToList();

            others = container
                .ResolveAll<IAnotherInterface>()
                .ToList();

            Assert.AreEqual(2, this.all.Count);
            Assert.IsInstanceOfType(this.all[0], typeof(MyClass));
            Assert.IsInstanceOfType(this.all[1], typeof(MyOtherClass));

            Assert.AreEqual(1, this.others.Count);
            Assert.IsInstanceOfType(this.others[0], typeof(ClassThatImplementsAnotherInterface));
        }
    }
}