using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Agile.ViewModels;
using TecX.Unity.Configuration;

namespace TecX.Agile.Registration
{
    public class CaliburnConfigurationBuilder : ConfigurationBuilder
    {
        public CaliburnConfigurationBuilder()
        {
            For<ShellViewModel>().Use<ShellViewModel>().AsSingleton();
            For<IShell>().Use<ShellViewModel>().AsSingleton();
            For<IWindowManager>().Use<WindowManager>().AsSingleton();
        }
    }
}
