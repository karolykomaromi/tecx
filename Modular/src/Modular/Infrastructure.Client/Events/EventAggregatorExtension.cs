using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Infrastructure.Events
{
    public class EventAggregatorExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new EventAggregatorStrategy(), UnityBuildStage.Initialization);
        }
    }
}
