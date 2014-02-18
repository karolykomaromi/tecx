namespace Infrastructure.UnityExtensions.Injection
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class MapToRegistrationNamesExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            var strategy = new MapToRegistrationNamesStrategy();

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }
    }
}