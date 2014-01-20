using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;

namespace Infrastructure.Commands
{
    public class CommandManagerExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new CommandManagerStrategy(), UnityBuildStage.Initialization);
        }
    }
}
