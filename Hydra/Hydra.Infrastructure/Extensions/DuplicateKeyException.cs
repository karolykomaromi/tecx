namespace Hydra.Infrastructure.Extensions
{
    using System;

    public class DuplicateKeyException : ArgumentException
    {
        private readonly object duplicateKey;

        public DuplicateKeyException(string paramName, Exception innerException, object duplicateKey)
            : base(string.Format(Infrastructure.Properties.Resources.DuplicateKeyException, duplicateKey), paramName, innerException)
        {
            this.duplicateKey = duplicateKey;
        }

        public object DuplicateKey
        {
            get
            {
                return this.duplicateKey;
            }
        }
    }
}