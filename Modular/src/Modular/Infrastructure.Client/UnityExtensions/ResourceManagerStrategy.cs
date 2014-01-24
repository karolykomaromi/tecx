namespace Infrastructure.UnityExtensions
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Microsoft.Practices.ObjectBuilder2;

    public class ResourceManagerStrategy : BuilderStrategy
    {
        public override void PreBuildUp(IBuilderContext context)
        {
            ViewModel vm = context.Existing as ViewModel;

            if (vm != null)
            {
                vm.ResourceManager = context.NewBuildUp<IResourceManager>();
            }
        }
    }
}