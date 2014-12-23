namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;

    public class Int32ValueWriter : PropertyValueWriter
    {
        public Int32ValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            int i;
            if (int.TryParse(value, NumberStyles.Number | NumberStyles.Integer, source, out i))
            {
                try
                {
                    this.Property.SetValue(instance, i);

                    return ImportMessage.Empty;
                }
                catch (Exception ex)
                {
                    HydraEventSource.Log.Error(ex);

                    return new Error(string.Format(Properties.Resources.ErrorWritingValue, i.ToString(CultureInfo.CurrentUICulture), this.Property.Name));
                }
            }

            return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(int).FullName));
        }
    }
}