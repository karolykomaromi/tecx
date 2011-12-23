namespace TecX.Agile.Messaging
{
    using System;

    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Messaging.Context;
    using TecX.Common;
    using TecX.Event;

    public class MessageHub :
        ISubscribeTo<PropertyChanged>,
        IMessageSubscriber<ChangeProperty>,
        IMessageSubscriber<HighlightField>,
        ISubscribeTo<FieldHighlighted>,
        IMessageSubscriber<AddStoryCard>,
        ISubscribeTo<StoryCardAdded>,
        IMessageSubscriber<MoveCaret>,
        ISubscribeTo<CaretMoved>
    {
        private readonly IMessageChannel channel;

        private readonly IEventAggregator eventAggregator;

        public MessageHub(IMessageChannel channel, IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(channel, "channel");
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.channel = channel;
            this.channel.Subscribe(this);

            this.eventAggregator = eventAggregator;
            this.eventAggregator.Subscribe(this);
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

        public void Handle(StoryCardAdded @event)
        {
            Guard.AssertNotNull(@event, "event");

            if (!InboundCommandContext.Current.MatchesEvent(@event))
            {
                this.channel.Send(@event);
            }
        }

        public void Handle(CaretMoved @event)
        {
            Guard.AssertNotNull(@event, "event");

            if(!InboundCommandContext.Current.MatchesEvent(@event))
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

        public void Handle(AddStoryCard command)
        {
            Guard.AssertNotNull(command, "command");

            using (new AddStoryCardContext(command))
            {
                this.eventAggregator.Publish(command);
            }
        }

        public void Handle(MoveCaret command)
        {
            Guard.AssertNotNull(command, "command");

            using (new MoveCaretContext(command))
            {
                this.eventAggregator.Publish(command);
            }
        }

        #endregion Incoming
    }

    public class MoveCaretContext : InboundCommandContext<MoveCaret>
    {
        public MoveCaretContext(MoveCaret command)
            : base(command)
        {
        }

        public override bool MatchesEvent(object outboundEvent)
        {
            Guard.AssertNotNull(outboundEvent, "outboundEvent");

            var @event = outboundEvent as CaretMoved;

            if(@event != null)
            {
                bool isMatch = this.Command.Id == @event.Id && 
                    Equals(this.Command.FieldName, @event.FieldName) && 
                    this.Command.CaretIndex == @event.CaretIndex;

                return isMatch;
            }

            return false;
        }
    }
}
