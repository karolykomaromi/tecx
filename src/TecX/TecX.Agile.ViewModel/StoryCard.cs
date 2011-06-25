using System;
using System.Windows.Media;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Events.Builder;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public class StoryCard : PlanningArtefact
    {
        #region Fields

        private Color _background;
        private string _taskOwner;
        private double _mostLikelyEstimate;
        private double _actualEffort;
        private double _x;
        private double _angle;
        private double _y;

        private readonly DelegateCommand<StoryCardMoved> _moveStoryCardCommand;

        #endregion Fields

        #region Properties

        public PlanningArtefactCollection<StoryCard> Parent { get; internal set; }

        public Color Background
        {
            get { return _background; }
            set
            {
                Set(() => _background, value);
            }
        }

        public string TaskOwner
        {
            get { return _taskOwner; }
            set
            {
                Set(() => _taskOwner, value);
            }
        }

        public double MostLikelyEstimate
        {
            get { return _mostLikelyEstimate; }
            set
            {
                Set(() => _mostLikelyEstimate, value);
            }
        }

        public double ActualEffort
        {
            get { return _actualEffort; }
            set
            {
                Set(() => _actualEffort, value);
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                Set(() => _x, value);
            }
        }
        
        public double Y
        {
            get { return _y; }
            set
            {
                Set(() => _y, value);
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {

                Set(() => _angle, value);
            }
        }

        #endregion Properties

        #region c'tor

        public StoryCard(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            _background = Constants.Colors.Yellow;
            _taskOwner = string.Empty;
            _mostLikelyEstimate = 0.0;
            _actualEffort = 0.0;

            _moveStoryCardCommand = new DelegateCommand<StoryCardMoved>(OnStoryCardMoved);

            Commands.MoveStoryCard.RegisterCommand(_moveStoryCardCommand);
        }

        private void OnStoryCardMoved(StoryCardMoved message)
        {
            Guard.AssertNotNull(message, "message");

            if (message.StoryCardId != Id)
                return;

            X = message.To.X;
            Y = message.To.Y;
            Angle = message.To.Angle;
        }

        #endregion c'tor
    }
}