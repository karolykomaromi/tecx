namespace TecX.ServiceModel.Unity.AutoMagic
{
    using System;

    public class MetadataExportFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataExportFailedException"/> class
        /// </summary>
        public MetadataExportFailedException(string message)
            : base(message)
        {
        }
    }
}