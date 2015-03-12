namespace Hydra.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MissingKeyException : KeyNotFoundException
    {
        private readonly object missingKey;

        public MissingKeyException(KeyNotFoundException innerException, object missingKey)
            : base(string.Format(Infrastructure.Properties.Resources.KeyNotFoundException, missingKey), innerException)
        {
            this.missingKey = missingKey;
        }

        public object MissingKey
        {
            get { return this.missingKey; }
        }
    }
}