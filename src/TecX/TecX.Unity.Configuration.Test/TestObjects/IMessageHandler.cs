namespace TecX.Unity.Configuration.Test.TestObjects
{
    public interface IMessageHandler<TMessage>
    {
        void Handle(TMessage message);
    }
}