using System;

using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    public class ContainerExtensionOptionsBuilder
    {
        private Action<Type, IUnityContainer> _registrator = (t, c) => { };
        private readonly Type _extensionType;

        public ContainerExtensionOptionsBuilder(Type extensionType)
        {
            Guard.AssertNotNull(extensionType, "extensionType");

            _extensionType = extensionType;
        }

        public ContainerExtensionOptionsBuilder WithConfiguration<TExtensionConfig>(
            Action<TExtensionConfig> configuration)
            where TExtensionConfig : IUnityContainerExtensionConfigurator
        {
            Func<IUnityContainer, TExtensionConfig> getConfigurator =
                container => container.Configure<TExtensionConfig>();

            Action<Type, IUnityContainer> registrator = (t, c) =>
                                                             {
                                                                 //resolve the extension using the container
                                                                 UnityContainerExtension extension =
                                                                     (UnityContainerExtension)c.Resolve(_extensionType);

                                                                 c.AddExtension(extension);
                                                                 //configure the extension
                                                                 configuration(getConfigurator(c));
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