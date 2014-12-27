namespace Hydra.Import
{
    using System.Globalization;

    public class NullValueWriter : IValueWriter
    {
        public string PropertyName
        {
            get { return string.Empty; }
        }

        public ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            return ImportMessage.Empty;
        }
    }
}