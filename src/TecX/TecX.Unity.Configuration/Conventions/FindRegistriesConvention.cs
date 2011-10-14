using System;

namespace TecX.Unity.Configuration.Conventions
{
    public class FindRegistriesConvention : IRegistrationConvention
    {
        public void Process(Type type, ConfigurationBuilder builder)
        {
            if (ConfigurationBuilder.IsPublicBuilder(type))
            {
                builder.AddExpression(config => config.ImportBuilder(type));
            }
        }
    }
}
