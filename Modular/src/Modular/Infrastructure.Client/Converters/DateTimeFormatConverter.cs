namespace Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;

                return dt.ToString("d", culture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}