namespace TecX.Unity.Test.TestObjects
{
    public class UsesLog
    {
        public ILog Log { get; set; }

        public UsesLog(ILog log)
        {
            Log = log;
        }
    }
}