namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    [Serializable]
    public class MissingKeyException : KeyNotFoundException
    {
        private string missingKey;

        public MissingKeyException(KeyNotFoundException innerException, object missingKey)
        {
            Contract.Requires(missingKey != null);
            Contract.Requires(!string.IsNullOrWhiteSpace(missingKey.ToString()));

            new Switch(missingKey)
                .Case<string>(s => this.missingKey = s)
                .Case<IConvertible>(convertible => this.missingKey = convertible.ToString(CultureInfo.CurrentCulture))
                .Case<IFormattable>(formattable => this.missingKey = formattable.ToString(string.Empty, CultureInfo.CurrentCulture))
                .Default(o => this.missingKey = o.ToString());
        }

        public string MissingKey
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                return this.missingKey;
            }
        }
    }
}