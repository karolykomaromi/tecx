using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;

using TecX.Logging;
using TecX.Unity.Configuration;

namespace TecX.Agile.Registration
{
    public class EntLibRegistry : Registry
    {
        private static class Constants
        {
            public static class Logging
            {
                /// <summary> DefaultListener </summary>
                public const string Listener = "DefaultListener";

                /// <summary> Formatter </summary>
                public const string Formatter = "Formatter";

                /// <summary> {timestamp} - {message} {newline} </summary>
                public const string Template = @"{timestamp} - {message} {newline}";
            }

            public static class ExceptionHandling
            {
                /// <summary> General </summary>
                public const string Policy = "General";
            }
        }

        public EntLibRegistry()
        {
            this.AddConfigAction(() =>
                {
                    Container.AddNewExtension<EnterpriseLibraryCoreExtension>();

                    var builder = new ConfigurationSourceBuilder();
                                    
                    builder.ConfigureLogging()
                        .SpecialSources.AllEventsCategory.SendTo.Custom(Constants.Logging.Listener, typeof(DebugTraceListener))
                            .FormatWith(new FormatterBuilder().TextFormatterNamed(Constants.Logging.Formatter).UsingTemplate(Constants.Logging.Template))
                        .SpecialSources.LoggingErrorsAndWarningsCategory.SendTo.SharedListenerNamed(Constants.Logging.Listener)
                        .SpecialSources.UnprocessedCategory.SendTo.SharedListenerNamed(Constants.Logging.Listener);

                    builder.ConfigureExceptionHandling()
                        .GivenPolicyWithName(Constants.ExceptionHandling.Policy).ForExceptionType(typeof(Exception)).ThenNotifyRethrow();

                    var config = new DictionaryConfigurationSource();

                    builder.UpdateConfigurationWithReplace(config);

                    var configurator = new UnityContainerConfigurator(Container);

                    EnterpriseLibraryContainer.ConfigureContainer(configurator, config);

                    UnityServiceLocator locator = new UnityServiceLocator(Container);

                    EnterpriseLibraryContainer.Current = locator;
                });
        }
    }
}
