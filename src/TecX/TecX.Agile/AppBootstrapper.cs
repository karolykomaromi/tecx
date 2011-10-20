using TecX.Agile.Infrastructure;
using TecX.Agile.ViewModels;

namespace TecX.Agile
{
    using TecX.Agile.Infrastructure.Modularization;

    public class AppBootstrapper : UnityBootstrapper<ShellViewModel>
    {
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var gestures = typeof(Modules.Gestures.Module);

            ModuleCatalog.AddModule(new ModuleInfo { Name = gestures.Name, ModuleType = gestures.AssemblyQualifiedName });

            var main = typeof(Modules.Main.Module);

            ModuleCatalog.AddModule(new ModuleInfo { Name = main.Name, ModuleType = main.AssemblyQualifiedName });
        }
    }
}
