namespace Hydra.Configuration
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.I18n;
    using Microsoft.Practices.Unity;

    public class ResourcesConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IResxPropertyConvention>(
                new InjectionFactory(_ => new CompositeConvention(new FindByTypeName(), new FindByTypeFullName())));

            this.Container.RegisterType<IResourceAccessorCache, ResourceAccessorCache>();

            Enumeration.SetResourceAccessorCache(new LazyResourceAccessorCache(() => this.Container.Resolve<IResourceAccessorCache>()));

            foreach (Type resourceFile in AllClasses.FromLoadedAssemblies().Where(IsResourceFile))
            {
                PropertyInfo property = GetResourceManagerProperty(resourceFile);

                IResourceManager resourceManager = new ResourceManagerWrapper(new ResourceManager(resourceFile.FullName, resourceFile.Assembly));

                property.SetValue(null, resourceManager);
            }
        }

        private static bool IsResourceFile(Type implementationType)
        {
            bool isResourceFile = (implementationType.Namespace ?? string.Empty).IndexOf("Assets", StringComparison.OrdinalIgnoreCase) > -1 &&
                                  GetResourceManagerProperty(implementationType) != null;

            return isResourceFile;
        }

        private static PropertyInfo GetResourceManagerProperty(Type implementationType)
        {
            return implementationType.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.Static);
        }
    }
}