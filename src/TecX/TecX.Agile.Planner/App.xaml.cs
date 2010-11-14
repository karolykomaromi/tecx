using System;
using System.Windows;

using Microsoft.Practices.Prism.UnityExtensions;

namespace TecX.Agile.Planner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if (DEBUG)
            RunInDebugMode();
#else
            RunInReleaseMode();
#endif
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private static void RunInDebugMode()
        {
            new Bootstrapper().Run();
        }

        private static void RunInReleaseMode()
        {
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;

            try
            {
                new Bootstrapper().Run();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        private static void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            HandleException(e.ExceptionObject as Exception);
        }

        private static void HandleException(Exception ex)
        {
            if (ex == null)
                return;

            //TODO weberse evaluate wether to use the exception handling block
            //ExceptionPolicy.HandleException(ex, "Default Policy");
            //MessageBox.Show(StockTraderRI.Properties.Resources.UnhandledException);
            Environment.Exit(1);
        }
    }
}