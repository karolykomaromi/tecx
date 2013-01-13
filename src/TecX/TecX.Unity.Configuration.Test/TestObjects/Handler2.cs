namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class Handler2 : IMessageHandler<int>, IMessageHandler<string>
    {
        public void Handle(int message)
        {
            throw new System.NotImplementedException();
        }

        public void Handle(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}