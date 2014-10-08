﻿namespace Hydra.Configuration
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Hydra.Infrastructure.I18n;
    using Microsoft.Practices.Unity;

    public class ResourcesConfiguration : UnityContainerExtension
    {
        protected override void Initialize()
        {
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