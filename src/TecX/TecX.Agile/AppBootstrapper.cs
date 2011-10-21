using System.Collections.Generic;
using System.Reflection;

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

            var gestures = typeof(Modules.Gestures.GestureModule);

            ModuleCatalog.AddModule(new ModuleInfo(gestures));

            var main = typeof(Modules.Main.MainModule);

            ModuleCatalog.AddModule(new ModuleInfo(main));
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[]
                {
                    Assembly.GetExecutingAssembly(), 
                    typeof(Modules.Gestures.GestureModule).Assembly,
                    typeof(Modules.Main.MainModule).Assembly
                };
        }
    }
}
