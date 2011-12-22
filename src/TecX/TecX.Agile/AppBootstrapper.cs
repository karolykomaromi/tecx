namespace TecX.Agile
{
    using System.Collections.Generic;
    using System.Reflection;

    using TecX.Agile.ViewModels;
    using TecX.CaliburnEx;
    using TecX.CaliburnEx.Modularization;

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
                    Assembly.GetEntryAssembly(),
                    typeof(Modules.Gestures.GestureModule).Assembly,
                    typeof(Modules.Main.MainModule).Assembly
                };
        }
    }
}
