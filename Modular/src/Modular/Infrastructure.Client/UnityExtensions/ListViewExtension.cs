namespace Infrastructure.UnityExtensions
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class ListViewExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new ListViewStrategy(), UnityBuildStage.Initialization);
        }
    }
}
