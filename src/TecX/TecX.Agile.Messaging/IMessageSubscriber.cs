namespace TecX.Agile.Messaging
{
    // TODO weberse 2011-12-21 this looks suspiciously like the eventaggregator interface. but the intent is to hide a how messages
    // are distributed to remote clients. EA is in process only.
    public interface IMessageSubscriber<in TMessage>
    {
        void Handle(TMessage message);
    }
}
