using System;
using System.Globalization;
using System.Windows.Data;

namespace TecX.Agile.View.Converter
{
    public class StringToDoubleConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string inputString = value as string;

            if (!string.IsNullOrEmpty(inputString))
            {
                double inputNumber;
                if (double.TryParse(inputString, NumberStyles.Float, culture, out inputNumber))
                {
                    return inputNumber;
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return value.ToString();

            return string.Empty;
        }

        #endregion
    }
}
