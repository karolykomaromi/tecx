namespace TecX.Unity.Test.TestObjects
{
    internal class AnotherService
    {
        public readonly ILogger Logger;

        public AnotherService(ILogger logger)
        {
            Logger = logger;
        }
    }
}