using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardRemoved : IDomainEvent
    {
        private readonly StoryCard _storyCard;
        private readonly StoryCardCollection _from;

        public StoryCardCollection From
        {
            get { return _from; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        public StoryCardRemoved(StoryCard storyCard, StoryCardCollection from)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(from, "from");

            _storyCard = storyCard;
            _from = from;
        }
    }
}