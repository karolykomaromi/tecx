namespace TecX.Unity.Configuration.Test.TestObjects
{
    public class Handler3 : IMessageHandler<int>
    {
        public void Handle(int message)
        {
            throw new System.NotImplementedException();
        }
    }
}