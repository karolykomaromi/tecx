namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class NullValueWriter : ValueWriter
    {
        private readonly string propertyName;

        public NullValueWriter(string propertyName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(propertyName));

            this.propertyName = propertyName;
        }

        public override string PropertyName
        {
            get { return this.propertyName; }
        }

        public override ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            return ImportMessage.Empty;
        }
    }
}