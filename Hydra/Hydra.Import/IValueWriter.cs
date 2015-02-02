namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [ContractClass(typeof(ValueWriterContract))]
    public interface IValueWriter
    {
        string PropertyName { get; }

        ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture);
    }

    [ContractClassFor(typeof(IValueWriter))]
    internal abstract class ValueWriterContract : IValueWriter
    {
        public string PropertyName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return string.Empty;
            }
        }

        public ImportMessage Write(object target, string value, CultureInfo sourceCulture, CultureInfo targetCulture)
        {
            Contract.Requires(target != null);
            Contract.Requires(sourceCulture != null);
            Contract.Requires(targetCulture != null);
            Contract.Ensures(Contract.Result<ImportMessage>() != null);

            return ImportMessage.Empty;
        }
    }
}