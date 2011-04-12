using System;

namespace TecX.Agile.Infrastructure.Events
{
    public class StoryCardMoved : IDomainEvent
    {
        #region Fields

        private readonly Guid _storyCardId;
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

        public Guid StoryCardId
        {
            get { return _storyCardId; }
        }

        #endregion Properties

        #region c'tor

        public StoryCardMoved(Guid storyCardId, double x, double y, double angle)
        {
            _storyCardId = storyCardId;
            _x = x;
            _y = y;
            _angle = angle;
        }

        #endregion c'tor
    }
}
