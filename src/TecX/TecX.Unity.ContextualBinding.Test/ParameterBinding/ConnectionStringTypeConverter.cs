namespace TecX.Unity.ContextualBinding.Test.ParameterBinding
{
    using System.ComponentModel;
    using System.Configuration;

    public class ConnectionStringTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return null;
            }

            string connectionStringName = (string)value;

            var settings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (settings == null)
            {
                throw new ConfigurationErrorsException(string.Format("No ConnectionString named '{0}' found in config file.", connectionStringName));
            }

            return settings.ConnectionString;
        }
    }
}
