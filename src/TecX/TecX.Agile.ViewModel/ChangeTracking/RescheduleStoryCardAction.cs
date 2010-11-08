using TecX.Undo;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class RescheduleStoryCardAction : AbstractAction
    {
        #region Fields

        private readonly StoryCard _storyCard;
        private readonly StoryCardCollection _to;
        private readonly StoryCardCollection _from;

        #endregion Fields

        #region Properties

        public StoryCardCollection From
        {
            get { return _from; }
        }

        public StoryCardCollection To
        {
            get { return _to; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        #endregion Properties

        public RescheduleStoryCardAction(StoryCard storyCard, StoryCardCollection from, StoryCardCollection to)
        {
            _storyCard = storyCard;
            _to = to;
            _from = from;
        }

        protected override void UnExecuteCore()
        {
            if (To.Contains(StoryCard) &&
                !From.Contains(StoryCard))
            {
                To.Reschedule(StoryCard, From);
            }
        }

        protected override void ExecuteCore()
        {
            if (!To.Contains(StoryCard) &&
                From.Contains(StoryCard))
            {
                From.Reschedule(StoryCard, To);
            }
        }
    }
}