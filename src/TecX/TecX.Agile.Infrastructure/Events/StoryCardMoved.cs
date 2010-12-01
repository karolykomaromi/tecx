using System;

using TecX.Agile.ViewModel;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardMoved
    {
        #region Fields

        private readonly StoryCard _storyCard;
        private readonly double _x;
        private readonly double _y;
        private readonly double _angle;

        #endregion Fields

        #region Properties

        public double Angle
        {
            get { return _angle; }
        }

        public double Y
        {
            get { return _y; }
        }

        public double X
        {
            get { return _x; }
        }

        public StoryCard StoryCard
        {
            get { return _storyCard; }
        }

        #endregion Properties

        #region c'tor

        public StoryCardMoved(StoryCard storyCard, double x, double y, double angle)
        {
            _storyCard = storyCard;
            _x = x;
            _y = y;
            _angle = angle;
        }

        #endregion c'tor
    }
}
