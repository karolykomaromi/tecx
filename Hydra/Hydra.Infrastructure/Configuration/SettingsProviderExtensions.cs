namespace Hydra.Infrastructure.Configuration
{
    using System.Diagnostics.Contracts;

    public static class SettingsProviderExtensions
    {
        public static T GetValue<T>(this ISettingsProvider provider, SettingName settingName)
        {
            Contract.Requires(provider != null);
            Contract.Requires(settingName != null);

            object value = provider.GetSettings()[settingName];

            if (value is T)
            {
                return (T)value;
            }

            return Default.Value<T>();
        }
    }
}