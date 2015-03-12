namespace Hydra.Infrastructure.ServiceModel
{
    using System;

    [Serializable]
    public class EnumerationParseException : Exception
    {
        public EnumerationParseException(string message)
            : base(message)
        {
        }
    }
}