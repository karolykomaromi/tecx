namespace TecX.Agile.Registration
{
    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.ViewModels;
    using TecX.CaliburnEx;
    using TecX.Unity.Configuration;

    public class CaliburnConfigurationBuilder : ConfigurationBuilder
    {
        public CaliburnConfigurationBuilder()
        {
            For<ShellViewModel>().Use<ShellViewModel>().AsSingleton();
            For<IShell>().Use<ShellViewModel>().AsSingleton();
            For<IWindowManager>().Use<WindowManager>().AsSingleton();

            MessageBinder.CustomConverters.Add(typeof(ScrollChangedMessageParameter), new ScrollChangedConverter().Convert);
        }
    }
}
