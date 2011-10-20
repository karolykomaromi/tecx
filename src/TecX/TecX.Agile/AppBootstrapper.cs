using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Modularization;
using TecX.Agile.ViewModels;

namespace TecX.Agile
{
    public class AppBootstrapper : UnityBootstrapper<ShellViewModel>
    {
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var gestures = typeof(Modules.Gestures.Module);

            ModuleCatalog.AddModule(new ModuleInfo(gestures));

            var main = typeof(Modules.Main.Module);

            ModuleCatalog.AddModule(new ModuleInfo(main));
        }
    }
}
