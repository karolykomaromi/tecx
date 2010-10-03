using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.Registration
{
    public class ContainerExtensionOptionsBuilder : IFluentInterface
    {
        private Action<Type, IUnityContainer> _registrator = (t, c) => { };
        private readonly Func<UnityContainerExtension> _extensionFactory;

        public ContainerExtensionOptionsBuilder(Func<UnityContainerExtension> extensionFactory)
        {
            Guard.AssertNotNull(extensionFactory, "extensionFactory");

            _extensionFactory = extensionFactory;

            Action<Type, IUnityContainer> registrator = (type, container) =>
                                                            {
                                                                UnityContainerExtension extension =
                                                                    _extensionFactory();

                                                                Guard.AssertNotNull(extension, "extension");

                                                                container.AddExtension(extension);
                                                            };

            _registrator = registrator;
        }

        public ContainerExtensionOptionsBuilder WithConfiguration<TExtensionConfig>(
            Action<TExtensionConfig> configuration)
            where TExtensionConfig : IUnityContainerExtensionConfigurator
        {
            Func<IUnityContainer, TExtensionConfig> getConfigurator =
                container => container.Configure<TExtensionConfig>();

            Action<Type, IUnityContainer> registrator = (type, container) =>
                                                            {
                                                                UnityContainerExtension extension = 
                                                                    _extensionFactory();

                                                                Guard.AssertNotNull(extension, "extension");

                                                                 container.AddExtension(extension);

                                                                 configuration(getConfigurator(container));
                                                             };

            _registrator = registrator;

            return this;
        }

        public ContainerExtensionOptions Build()
        {
            return new ContainerExtensionOptions(_registrator);
        }

        public static implicit operator ContainerExtensionOptions(ContainerExtensionOptionsBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Build();
        }
    }
}