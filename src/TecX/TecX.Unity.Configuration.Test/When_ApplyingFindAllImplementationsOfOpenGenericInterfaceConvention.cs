namespace TecX.Unity.Configuration.Test
{
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Unity.Configuration.Conventions;
    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_ApplyingFindAllImplementationsOfOpenGenericInterfaceConvention : Given_ContainerAndBuilder
    {
        protected override void Given()
        {
            base.Given();

            this.builder.Scan(x =>
                {
                    x.With(new FindAllImplementationsOfOpenGenericInterfaceConvention(typeof(IMessageHandler<>)));

                    x.AssemblyContainingType(typeof(IMessageHandler<>));
                });
        }

        [TestMethod]
        public void Then_RegistersAllImplementations()
        {
            var resolved = this.container.ResolveAll(typeof(IMessageHandler<int>)).ToList();

            Assert.AreEqual(2, resolved.Count);

            resolved = this.container.ResolveAll(typeof(IMessageHandler<string>)).ToList();

            Assert.AreEqual(2, resolved.Count);
        }
    }
}
