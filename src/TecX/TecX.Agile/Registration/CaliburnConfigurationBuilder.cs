using Caliburn.Micro;

using TecX.Unity.Configuration;

namespace TecX.Agile.Registration
{
public class CaliburnConfigurationBuilder : ConfigurationBuilder
    {
        public CaliburnConfigurationBuilder()
        {
            For<ShellViewModel>().Use<ShellViewModel>().AsSingleton();
            For<IWindowManager>().Use<WindowManager>().AsSingleton();
        }
    }
}
