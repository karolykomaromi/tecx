namespace TecX.Agile.Messaging.Context
{
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;
    using TecX.Common.Comparison;

    public class AddStoryCardContext : InboundCommandContext<AddStoryCard>
    {
        public AddStoryCardContext(AddStoryCard command)
            : base(command)
        {
        }

        public override bool MatchesEvent(object outboundEvent)
        {
            Guard.AssertNotNull(outboundEvent, "outboundEvent");

            var @event = outboundEvent as StoryCardAdded;

            if(@event != null)
            {
                bool isMatch = this.Command.Id == @event.Id && 
                               EpsilonComparer.AreEqual(this.Command.Angle, @event.Angle) && 
                               EpsilonComparer.AreEqual(this.Command.X, @event.X) && 
                               EpsilonComparer.AreEqual(this.Command.Y, @event.Y);

                return isMatch;
            }

            return false;
        }
    }
}