namespace TecX.Common.Event
{
    public interface ISubscribeTo<in TMessage>
    {
        void Handle(TMessage message);
    }
}