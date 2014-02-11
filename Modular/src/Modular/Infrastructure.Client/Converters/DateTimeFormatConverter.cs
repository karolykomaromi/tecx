namespace Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Data;

    public class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;

                // changing the (ui)culture of the current thread does obviously not effect the culture
                // of the current control and thus the converter is handed the wrong culture object
                return dt.ToString("d", Thread.CurrentThread.CurrentUICulture);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}