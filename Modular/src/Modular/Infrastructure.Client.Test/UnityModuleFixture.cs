using System;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Infrastructure.Client.Test
{
    [TestClass]
    public class UnityModuleFixture
    {
        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_ViewModel()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();

            var vm = new ViewModelResolutionFailsModule(container, regionManager.Object, logger.Object);

            vm.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Error_On_Failed_Resolution_Of_View()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();

            var vm = new ViewResolutionFailsModule(container, regionManager.Object, logger.Object);

            vm.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Exception, Priority.High));
        }

        [TestMethod]
        public void Should_Log_Warning_On_View_Type_Not_Found()
        {
            IUnityContainer container = new UnityContainer();
            var regionManager = new Mock<IRegionManager>();
            var logger = new Mock<ILoggerFacade>();

            var vm = new NoViewModule(container, regionManager.Object, logger.Object);

            vm.Initialize();

            logger.Verify(l => l.Log(It.IsAny<string>(), Category.Warn, Priority.High));
        }
    }

    public class ViewResolutionFailsModule : UnityModule
    {
        public ViewResolutionFailsModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
            : base(container, regionManager, logger)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            TryGetViewFor<ViewModel2>(out view);
        }
    }

    public class NoViewModule : UnityModule
    {
        public NoViewModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
            : base(container, regionManager, logger)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            TryGetViewFor<NoView>(out view);
        }
    }

    public class NoView : ViewModel
    {

    }

    public class ViewModelResolutionFailsModule : UnityModule
    {
        public ViewModelResolutionFailsModule(IUnityContainer container, IRegionManager regionManager, ILoggerFacade logger)
            : base(container, regionManager, logger)
        {
        }

        protected override void ConfigureRegions(IRegionManager regionManager)
        {
            FrameworkElement view;
            TryGetViewFor<ViewModel1>(out view);
        }
    }

    public class ViewModel2 : ViewModel
    {
    }

    public class View2 : FrameworkElement
    {
        public View2(IDisposable _)
        {
        }
    }

    public class ViewModel1 : ViewModel
    {
        public ViewModel1(IDisposable _)
        {
        }
    }

    public class View1 : FrameworkElement
    {
    }
}
