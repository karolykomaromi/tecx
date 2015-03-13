namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [Serializable]
    public class DuplicateKeyException : ArgumentException
    {
        private string duplicateKey;

        public DuplicateKeyException(string paramName, Exception innerException, object duplicateKey)
        {
            Contract.Requires(duplicateKey != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(duplicateKey.ToString()));

            new Switch(duplicateKey)
                .Case<string>(s => this.duplicateKey = s)
                .Case<IConvertible>(convertible => this.duplicateKey = convertible.ToString(CultureInfo.CurrentCulture))
                .Case<IFormattable>(formattable => this.duplicateKey = formattable.ToString(string.Empty, CultureInfo.CurrentCulture))
                .Default(o => this.duplicateKey = o.ToString());
        }

        public string DuplicateKey
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                return this.duplicateKey;
            }
        }
    }
}