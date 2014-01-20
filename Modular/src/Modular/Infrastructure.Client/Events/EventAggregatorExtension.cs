namespace Infrastructure.Events
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class EventAggregatorExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new EventAggregatorStrategy(), UnityBuildStage.Initialization);
        }
    }
}
