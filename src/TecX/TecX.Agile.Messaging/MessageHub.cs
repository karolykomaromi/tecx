namespace TecX.Agile.Messaging
{
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;
    using TecX.Event;

    public class MessageHub :
        ISubscribeTo<PropertyChanged>,
        IMessageSubscriber<ChangeProperty>,
        IMessageSubscriber<HighlightField>,
        ISubscribeTo<FieldHighlighted>
    {
        private readonly IMessageChannel channel;

        private readonly IEventAggregator eventAggregator;

        public MessageHub(IMessageChannel channel, IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(channel, "channel");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.channel = channel;
            this.eventAggregator = eventAggregator;
        }

        #region Outgoing

        public void Handle(PropertyChanged @event)
        {
            Guard.AssertNotNull(@event, "event");

            if (!InboundCommandContext.Current.MatchesEvent(@event))
            {
                this.channel.Send(@event);
            }
        }

        public void Handle(FieldHighlighted @event)
        {
            Guard.AssertNotNull(@event, "event");

            if (!InboundCommandContext.Current.MatchesEvent(@event))
            {
                this.channel.Send(@event);
            }
        }

        #endregion Outgoing

        #region Incoming

        public void Handle(ChangeProperty command)
        {
            Guard.AssertNotNull(command, "command");

            using (new ChangePropertyContext(command))
            {
                this.eventAggregator.Publish(command);
            }
        }

        public void Handle(HighlightField command)
        {
            Guard.AssertNotNull(command, "command");

            using (new HighlightFieldContext(command))
            {
                this.eventAggregator.Publish(command);
            }
        }

        #endregion Incoming
    }
}
