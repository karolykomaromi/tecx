using System;
using System.Windows.Media;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Events.Builder;
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

                Color pre = _background;
                Color post = value;

                string propertyName = GetPropertyName(() => Background);

                OnPropertyChanging(propertyName);

                _background = value;

                OnPropertyChanged(propertyName);

                EventAggregator.Publish(new PropertyUpdated(Id, propertyName, pre, post));
            }
        }

        public string TaskOwner
        {
            get { return _taskOwner; }
            set
            {
                if (_taskOwner == value)
                    return;

                string pre = _taskOwner;
                string post = value;

                OnPropertyChanging(() => TaskOwner);

                _taskOwner = value;

                OnPropertyChanged(() => TaskOwner);

                EventAggregator.Publish(new PropertyUpdated(Id, "TaskOwner", pre, post));
            }
        }

        public double MostLikelyEstimate
        {
            get { return _mostLikelyEstimate; }
            set
            {
                if (_mostLikelyEstimate == value)
                    return;

                double pre = _mostLikelyEstimate;
                double post = value;

                OnPropertyChanging(() => MostLikelyEstimate);

                _mostLikelyEstimate = value;

                OnPropertyChanged(() => MostLikelyEstimate);

                EventAggregator.Publish(new PropertyUpdated(Id, "MostLikelyEstimate", pre, post));
            }
        }

        public double ActualEffort
        {
            get { return _actualEffort; }
            set
            {
                if (_actualEffort == value)
                    return;

                double pre = _actualEffort;
                double post = value;

                OnPropertyChanging(() => ActualEffort);
                _actualEffort = value;
                OnPropertyChanged(() => ActualEffort);

                EventAggregator.Publish(new PropertyUpdated(Id, "ActualEffort", pre, post));
            }
        }

        public double X
        {
            get { return _x; }
            set
            {
                if (_x == value)
                    return;

                double pre = _x;
                double post = value;

                OnPropertyChanging(Infrastructure.Constants.PropertyNames.X);

                _x = value;

                OnPropertyChanged(Infrastructure.Constants.PropertyNames.X);

                StoryCardMoved message = new StoryCardMovedMessageBuilder()
                                                .MoveStoryCard(Id)
                                                .From(pre, Y, Angle)
                                                .To(post, Y, Angle);

                EventAggregator.Publish(message);
            }
        }
        
        public double Y
        {
            get { return _y; }
            set
            {
                if (_y == value)
                    return;

                double pre = _y;
                double post = value;

                OnPropertyChanging(Infrastructure.Constants.PropertyNames.Y);

                _y = value;

                OnPropertyChanged(Infrastructure.Constants.PropertyNames.Y);

                StoryCardMoved message = new StoryCardMovedMessageBuilder()
                                                .MoveStoryCard(Id)
                                                .From(X, pre, Angle)
                                                .To(X, post, Angle);

                EventAggregator.Publish(message);
            }
        }

        public double Angle
        {
            get { return _angle; }
            set
            {
                if (_angle == value)
                    return;

                double pre = _angle;
                double post = value;

                OnPropertyChanging(Infrastructure.Constants.PropertyNames.Angle);

                _angle = value;

                OnPropertyChanged(Infrastructure.Constants.PropertyNames.Angle);

                StoryCardMoved message = new StoryCardMovedMessageBuilder()
                                                .MoveStoryCard(Id)
                                                .From(X, Y, pre)
                                                .To(X, Y, post);

                EventAggregator.Publish(message);
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