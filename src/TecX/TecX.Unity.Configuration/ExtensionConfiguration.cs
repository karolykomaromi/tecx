namespace TecX.Unity.Configuration
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ExtensionConfiguration : IContainerConfigurator
    {
        private readonly Type extensionType;

        private readonly List<Action<IUnityContainer>> actions;

        public ExtensionConfiguration(Type extensionType)
        {
            this.extensionType = extensionType;
            if (!typeof(UnityContainerExtension).IsAssignableFrom(extensionType))
            {
                throw new ArgumentException();
            }

            this.actions = new List<Action<IUnityContainer>>();
        }

        public void With<TExtensionConfigurator>(Action<TExtensionConfigurator> action)
            where TExtensionConfigurator : IUnityContainerExtensionConfigurator
        {
            Guard.AssertNotNull(action, "action");

            if (!typeof(TExtensionConfigurator).IsAssignableFrom(this.extensionType))
            {
                throw new ArgumentException();
            }

            Action<IUnityContainer> x = container =>
                {
                    Action<TExtensionConfigurator> a1 = action;

                    var extensionConfigurator = container.Configure<TExtensionConfigurator>();

                    if (extensionConfigurator == null)
                    {
                        throw new InvalidOperationException();
                    }

                    a1(extensionConfigurator);
                };

            this.actions.Add(x);
        }

        public void Configure(IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            this.actions.ForEach(action => action(container));
        }
    }

    public class ExtensionConfigurationCollection
    {
        public ExtensionConfiguration this[Type extensionType]
        {
            get
            {
                if (!typeof(UnityContainerExtension).IsAssignableFrom(extensionType))
                {
                    throw new ArgumentException();
                }

                // create new config if not found
                return null;
            }
        }
    }
}
