﻿namespace TecX.Agile.Registration
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
    using Microsoft.Practices.Unity;

    using TecX.Logging;
    using TecX.Unity.Configuration;

    public class EnterpriseLibraryConfigurationBuilder : ConfigurationBuilder
    {
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder",
            Justification = "Reviewed. Suppression is OK here.")]
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

        public EnterpriseLibraryConfigurationBuilder()
        {
            AddExpression(
                graph => graph.AddModification(
                    container =>
                    {
                        container.AddNewExtension<EnterpriseLibraryCoreExtension>();

                        var builder = new ConfigurationSourceBuilder();

                        builder.ConfigureLogging()
                            .SpecialSources
                                .AllEventsCategory
                                    .SendTo.Custom(Constants.Logging.Listener, typeof(DebugTraceListener))
                                    .FormatWith(new FormatterBuilder().TextFormatterNamed(Constants.Logging.Formatter)
                                    .UsingTemplate(Constants.Logging.Template))
                            .SpecialSources
                                .LoggingErrorsAndWarningsCategory
                                    .SendTo.SharedListenerNamed(Constants.Logging.Listener)
                            .SpecialSources
                                .UnprocessedCategory
                                    .SendTo.SharedListenerNamed(Constants.Logging.Listener);

                        builder.ConfigureExceptionHandling().GivenPolicyWithName(Constants.ExceptionHandling.Policy)
                            .ForExceptionType(typeof(Exception)).ThenNotifyRethrow();

                        var config = new DictionaryConfigurationSource();

                        builder.UpdateConfigurationWithReplace(config);

                        var configurator = new UnityContainerConfigurator(container);

                        EnterpriseLibraryContainer.ConfigureContainer(configurator, config);
                    }));
        }
    }
}