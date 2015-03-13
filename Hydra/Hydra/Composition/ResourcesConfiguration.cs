namespace Hydra.Composition
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Hydra.Infrastructure;
    using Hydra.Infrastructure.I18n;
    using Hydra.Infrastructure.Reflection;
    using Hydra.Unity;
    using Microsoft.Practices.Unity;

    public class ResourcesConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
            this.Container.RegisterType<IResXPropertyConvention>(
                new InjectionFactory(_ => new CompositeConvention(new FindByTypeName(), new FindByTypeFullName())));

            this.Container.RegisterType<IResourceAccessorCache, ResourceAccessorCache>();

            Enumeration.SetResourceAccessorCache(new LazyResourceAccessorCache(() => this.Container.Resolve<IResourceAccessorCache>()));

            foreach (Type resourceFile in AllTypes.FromHydraAssemblies().Where(IsResourceFile))
            {
                PropertyInfo property = GetResourceManagerProperty(resourceFile);

                IResourceManager resourceManager = new ResourceManagerWrapper(new ResourceManager(resourceFile.FullName, resourceFile.Assembly));

                property.SetValue(null, resourceManager);
            }

            SupportedCulturesProvider.Current = new CompositeSupportedCulturesProvider(SupportedCulturesProvider.Current, new WebConfigSupportedCulturesProvider());
        }

        private static bool IsResourceFile(Type implementationType)
        {
            bool isResourceFile = (implementationType.Namespace ?? string.Empty).IndexOf("Assets", StringComparison.OrdinalIgnoreCase) > -1 &&
                                  GetResourceManagerProperty(implementationType) != null;

            return isResourceFile;
        }

        private static PropertyInfo GetResourceManagerProperty(Type implementationType)
        {
            Contract.Requires(implementationType != null);
            Contract.Ensures(Contract.Result<PropertyInfo>() != null);

            return implementationType.GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.Static) ?? Property.Null;
        }
    }
}