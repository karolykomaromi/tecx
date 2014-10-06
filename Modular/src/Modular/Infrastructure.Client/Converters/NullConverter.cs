﻿namespace Infrastructure.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
