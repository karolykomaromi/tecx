namespace TecX.Unity.Mixin
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class MixinExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            MixinStrategy strategy = new MixinStrategy();

            Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}
