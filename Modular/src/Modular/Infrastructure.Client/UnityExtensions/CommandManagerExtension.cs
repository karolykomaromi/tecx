namespace Infrastructure.UnityExtensions
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class CommandManagerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new CommandManagerStrategy(), UnityBuildStage.Initialization);
        }
    }
}
