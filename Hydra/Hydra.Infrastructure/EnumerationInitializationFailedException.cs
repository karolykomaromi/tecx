namespace Hydra.Infrastructure
{
    using System;

    [Serializable]
    public class EnumerationInitializationFailedException : InvalidOperationException
    {
        public EnumerationInitializationFailedException()
            : base(Properties.Resources.InitializationOfEnumerationFailed)
        {
        }
    }
}