using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;

namespace TecX.Agile.Planner
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();

            Application.Current.RootVisual = shell;

            return shell;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance<Dispatcher>(Deployment.Current.Dispatcher,
                                                   new ContainerControlledLifetimeManager());
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            IModuleCatalog catalog = new ModuleCatalog();

            return catalog;
        }

        protected override void ConfigureModuleCatalog()
        {
            //base.ConfigureModuleCatalog();

            //ModuleInfo main = new ModuleInfo(Modules.Main.Module.ModuleName, typeof(Modules.Main.Module).AssemblyQualifiedName);

            //ModuleCatalog.AddModule(main);
        }
    }
}
