namespace TecX.Unity.Test.TestObjects
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