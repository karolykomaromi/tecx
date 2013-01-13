namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    class OperationContext
    {
        public static OperationContext Current { get; set; }

        public MessageHeaders IncomingMessageHeaders { get; set; }
    }
}