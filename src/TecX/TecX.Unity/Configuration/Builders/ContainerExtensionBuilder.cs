namespace TecX.Unity.Configuration.Builders
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ContainerExtensionBuilder<TExtension> where TExtension : UnityContainerExtension
    {
        private readonly TExtension extension;

        private readonly List<Action<TExtension>> alternations;

        public ContainerExtensionBuilder(ConfigurationBuilder builder, TExtension extension)
        {
            Guard.AssertNotNull(builder, "builder");
            Guard.AssertNotNull(extension, "extension");

            this.extension = extension;
            this.alternations = new List<Action<TExtension>>();

            builder.AddExpression(config =>
                {
                    config.AddExtension(this.extension);

                    this.alternations.ForEach(action => action(this.extension));
                });
        }

        public void Using(Action<TExtension> action)
        {
            Guard.AssertNotNull(action, "action");

            this.alternations.Add(ext => action(this.extension));
        }
    }
}
