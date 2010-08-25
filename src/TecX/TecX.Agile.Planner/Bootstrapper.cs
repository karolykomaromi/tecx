using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Composite.UnityExtensions;

namespace TecX.Agile.Planner
{
    public class Bootstrapper : UnityBootstrapper
    {
        #region Overrides of UnityBootstrapper

        /// <summary>
        /// Creates the shell or main window of the application.
        /// </summary>
        /// <returns>
        /// The shell of the application.
        /// </returns>
        /// <remarks>
        /// If the returned instance is a <see cref="T:System.Windows.DependencyObject"/>, the
        ///             <see cref="T:Microsoft.Practices.Composite.UnityExtensions.UnityBootstrapper"/> will attach the default <seealso cref="T:Microsoft.Practices.Composite.Regions.IRegionManager"/> of
        ///             the application in its <see cref="F:Microsoft.Practices.Composite.Presentation.Regions.RegionManager.RegionManagerProperty"/> attached property
        ///             in order to be able to add regions by using the <seealso cref="F:Microsoft.Practices.Composite.Presentation.Regions.RegionManager.RegionNameProperty"/>
        ///             attached property from XAML.
        /// </remarks>
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
        }

        #endregion
    }
}
