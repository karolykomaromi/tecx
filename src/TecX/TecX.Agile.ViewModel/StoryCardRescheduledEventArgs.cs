using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    public class StoryCardRescheduledEventArgs : EventArgs
    {
        #region Fields

        private readonly StoryCard _storyCard;
        private readonly StoryCardCollection _from;
        private readonly StoryCardCollection _to;

        #endregion Fields

        #region Properties

        public StoryCardCollection To
        {
            get { return _to; }
        }

        public StoryCardCollection From
        {
            get { return _from; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        #endregion Properties

        #region c'tor

        public StoryCardRescheduledEventArgs(StoryCard storyCard, StoryCardCollection from, StoryCardCollection to)
        {
            Guard.AssertNotNull(storyCard, "storyCard");
            Guard.AssertNotNull(from, "from");
            Guard.AssertNotNull(to, "to");

            _storyCard = storyCard;
            _from = from;
            _to = to;
        }

        #endregion c'tor
    }
}