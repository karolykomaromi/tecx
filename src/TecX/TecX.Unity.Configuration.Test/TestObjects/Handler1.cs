namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class Handler1 : IMessageHandler<string>
    {
        public void Handle(string message)
        {
            throw new System.NotImplementedException();
        }
    }
}