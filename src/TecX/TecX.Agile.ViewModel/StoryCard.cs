using System;
using System.Windows.Media;

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
        }

        #endregion c'tor
    }
}