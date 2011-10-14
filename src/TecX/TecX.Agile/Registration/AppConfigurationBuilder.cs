namespace TecX.Agile.Registration
{
    using Caliburn.Micro;

    using TecX.Unity.Configuration;

    public class AppConfigurationBuilder : ConfigurationBuilder
    {
        public AppConfigurationBuilder()
        {
            For<ShellViewModel>().Use<ShellViewModel>().AsSingleton();
            For<IWindowManager>().Use<WindowManager>().AsSingleton();
        }
    }
}
