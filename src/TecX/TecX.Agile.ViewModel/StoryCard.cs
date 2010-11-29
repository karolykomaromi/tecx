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
        private decimal _mostLikelyEstimate;
        private decimal _actualEffort;

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

        public decimal MostLikelyEstimate
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

        public decimal ActualEffort
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

        #endregion Properties

        #region c'tor

        public StoryCard()
        {
            _background = Constants.Colors.Yellow;
            _taskOwner = string.Empty;
            _mostLikelyEstimate = 0.0m;
            _actualEffort = 0.0m;
        }

        #endregion c'tor
    }
}