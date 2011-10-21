using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Agile.ViewModels;
using TecX.Unity.Configuration;

namespace TecX.Agile.Registration
{
    using Microsoft.Practices.Unity;

    public class CaliburnConfigurationBuilder : ConfigurationBuilder
    {
        public CaliburnConfigurationBuilder()
        {
            For<ShellViewModel>().Use<ShellViewModel>().AsSingleton();
            For<IShell>().Use<ShellViewModel>().AsSingleton();

            //this.AddExpression(x => x.AddModification(c => c.RegisterType<ShellViewModel>(new ContainerControlledLifetimeManager())));
            //this.AddExpression(x => x.AddModification(c => c.RegisterType<IShell, ShellViewModel>()));

            For<IWindowManager>().Use<WindowManager>().AsSingleton();
        }
    }
}
