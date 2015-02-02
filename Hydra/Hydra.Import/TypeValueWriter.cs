namespace Hydra.Import
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Hydra.Infrastructure.Logging;

    public class TypeValueWriter : PropertyValueWriter
    {
        public TypeValueWriter(PropertyInfo property)
            : base(property)
        {
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return ImportMessage.Empty;
            }

            Type t = Type.GetType(value, false, true);

            if (t == null)
            {
                return new Warning(string.Format(Properties.Resources.CouldNotParseValue, value, typeof(Type).FullName));
            }

            try
            {
                this.Property.SetValue(target, t);
            }
            catch (Exception ex)
            {
                HydraEventSource.Log.Error(ex);

                return new Error(string.Format(Properties.Resources.ErrorWritingValue, t.AssemblyQualifiedName, this.Property.Name));
            }

            return ImportMessage.Empty;
        }
    }
}