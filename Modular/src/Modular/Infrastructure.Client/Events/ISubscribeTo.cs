namespace Infrastructure.Events
{
    public interface ISubscribeTo<in TMessage>
    {
        void Handle(TMessage message);
    }
}