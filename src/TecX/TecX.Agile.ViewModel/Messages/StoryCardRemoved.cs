using TecX.Common;

namespace TecX.Agile.ViewModel.Messages
{
    public class StoryCardRemoved : IMessage
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