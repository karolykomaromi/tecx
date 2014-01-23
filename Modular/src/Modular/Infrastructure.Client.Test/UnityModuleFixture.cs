namespace Infrastructure.Client.Test
{
    using Infrastructure.Client.Test.TestObjects;
    using Infrastructure.I18n;
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
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();
            var resources = new Mock<IAppResourceAppender>();
            var resourceManager = new Mock<IResourceManager>();

            IModule module = new ViewModelResolutionFailsModule(container, regionManager.Object, logger.Object, resources.Object, resourceManager.Object);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_View()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();
            var resources = new Mock<IAppResourceAppender>();
            var resourceManager = new Mock<IResourceManager>();

            IModule module = new ViewResolutionFailsModule(container, regionManager.Object, logger.Object, resources.Object, resourceManager.Object);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Warning_On_View_Type_Not_Found()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();
            var resources = new Mock<IAppResourceAppender>();
            var resourceManager = new Mock<IResourceManager>();

            IModule module = new NoViewModule(container, regionManager.Object, logger.Object, resources.Object, resourceManager.Object);

            module.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Warn, Priority.High));
        }
    }
}
