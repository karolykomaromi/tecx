using System;

namespace TecX.Unity.Configuration.Conventions
{
    public class FindRegistriesConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (Registry.IsPublicRegistry(type))
            {
                registry.Configure(graph => graph.ImportRegistry(type));
            }
        }
    }
}
