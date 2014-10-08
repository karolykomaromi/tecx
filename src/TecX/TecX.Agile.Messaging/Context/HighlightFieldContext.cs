namespace TecX.Agile.Messaging.Context
{
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;

    public class HighlightFieldContext : InboundCommandContext<HighlightField>
    {
        public HighlightFieldContext(HighlightField command)
            : base(command)
        {
        }

        public override bool MatchesEvent(object outboundEvent)
        {
            Guard.AssertNotNull(outboundEvent, "outboundEvent");

            var @event = outboundEvent as FieldHighlighted;

            if (@event != null)
            {
                bool isMatch = this.Command.Id == @event.Id && Equals(this.Command.FieldName, @event.FieldName);

                return isMatch;
            }

            return false;
        }
    }
}