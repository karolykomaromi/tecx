namespace TecX.Unity.Literals
{
    using System;
    using System.Configuration;

    using Microsoft.Practices.ObjectBuilder2;

    using TecX.Common;

    using Convert = System.Convert;

    public class AppSettingsResolverPolicy : IDependencyResolverPolicy
    {
        private readonly string name;

        private readonly Type targetType;

        public AppSettingsResolverPolicy(string name, Type targetType)
        {
            Guard.AssertNotEmpty(name, "name");
            Guard.AssertNotNull(targetType, "targetType");

            this.name = name;
            this.targetType = targetType;
        }

        public object Resolve(IBuilderContext context)
        {
            Guard.AssertNotNull(context, "context");

            string setting = ConfigurationManager.AppSettings[this.name];

            return Convert.ChangeType(setting, this.targetType);
        }
    }
}