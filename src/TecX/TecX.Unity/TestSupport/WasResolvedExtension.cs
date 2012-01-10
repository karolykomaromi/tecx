namespace TecX.Unity.TestSupport
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    public class WasResolvedExtension : UnityContainerExtension
    {
        private WasResolvedStrategy strategy;

        public bool WasResolved<T>()
        {
            return WasResolved<T>(null);
        }

        public bool WasResolved<T>(string name)
        {
            return this.strategy.WasResolved<T>(name);
        }

        protected override void Initialize()
        {
            this.strategy = new WasResolvedStrategy();

            this.Context.Strategies.Add(this.strategy, UnityBuildStage.Creation);
        }
    }
}
