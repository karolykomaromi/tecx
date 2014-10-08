namespace TecX.Agile.Messaging
{
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;

    public static class EventExtensions
    {
        public static MoveCaret ToCommand(this CaretMoved @event)
        {
            Guard.AssertNotNull(@event, "event");

            var command = new MoveCaret(@event.Id, @event.FieldName, @event.CaretIndex);

            return command;
        }

        public static HighlightField ToCommand(this FieldHighlighted @event)
        {
            Guard.AssertNotNull(@event, "event");

            var command = new HighlightField(@event.Id, @event.FieldName);

            return command;
        }

        public static ChangeProperty ToCommand(this PropertyChanged @event)
        {
            Guard.AssertNotNull(@event, "event");

            var command = new ChangeProperty(@event.Id, @event.PropertyName, @event.From, @event.To);

            return command;
        }        
        
        public static AddStoryCard ToCommand(this StoryCardAdded @event)
        {
            Guard.AssertNotNull(@event, "event");

            var command = new AddStoryCard(@event.Id, @event.X, @event.Y, @event.Angle);

            return command;
        }
    }
}
