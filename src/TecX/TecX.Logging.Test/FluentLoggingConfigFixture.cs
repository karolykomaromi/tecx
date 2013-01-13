using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Logging.Test.TestObjects;

namespace TecX.Logging.Test
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
                .Custom("DefaultTraceListener", typeof (MemoryTraceListener))
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

            MemoryTraceListener listener = wrapper.InnerTraceListener as MemoryTraceListener;

            Assert.IsNotNull(listener);

            Logger.Write("TestTestTest");

            Assert.IsTrue(listener.Messages.Contains("TestTestTest"));

            Logger.Write(new Exception("absolutely fatal error!"));

            Assert.IsTrue(listener.Messages.Contains("absolutely fatal error!"));
        }
    }
}