namespace TecX.Search.WpfClient.Converter
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Media;

    public class StatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                string status = (string)value;

                if (!string.IsNullOrEmpty(status) && status.StartsWith(Constants.ErrorStatusPrefix))
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }

            return SystemColors.ControlLightLightBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
