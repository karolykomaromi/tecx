namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public static class DefaultSettings
    {
        private static readonly Lazy<IReadOnlyList<Setting>> AllDefaultSettings = new Lazy<IReadOnlyList<Setting>>(GetAllDefaultSettings);

        public static IReadOnlyList<Setting> All()
        {
            return AllDefaultSettings.Value;
        }

        private static IReadOnlyList<Setting> GetAllDefaultSettings()
        {
            return new ReadOnlyCollection<Setting>(GetForType(typeof(DefaultSettings)).OrderBy(s => s).ToList());
        }

        private static IEnumerable<Setting> GetForType(Type type)
        {
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
                    public static readonly Setting Server = new Setting(KnownSettingNames.Hydra.Mail.Smtp.Server, new Uri("http://smtp.mail.invalid:8080", UriKind.Absolute));

                    public static readonly Setting UserName = new Setting(KnownSettingNames.Hydra.Mail.Smtp.UserName, "john.wayne@mail.invalid");

                    public static readonly Setting Password = new Setting(KnownSettingNames.Hydra.Mail.Smtp.Password, "not-a-password");
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
