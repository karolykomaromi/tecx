namespace TecX.Agile.Infrastructure.Messaging
{
    using TecX.Common;

    public abstract class InboundCommandContext : IInboundCommandContext
    {
        protected readonly object command;

        protected InboundCommandContext(object message)
        {
            Guard.AssertNotNull(message, "message");

            this.command = message;

            Current = this;
        }

        public static IInboundCommandContext Current { get; set; }

        object IInboundCommandContext.Command
        {
            get
            {
                return this.command;
            }
        }

        public abstract bool MatchesEvent(object outboundEvent);

        public void Dispose()
        {
            if (Current == this)
            {
                Current = null;
            }
        }
    }

    public abstract class InboundCommandContext<TCommand> : InboundCommandContext, IInboundCommandContext<TCommand>
    {
        protected InboundCommandContext(TCommand command)
            : base(command)
        {
        }

        public TCommand Command
        {
            get
            {
                return (TCommand)this.command;
            }
        }
    }
}
