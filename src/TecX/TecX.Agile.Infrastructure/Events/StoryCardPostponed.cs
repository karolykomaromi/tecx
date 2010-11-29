using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardPostponed : IDomainEvent
    {
        private readonly StoryCard _storyCard;
        private readonly Iteration _from;

        public Iteration From
        {
            get { return _from; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        public StoryCardPostponed(StoryCard storyCard, Iteration from)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(from, "from");

            _storyCard = storyCard;
            _from = from;
        }

    }
}