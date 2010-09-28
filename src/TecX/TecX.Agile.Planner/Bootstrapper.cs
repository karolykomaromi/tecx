using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.UnityExtensions;

using TecX.Common.Event.Unity;
using TecX.Unity.Registration;
using TecX.Agile.Data;

namespace TecX.Agile.Planner
{
    public class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of UnityBootstrapper

        protected override DependencyObject CreateShell()
        {
            Shell shell = Container.Resolve<Shell>();

            return shell;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            //TODO weberse configure logging, maybe wcf automagic, a repository and all the other funny
            //Stuff

            Container
                .ConfigureRegistrations()
                .ExcludeSystemAssemblies()
                .Include(The.Extension<EventAggregatorContainerExtension>())
                .Include(If.Implements<IRepository>(), 
                    Then.Register().WithoutPartName(WellKnownAppParts.DesignPatterns.Repository))
                .ApplyRegistrations();
        }

        protected override IModuleCatalog GetModuleCatalog()
        {
            IModuleCatalog catalog = new ModuleCatalog();

            return catalog;
        }

        #endregion
    }
}
