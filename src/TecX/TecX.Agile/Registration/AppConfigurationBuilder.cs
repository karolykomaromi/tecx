using Microsoft.Practices.Unity;

using TecX.Common.Event.Unity;
using TecX.Unity.Configuration;

namespace TecX.Agile.Registration
{
    public class AppConfigurationBuilder : ConfigurationBuilder
    {
        public AppConfigurationBuilder()
        {
            AddExpression(x => x.AddModification(container => container.AddNewExtension<EventAggregatorExtension>()));
        }
    }
}
