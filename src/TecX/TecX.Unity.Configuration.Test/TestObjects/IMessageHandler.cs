namespace TecX.Unity.Configuration.Test.TestObjects
{
    public interface IMessageHandler<in TMessage>
    {
        void Handle(TMessage message);
    }
}