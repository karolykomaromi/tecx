namespace TecX.Unity.Configuration.Test
{
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingImplementsIInterfaceNameConvention : Given_ContainerAndBuilder
    {
        protected override void Arrange()
        {
            base.Arrange();

            builder.Scan(
                scanner =>
                    {
                        scanner.AssemblyContainingType(typeof(IInterfaceName));

                        scanner.With(new ImplementsIInterfaceNameConvention());
                    });
        }

        [TestMethod]
        public void Then_FindsTypesThatImplementsIInterfaceName()
        {
            IInterfaceName result = container.ResolveAll<IInterfaceName>().Single();

            Assert.IsNotNull(result);
        }
    }
}