using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class StoryCardPostponedEventArgs : EventArgs
    {
        #region Fields

        private readonly StoryCard _storyCard;
        private readonly Iteration _from;

        #endregion Fields

        #region Properties

        public Iteration From
        {
            get { return _from; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        #endregion Properties

        #region c'tor

        public StoryCardPostponedEventArgs(StoryCard storyCard, Iteration from)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(from, "from");

            _storyCard = storyCard;
            _from = from;
        }

        #endregion c'tor
    }
}