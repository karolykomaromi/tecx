using System;

namespace TecX.Unity.Configuration.Conventions
{
    public interface IRegistrationConvention
    {
        void Process(Type type, Registry registry);
    }
}
