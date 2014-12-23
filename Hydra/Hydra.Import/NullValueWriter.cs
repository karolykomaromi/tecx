namespace Hydra.Import
{
    using System.Globalization;

    public class NullValueWriter : IValueWriter
    {
        public string PropertyName
        {
            get { return string.Empty; }
        }

        public ImportMessage Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            return ImportMessage.Empty;
        }
    }
}