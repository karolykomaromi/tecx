namespace TecX.Unity.Configuration.Test
{
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingSingleImplementationOfInterfaceConvention : Given_ContainerAndBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            builder.Scan(
                s =>
                    {
                        s.AssemblyContainingType(typeof(IInterfaceName));

                        s.SingleImplementationsOfInterface();
                    });
        }

        [TestMethod]
        public void Then_FinsSingleImplementationsOfInterface()
        {
            IInterfaceName result = container.ResolveAll<IInterfaceName>().Single();

            Assert.IsNotNull(result);
        }
    }
}