namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Hydra.Infrastructure.Properties;

    public static class KnownSettingNames
    {
        private static readonly Lazy<IReadOnlyList<SettingName>> AllKnownSettingNames = new Lazy<IReadOnlyList<SettingName>>(GetAllKnownSettingNames);

        public static IReadOnlyList<SettingName> All()
        {
            return AllKnownSettingNames.Value;
        }

        private static IReadOnlyList<SettingName> GetAllKnownSettingNames()
        {
            return new ReadOnlyCollection<SettingName>(GetForType(typeof(KnownSettingNames)).OrderBy(sn => sn).ToList());
        }

        private static IEnumerable<SettingName> GetForType(Type type)
        {
            foreach (Type nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
            {
                foreach (SettingName settingName in GetForType(nestedType))
                {
                    yield return settingName;
                }
            }

            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(SettingName))
                {
                    yield return (SettingName)field.GetValue(null);
                }
            }
        }

        private static SettingName GetSettingName(Expression<Func<SettingName>> selector)
        {
            MemberExpression expr = (MemberExpression)selector.Body;

            string s = expr.Member.DeclaringType.FullName;

            int i = s.IndexOf("+", StringComparison.Ordinal);

            s = s.Substring(i + 1);

            string[] names = s.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

            string settingName = string.Join(".", names.Concat(new[] { expr.Member.Name }));

            SettingName sn;
            if (!SettingName.TryParse(settingName, out sn))
            {
                throw new ArgumentException(string.Format(Resources.InvalidSettingName, settingName));
            }

            return sn;
        }

        public static class Hydra
        {
            public static class Mail
            {
                public static class Smtp
                {
                    public static readonly SettingName Server = GetSettingName(() => Server);

                    public static readonly SettingName UserName = GetSettingName(() => UserName);

                    public static readonly SettingName Password = GetSettingName(() => Password);
                }
            }

            public static class Infrastructure
            {
                public static class Configuration
                {
                    public static readonly SettingName Test = GetSettingName(() => Test);
                }
            }
        }
    }
}
