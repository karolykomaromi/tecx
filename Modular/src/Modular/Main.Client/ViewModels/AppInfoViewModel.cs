namespace Main.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Infrastructure.Modularity;
    using Infrastructure.ViewModels;

    public class AppInfoViewModel : ViewModel
    {
        private readonly IModuleTracker moduleTracker;

        public AppInfoViewModel(IModuleTracker moduleTracker)
        {
            Contract.Requires(moduleTracker != null);

            this.moduleTracker = moduleTracker;
        }

        public IEnumerable<string> LoadedAssemblies
        {
            get
            {
                if (DesignerProperties.IsInDesignTool)
                {
                    return new string[0];
                }

                return AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName);
            }
        }

        public IEnumerable<ModuleTrackingState> Modules
        {
            get { return this.moduleTracker.ModuleTrackingStates; }
        }

        public void Refresh()
        {
            this.OnPropertyChanged(() => this.LoadedAssemblies);
            this.OnPropertyChanged(() => this.Modules);
        }
    }
}
