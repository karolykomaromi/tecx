namespace TecX.Search.WpfClient
{
    using System.Windows;

    using Microsoft.Practices.Unity;

    using TecX.Search.Data;
    using TecX.Search.Data.EF;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IUnityContainer container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.container = new UnityContainer();
            
            this.container.RegisterType<IMessageEntities, MessageEntities>(new InjectionConstructor("MessageSearch"));

            this.container.RegisterType<IMessageRepository, EFMessageRepository>();

            var window = this.container.Resolve<MainWindow>();

            window.Show();

            this.MainWindow = window;
        }
    }
}
