using System;

using TecX.Common;

namespace TecX.Agile.Infrastructure.Events.Builder
{
    public class StoryCardMovedMessageBuilder
    {
        private Guid _storyCardId;

        private double _fromX;
        private double _fromY;
        private double _fromAngle;

        private double _toX;
        private double _toY;
        private double _toAngle;

        public StoryCardMovedMessageBuilder MoveStoryCard(Guid id)
        {
            _storyCardId = id;

            return this;
        }

        public StoryCardMovedMessageBuilder From(double x, double y, double angle)
        {
            _fromX = x;
            _fromY = y;
            _fromAngle = angle;

            return this;
        }

        public StoryCardMovedMessageBuilder To(double x, double y, double angle)
        {
            _toX = x;
            _toY = y;
            _toAngle = angle;

            return this;
        }

        public StoryCardMoved Build()
        {
            PositionAndOrientation from = new PositionAndOrientation(_fromX, _fromY, _fromAngle);
            PositionAndOrientation to = new PositionAndOrientation(_toX, _toY, _toAngle);

            StoryCardMoved message = new StoryCardMoved(_storyCardId, from, to);

            return message;
        }

        public static implicit operator StoryCardMoved(StoryCardMovedMessageBuilder builder)
        {
            Guard.AssertNotNull(builder, "builder");

            return builder.Build();
        }
    }
}
