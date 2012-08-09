namespace TecX.ServiceModel
{
    using System;
    using System.ServiceModel;

    using TecX.Common;

    public static class OperationContextProvider
    {
        [ThreadStatic]
        private static IOperationContext current;

        public static IOperationContext Current
        {
            get
            {
                if (current == null)
                {
                    if (OperationContext.Current == null)
                    {
                        throw new OperationContextNotAvailableExeption();
                    }

                    return new OperationContextWrapper(OperationContext.Current);
                }

                return current;
            }

            set
            {
                Guard.AssertNotNull(value, "Current");

                current = value;
            }
        }
    }
}
