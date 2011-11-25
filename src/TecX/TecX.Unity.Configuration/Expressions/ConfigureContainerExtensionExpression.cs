namespace TecX.Unity.Configuration.Expressions
{
    using System;

    using Microsoft.Practices.Unity;

    using TecX.Common;

    public class ConfigureContainerExtensionExpression
    {
        private readonly ConfigurationBuilder builder;

        public ConfigureContainerExtensionExpression(ConfigurationBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            this.builder = builder;

            builder.AddExpression(config =>
                {
                });
        }

        public void With<TExtensionConfigurator>(Action<TExtensionConfigurator> action)
            where TExtensionConfigurator : IUnityContainerExtensionConfigurator
        {
            this.builder.AddExpression(
                config => config.AddModification(
                    container =>
                        {
                            TExtensionConfigurator configurator = container.Configure<TExtensionConfigurator>();

                            action(configurator);
                        }));
        }
    }
}
