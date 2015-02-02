namespace Infrastructure.Client.Test.TestObjects
{
    using Infrastructure.Events;

    public class Subscriber : ISubscribeTo<Message>
    {
        public void Handle(Message message)
        {
        }
    }
}