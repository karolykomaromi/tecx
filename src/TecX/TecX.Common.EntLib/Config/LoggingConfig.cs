using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace TecX.Common.EntLib.Config
{
    public class LoggingConfig
    {
        public void Config()
        {
            var builder = new ConfigurationSourceBuilder();

            builder.ConfigureLogging()
            .LogToCategoryNamed("General")
                .WithOptions
                .SetAsDefaultCategory()
                .SendTo
                    .Custom("DefaultTraceListener", typeof(InMemoryTraceListener))
                .FormatWith(
                    new FormatterBuilder()
                        .TextFormatterNamed("DefaultTextFormatter")
                        .UsingTemplate("{timestamp} - {category} - {severity} : {message}{newline}"));

            var configSource = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(configSource);
            EnterpriseLibraryContainer.Current
              = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);
        }
    }
}
