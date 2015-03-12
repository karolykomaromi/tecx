namespace Hydra.Infrastructure
{
    using System;

    [Serializable]
    public class InvalidEnumerationException : InvalidOperationException
    {
        public InvalidEnumerationException(string message)
            : base(message)
        {
        }
    }
}