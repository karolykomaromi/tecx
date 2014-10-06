namespace TecX.Agile.Messaging.Context
{
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;

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

            if (@event != null)
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