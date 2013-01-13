namespace TecX.Unity.Configuration.Conventions
{
    using System;

    public class FindConfigBuildersConvention : IRegistrationConvention
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
