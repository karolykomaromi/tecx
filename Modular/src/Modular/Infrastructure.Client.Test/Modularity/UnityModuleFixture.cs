namespace Infrastructure.Client.Test.Modularity
{
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.Modularity;
    using Microsoft.Practices.Prism.Logging;
    using Microsoft.Practices.Prism.Modularity;
    using Microsoft.Practices.Prism.Regions;
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class UnityModuleFixture
    {
        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_ViewModel()
        {
            IUnityContainer container = new UnityContainer();
            var logger = new Mock<ILoggerFacade>();
            var regionManager = new Mock<IRegionManager>();
            var initializer = new RegionInitializer(regionManager.Object);

            IModule module = new ViewModelResolutionFailsModule(container, logger.Object, initializer);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_View()
        {
            IUnityContainer container = new UnityContainer();
            var logger = new Mock<ILoggerFacade>();
            var regionManager = new Mock<IRegionManager>();
            var initializer = new RegionInitializer(regionManager.Object);

            IModule module = new ViewResolutionFailsModule(container, logger.Object, initializer);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Warning_On_View_Type_Not_Found()
        {
            IUnityContainer container = new UnityContainer();
            var logger = new Mock<ILoggerFacade>();
            var regionManager = new Mock<IRegionManager>();
            var initializer = new RegionInitializer(regionManager.Object);

            IModule module = new NoViewModule(container, logger.Object, initializer);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Warn, Priority.High));
        }
    }
}
