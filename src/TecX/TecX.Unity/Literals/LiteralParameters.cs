namespace TecX.Unity.Literals
{
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.Unity.ObjectBuilder;

    using TecX.Common;

    public class LiteralParameters : UnityContainerExtension
    {
        private readonly ResolverConventionCollection conventions;

        public LiteralParameters()
        {
            this.conventions = new ResolverConventionCollection();
        }

        protected override void Initialize()
        {
            var strategy = new LiteralParametersStrategy(this.conventions);

            this.Context.Strategies.Add(strategy, UnityBuildStage.PreCreation);
        }

        public void AddConvention(IDependencyResolverConvention convention)
        {
            Guard.AssertNotNull(convention, "convention");

            this.conventions.AddConvention(convention);
        }
    }
}
