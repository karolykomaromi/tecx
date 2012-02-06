namespace TecX.Unity.Configuration.Expressions
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ConfigureContainerExtensionExpression
    {
        private readonly UnityContainerExtension extension;

        private readonly List<Action<UnityContainerExtension>> alternations;

        public ConfigureContainerExtensionExpression(ConfigurationBuilder builder, UnityContainerExtension extension)
        {
            Guard.AssertNotNull(builder, "builder");
            Guard.AssertNotNull(extension, "extension");

            this.extension = extension;
            this.alternations = new List<Action<UnityContainerExtension>>();

            builder.AddExpression(config =>
                {
                    config.AddExtension(this.extension);

                    this.alternations.ForEach(action => action(this.extension));
                });
        }

        public void Using<TExtensionConfigurator>(Action<TExtensionConfigurator> action)
            where TExtensionConfigurator : class, IUnityContainerExtensionConfigurator
        {
            if (!(this.extension is TExtensionConfigurator))
            {
                throw new ArgumentException("The extension you try to configure does not implement the specified interface.", "TExtensionConfigurator");
            }

            this.alternations.Add(ext => action(this.extension as TExtensionConfigurator));
        }

        public void Using(Action<UnityContainerExtension> action)
        {
            Guard.AssertNotNull(action, "action");

            this.alternations.Add(action);
        }
    }
}
