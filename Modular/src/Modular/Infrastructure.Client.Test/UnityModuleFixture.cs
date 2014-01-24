namespace Infrastructure.Client.Test
{
    using Infrastructure.Client.Test.TestObjects;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using IModuleInitializer = Infrastructure.Modularity.IModuleInitializer;

    [TestClass]
    public class UnityModuleFixture
    {
        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_ViewModel()
        {
            IUnityContainer container = new UnityContainer();
            var logger = new Mock<ILoggerFacade>();
            var initializer = new Mock<IModuleInitializer>();

            IModule module = new ViewModelResolutionFailsModule(container, logger.Object, initializer.Object);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_View()
        {
            IUnityContainer container = new UnityContainer();
            var logger = new Mock<ILoggerFacade>();
            var initializer = new Mock<IModuleInitializer>();

            IModule module = new ViewResolutionFailsModule(container, logger.Object, initializer.Object);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Warning_On_View_Type_Not_Found()
        {
            IUnityContainer container = new UnityContainer();
            var logger = new Mock<ILoggerFacade>();
            var initializer = new Mock<IModuleInitializer>();

            IModule module = new NoViewModule(container, logger.Object, initializer.Object);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Warn, Priority.High));
        }
    }
}
