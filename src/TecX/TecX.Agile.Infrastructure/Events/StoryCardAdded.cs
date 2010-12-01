using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardAdded : IDomainEvent
    {
        private readonly Guid _storyCardId;
        private readonly Guid _to;

        public Guid To
        {
            get { return _to; }
        }

        public Guid StoryCardId
        {
            get { return _storyCardId; }
        }

        public StoryCardAdded(Guid storyCardId, Guid to)
        {

            _storyCardId = storyCardId;
            _to = to;
        }
    }
}
