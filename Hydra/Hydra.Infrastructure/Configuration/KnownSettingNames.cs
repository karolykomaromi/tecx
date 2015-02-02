namespace Hydra.Infrastructure.Configuration
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class KnownSettingNames
    {
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
                throw new ArgumentException(string.Format(Properties.Resources.InvalidSettingName, settingName));
            }

            return sn;
        }

        public static class Hydra
        {
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
