namespace TecX.Agile.Messaging.Context
{
    public class NullInboundCommandContext : IInboundCommandContext
    {
        public object Command
        {
            get
            {
                return new object();
            }
        }

        public void Dispose()
        {
        }

        public bool MatchesEvent(object outboundEvent)
        {
            return false;
        }
    }
}
