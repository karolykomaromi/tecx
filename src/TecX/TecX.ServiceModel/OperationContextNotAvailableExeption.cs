namespace TecX.ServiceModel
{
    using System;

    public class OperationContextNotAvailableExeption : Exception
    {
        public OperationContextNotAvailableExeption()
            : base("OperationContext.Current is null.")
        {
        }
    }
}