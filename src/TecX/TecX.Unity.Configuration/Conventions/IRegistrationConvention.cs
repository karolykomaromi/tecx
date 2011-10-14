using System;

namespace TecX.Unity.Configuration.Conventions
{
    public interface IRegistrationConvention
    {
        void Process(Type type, ConfigurationBuilder builder);
    }
}
