using System;
using System.Windows.Media;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Common;

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
                if (_background == value)
                    return;

                OnPropertyChanging(() => Background);
                _background = value;
                OnPropertyChanged(() => Background);
            }
        }

        public string TaskOwner
        {
            get { return _taskOwner; }
            set
            {
                if (_taskOwner == value)
                    return;

                OnPropertyChanging(() => TaskOwner);
                _taskOwner = value;
                OnPropertyChanged(() => TaskOwner);
            }
        }

        public double MostLikelyEstimate
        {
            get { return _mostLikelyEstimate; }
            set
            {
                if (_mostLikelyEstimate == value)
                    return;

                OnPropertyChanging(() => MostLikelyEstimate);
                _mostLikelyEstimate = value;
                OnPropertyChanged(() => MostLikelyEstimate);
            }
        }

        public double ActualEffort
        {
            get { return _actualEffort; }
            set
            {
                if (_actualEffort == value)
                    return;

                OnPropertyChanging(() => ActualEffort);
                _actualEffort = value;
                OnPropertyChanged(() => ActualEffort);
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (_x == value)
                    return;

                OnPropertyChanging(() => X);
                _x = value;
                OnPropertyChanged(() => X);
            }
        }
        
        public double Y
        {
            get { return _y; }
            set
            {
                if (_y == value)
                    return;

                OnPropertyChanging(() => Y);
                _y = value;
                OnPropertyChanged(() => Y);
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                if (_angle == value)
                    return;

                OnPropertyChanging(() => Angle);
                _angle = value;
                OnPropertyChanged(() => Angle);
            }
        }

        #endregion Properties

        #region c'tor

        public StoryCard()
        {
            _background = Constants.Colors.Yellow;
            _taskOwner = string.Empty;
            _mostLikelyEstimate = 0.0;
            _actualEffort = 0.0;

            _moveStoryCardCommand = new DelegateCommand<StoryCardMoved>(OnStoryCardMoved);

            Commands.MoveStoryCard.RegisterCommand(_moveStoryCardCommand);
        }

        private void OnStoryCardMoved(StoryCardMoved args)
        {
            Guard.AssertNotNull(args, "args");

            if (args.StoryCardId != Id)
                return;

            X = args.X;
            Y = args.Y;
            Angle = args.Angle;
        }

        #endregion c'tor
    }
}