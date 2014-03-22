namespace TecX.Unity.Factories
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class DelegateFactoryExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Context.Strategies.Add(new DelegateFactoryStrategy(), UnityBuildStage.PreCreation);
        }
    }
}
