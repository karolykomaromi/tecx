namespace Hydra.Import
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [ContractClass(typeof(ValueWriterContract))]
    public interface IValueWriter
    {
        string PropertyName { get; }

        void Write(object instance, string value, CultureInfo source, CultureInfo target);
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

        public void Write(object instance, string value, CultureInfo source, CultureInfo target)
        {
            Contract.Requires(instance != null);
            Contract.Requires(source != null);
            Contract.Requires(target != null);
        }
    }
}