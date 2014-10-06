namespace TecX.Unity.Enums
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class EnumExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new EnumStrategy();
            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}