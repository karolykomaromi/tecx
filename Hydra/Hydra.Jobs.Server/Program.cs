namespace Hydra.Jobs.Server
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using Common.Logging;
    using Common.Logging.Log4Net;
    using Hydra.Infrastructure.Input;
    using Hydra.Unity;
    using Microsoft.Practices.Unity;
    using Topshelf;
    using Topshelf.Common.Logging;
    using Topshelf.Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            string p = string.Join(" ", args ?? new string[0]);

            CmdLineParameterParser parser = new CmdLineParameterParser();

            IEnumerable<CmdLineParameter> parameters = parser.Parse(p);

            IUnityContainer container = new UnityContainer()
                .AddExtension(new CompositionRoot(typeof(Program).Assembly));
            
            // tell common.logging to use log4net
            LogManager.Adapter = new Log4NetLoggerFactoryAdapter(new NameValueCollection());

            // tell topshelf to use common.logging
            HostLogger.UseLogger(new CommonLoggingConfigurator());

            HostFactory.Run(x =>
                {
                    x.Service<SchedulerServiceHost>(s =>
                        {
                            s.ConstructUsing(name => container.Resolve<SchedulerServiceHost>());
                            s.WhenStarted((svc, _) => svc.Start());
                            s.WhenStopped((svc, _) => svc.Stop());
                            s.WhenPaused((svc, _) => svc.Pause());
                            s.WhenContinued((svc, _) => svc.Continue());
                            s.WhenShutdown((svc, _) => svc.Shutdown());
                        });

                    x.StartAutomatically();

                    x.SetDisplayName(Properties.Resources.ServiceDisplayName);
                });
        }
    }
}
