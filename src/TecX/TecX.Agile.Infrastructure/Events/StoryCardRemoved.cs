using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardRemoved : IDomainEvent
    {
        private readonly Guid _storyCardId;
        private readonly Guid _from;

        public Guid From
        {
            get { return _from; }
        }

        public Guid StoryCardId
        {
            get { return _storyCardId; }
        }

        public StoryCardRemoved(Guid storyCardId, Guid from)
        {
            _storyCardId = storyCardId;
            _from = from;
        }
    }
}