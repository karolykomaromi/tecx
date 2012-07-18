namespace TecX.Unity.Configuration.Conventions
{
    using System;

    public interface IRegistrationConvention
    {
        void Process(Type type, ConfigurationBuilder builder);
    }
}
