namespace Infrastructure.Client.Test
{
    using Infrastructure.Client.Test.TestObjects;

    using Microsoft.Practices.Prism.Logging;
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
            var resources = new Mock<IApplicationResources>();

            var vm = new ViewModelResolutionFailsModule(container, regionManager.Object, logger.Object, resources.Object);

            vm.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_View()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();
            var resources = new Mock<IApplicationResources>();

            var vm = new ViewResolutionFailsModule(container, regionManager.Object, logger.Object, resources.Object);

            vm.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Warning_On_View_Type_Not_Found()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();
            var resources = new Mock<IApplicationResources>();

            var vm = new NoViewModule(container, regionManager.Object, logger.Object, resources.Object);

            vm.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Warn, Priority.High));
        }
    }
}
