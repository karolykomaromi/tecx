namespace TecX.ServiceModel.Unity.AutoMagic
{
    using System;

    public class ProxyCreationFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyCreationFailedException"/> class.
        /// </summary>
        /// <param name="message">The error message.</param>
        public ProxyCreationFailedException(string message)
            : base(message)
        {
        }
    }
}