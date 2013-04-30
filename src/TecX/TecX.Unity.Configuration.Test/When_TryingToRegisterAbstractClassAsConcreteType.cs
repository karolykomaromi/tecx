namespace TecX.Unity.Configuration.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Common.Error;
    using TecX.Unity.Configuration.Test.TestObjects;

    [TestClass]
    public class When_TryingToRegisterAbstractClassAsConcreteType : Given_ContainerAndBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_Throws()
        {
            this.builder.ForConcreteType<AbstractClass>();
        }
    }
}