namespace Infrastructure.UnityExtensions
{
    using Microsoft.Practices.ObjectBuilder2;
    using Microsoft.Practices.Prism.Regions;

    public class RegionStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            var key = context.BuildKey;

            if (key.Type == typeof(INavigateAsync))
            {
                IRegionManager regionManager = context.NewBuildUp<IRegionManager>();

                context.Existing = regionManager.Regions[key.Name];
            }
        }
    }
}