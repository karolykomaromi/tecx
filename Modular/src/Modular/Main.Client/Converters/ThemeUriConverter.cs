namespace Main.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ThemeUriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri themeUri = value as Uri;

            if (themeUri != null)
            {
                string uri = themeUri.ToString();

                int idx = uri.LastIndexOf("/", StringComparison.Ordinal);

                string themeName = uri.Substring(idx + 1).Replace("Theme.xaml", string.Empty);

                return themeName;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
