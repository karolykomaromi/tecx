namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;

    public static class DefaultSettings
    {
        private static readonly Lazy<IReadOnlyList<Setting>> AllDefaultSettings = new Lazy<IReadOnlyList<Setting>>(GetAllDefaultSettings);

        public static IReadOnlyList<Setting> All()
        {
            Contract.Ensures(Contract.Result<IReadOnlyList<Setting>>() != null);

            return AllDefaultSettings.Value;
        }

        private static IReadOnlyList<Setting> GetAllDefaultSettings()
        {
            Contract.Ensures(Contract.Result<IReadOnlyList<Setting>>() != null);

            return new ReadOnlyCollection<Setting>(GetForType(typeof(DefaultSettings)).OrderBy(s => s).ToList());
        }

        private static IEnumerable<Setting> GetForType(Type type)
        {
            Contract.Ensures(Contract.Result<IEnumerable<Setting>>() != null);

            foreach (Type nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
            {
                foreach (Setting setting in GetForType(nestedType))
                {
                    yield return setting;
                }
            }

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(Setting))
                {
                    yield return (Setting)field.GetValue(null);
                }
            }
        }

        public static class Hydra
        {
            public static class Mail
            {
                public static class Smtp
                {
                    public static readonly Setting Host = new Setting(KnownSettingNames.Hydra.Mail.Smtp.Host, "smtp.mail.invalid");

                    public static readonly Setting Port = new Setting(KnownSettingNames.Hydra.Mail.Smtp.Port, 587);

                    public static readonly Setting UserName = new Setting(KnownSettingNames.Hydra.Mail.Smtp.UserName, "john.wayne@mail.invalid");

                    public static readonly Setting Password = new Setting(KnownSettingNames.Hydra.Mail.Smtp.Password, "not-a-password");

                    public static readonly Setting IsAuthenticationRequired = new Setting(KnownSettingNames.Hydra.Mail.Smtp.IsAuthenticationRequired, false);
                }
            }

            public static class Infrastructure
            {
                public static class Configuration
                {
                    public static readonly Setting Test = new Setting(KnownSettingNames.Hydra.Infrastructure.Configuration.Test, 1);
                }
            }
        }
    }
}
