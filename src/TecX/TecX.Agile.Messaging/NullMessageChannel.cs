namespace TecX.Agile.Messaging
{
    using System.Windows.Threading;

    public class NullMessageChannel : MessageChannel
    {
        public NullMessageChannel(Dispatcher dispatcher)
            : base(dispatcher)
        {
        }

        public override void Send<TMessage>(TMessage message)
        {
            /* used for local planning sessions. no messages are sent to remote recipient */
        }
    }
}
