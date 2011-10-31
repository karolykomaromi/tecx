namespace TecX.Unity.Configuration.Test
{
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class When_RegisteringMappingForOpenGenericType : Given_ContainerAndBuilder
    {
        protected override void Given()
        {
            base.Given();

            builder.For(typeof(IEnumerable<>)).Use(typeof(List<>)).DefaultCtor();
        }

        [TestMethod]
        public void Then_CanRegisterMappingForOpenGenericType()
        {
            IEnumerable<int> result = container.Resolve<IEnumerable<int>>();

            Assert.IsInstanceOfType(result, typeof(List<int>));
        }
    }
}
