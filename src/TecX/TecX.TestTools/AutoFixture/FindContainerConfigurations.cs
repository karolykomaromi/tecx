namespace TecX.TestTools.AutoFixture
{
    using System;
    using System.Linq;

    using Microsoft.Practices.Unity;

    public class FindContainerConfigurations : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Type[] configurationTypes = AppDomain.CurrentDomain
                                                 .GetAssemblies()
                                                 .Where(assembly => !assembly.IsDynamic)
                                                 .SelectMany(assembly => assembly.GetExportedTypes())
                                                 .Where(
                                                     type =>
                                                     typeof(UnityContainerExtension).IsAssignableFrom(type) &&
                                                     type.Name.EndsWith("Configuration",
                                                                        StringComparison.OrdinalIgnoreCase))
                                                 .ToArray();

            UnityContainerExtension[] configurations = configurationTypes.Select(type => (UnityContainerExtension)Activator.CreateInstance(type)).ToArray();

            foreach (var configuration in configurations)
            {
                this.Container.AddExtension(configuration);
            }
        }
    }
}