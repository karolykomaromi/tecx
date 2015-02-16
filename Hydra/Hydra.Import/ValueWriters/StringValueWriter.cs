namespace Hydra.Import.ValueWriters
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Import.Messages;
    using Hydra.Infrastructure.Logging;

    public class StringValueWriter : PropertyValueWriter
    {
        public StringValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (value == null)
            {
                return ImportMessage.Empty;
            }

            try
            {
                this.Property.SetValue(target, value.ToString(targetCulture));
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new Error(string.Format(Properties.Resources.ErrorWritingValue, value.ToString(targetCulture), this.Property.Name));
            }

            return ImportMessage.Empty;
        }
    }
}