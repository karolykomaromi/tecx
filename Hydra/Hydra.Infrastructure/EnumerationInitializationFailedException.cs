namespace Hydra.Infrastructure
{
    using System;

    public class EnumerationInitializationFailedException : InvalidOperationException
    {
        public EnumerationInitializationFailedException()
            : base(Properties.Resources.InitializationOfEnumerationFailed)
        {
        }
    }
}