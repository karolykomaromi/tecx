using System;
using System.Windows.Data;
using System.Windows.Media;

namespace TecX.Agile.Infrastructure.Converters
{
    public class ColorToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
                return null;

            if (value is Color)
            {
                Color color = (Color)value;
                return new SolidColorBrush(color);
            }

            Type type = value.GetType();
            throw new NotSupportedException("Unsupported type [" + type.Name + "]");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (null == value)
                return null;

            if (value is SolidColorBrush)
            {
                SolidColorBrush brush = (SolidColorBrush)value;

                return brush.Color;
            }

            Type type = value.GetType();
            throw new NotSupportedException("Unsupported type [" + type.Name + "]");
        }
    }
}
