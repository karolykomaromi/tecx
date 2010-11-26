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

        #endregion Properties

        #region c'tor

        public StoryCard()
        {
            _background = Constants.Colors.Yellow;
        }

        #endregion c'tor
    }
}