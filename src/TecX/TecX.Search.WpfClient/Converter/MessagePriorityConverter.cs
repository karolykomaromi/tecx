namespace TecX.Search.WpfClient.Converter
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class MessagePriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                return "P0" + (int)value;
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}