namespace Hydra.Infrastructure
{
    using System;

    public class InvalidEnumerationException : InvalidOperationException
    {
        public InvalidEnumerationException(string message)
            : base(message)
        {
        }
    }
}