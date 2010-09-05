using Microsoft.Practices.Unity;

using TecX.Common;

namespace TecX.Unity.AutoRegistration
{
    /// <summary>
    /// Extension methods to various types
    /// </summary>
    public static class Extension
    {
        /// <summary>
        /// Configures auto registration - starts chain of fluent configuration
        /// </summary>
        /// <param name="container">Unity container.</param>
        /// <returns>Auto registration</returns>
        public static IAutoRegistration ConfigureAutoRegistration(this IUnityContainer container)
        {
            Guard.AssertNotNull(container, "container");

            return new AutoRegistration(container);
        }
    }
}