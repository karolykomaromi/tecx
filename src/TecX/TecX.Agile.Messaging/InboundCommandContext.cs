namespace TecX.Agile.Messaging
{
    using TecX.Common;

    public abstract class InboundCommandContext : IInboundCommandContext
    {
        public static readonly IInboundCommandContext Empty = new NullInboundCommandContext();

        protected readonly object command;

        private static IInboundCommandContext current;

        static InboundCommandContext()
        {
            Current = InboundCommandContext.Empty;
        }

        protected InboundCommandContext(object message)
        {
            Guard.AssertNotNull(message, "message");

            this.command = message;

            Current = this;
        }

        public static IInboundCommandContext Current
        {
            get
            {
                return current;
            }

            set
            {
                Guard.AssertNotNull(value, "Current");

                current = value;
            }
        }

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
                Current = InboundCommandContext.Empty;
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
