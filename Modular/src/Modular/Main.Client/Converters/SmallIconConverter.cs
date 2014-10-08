namespace Main.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;

    public class SmallIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string name = value as string;

            if (!string.IsNullOrEmpty(name))
            {
                Uri uri = new Uri("/Main.Client;component/Assets/Images/" + name + "24x16.png", UriKind.Relative);

                return new BitmapImage { UriSource = uri };
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
