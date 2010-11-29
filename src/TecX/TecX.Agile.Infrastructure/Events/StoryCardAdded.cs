using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardAdded : IDomainEvent
    {
        private readonly StoryCard _storyCard;
        private readonly StoryCardCollection _to;

        public StoryCardCollection To
        {
            get { return _to; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        public StoryCardAdded(StoryCard storyCard, StoryCardCollection to)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(to, "to");

            _storyCard = storyCard;
            _to = to;
        }
    }
}
