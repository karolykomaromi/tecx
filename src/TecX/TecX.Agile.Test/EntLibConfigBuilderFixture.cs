namespace TecX.Agile.Test
{
    using System;

    using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
    using Microsoft.Practices.EnterpriseLibrary.Logging;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Agile.Registration;
    using TecX.TestTools;

    public abstract class Given_Container : GivenWhenThen
    {
        protected IUnityContainer container;

        protected override void Given()
        {
            this.container = new UnityContainer();
        }
    }

    [TestClass]
    public class When_AddingEntLibRegistry : Given_Container
    {
        protected override void When()
        {
            container.AddNewExtension<EnterpriseLibraryConfigurationBuilder>();
        }

        [TestMethod]
        public void Then_CanResolveLogWriter()
        {
            var writer = container.Resolve<LogWriter>();

            Assert.IsNotNull(writer);

            writer.Write("blabla");
        }

        [TestMethod]
        public void Then_CanHandleExceptions()
        {
            var exMngr = container.Resolve<ExceptionManager>();

            Assert.IsNotNull(exMngr);

            Exception ex = new Exception();

            Exception toThrow;
            bool handle = exMngr.HandleException(ex, "General", out toThrow);

            Assert.IsTrue(handle);
            Assert.IsNull(toThrow);
        }
    }
}
