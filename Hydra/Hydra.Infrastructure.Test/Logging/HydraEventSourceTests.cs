namespace Hydra.Infrastructure.Test.Logging
{
    using Hydra.Infrastructure.Logging;
    using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Utility;
    using Xunit;

    public class HydraEventSourceTests
    {
        [Fact]
        public void Should_Be_Configured_Correctly()
        {
            EventSourceAnalyzer.InspectAll(HydraEventSource.Log);
        }
    }
}
