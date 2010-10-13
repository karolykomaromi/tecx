namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    internal class SomeService
    {
        public readonly ILogger Logger;

        public SomeService(ILogger logger)
        {
            Logger = logger;
        }
    }
}