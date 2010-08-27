using System;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common.EntLib;

namespace TecX.Common.Test
{
    [TestClass]
    public class FluentLoggingConfigFixture
    {
        [TestMethod]
        public void CanConfigureLogger()
        {
            var builder = new ConfigurationSourceBuilder();

            builder.ConfigureLogging()
                .LogToCategoryNamed("General")
                .WithOptions
                .SetAsDefaultCategory()
                .SendTo
                .Custom("DefaultTraceListener", typeof (InMemoryTraceListener))
                .FormatWith(
                    new FormatterBuilder()
                        .TextFormatterNamed("DefaultTextFormatter")
                        .UsingTemplate("{timestamp} - {category} - {severity} : {message}"));

            var configSource = new DictionaryConfigurationSource();
            builder.UpdateConfigurationWithReplace(configSource);

            EnterpriseLibraryContainer.Current
                = EnterpriseLibraryContainer.CreateDefaultContainer(configSource);

            LogSource generalLogSource = Logger.Writer.TraceSources["General"];

            Assert.IsNotNull(generalLogSource);

            var wrapper = generalLogSource.Listeners[0] as ReconfigurableTraceListenerWrapper;

            Assert.IsNotNull(wrapper);

            InMemoryTraceListener listener = wrapper.InnerTraceListener as InMemoryTraceListener;

            Assert.IsNotNull(listener);

            Logger.Write("TestTestTest");

            Assert.IsTrue(listener.Messages.Contains("TestTestTest"));

            Logger.Write(new Exception("absolutely fatal error!"));

            Assert.IsTrue(listener.Messages.Contains("absolutely fatal error!"));
        }
    }
}