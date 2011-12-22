namespace TecX.Agile.ViewModels
{
    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Common.Comparison;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public class StoryCardViewModel : CardViewModel
    {
        #region Fields

        private readonly IObservableCollection<object> decorators;

        private string title;

        private double mostLikelyEstimate;

        private double actualEffort;

        private string taskOwner;

        private bool isHandWritingEnabled;

        private string description;

        #endregion Fields

        public StoryCardViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            this.Width = Defaults.StoryCard.Width;
            this.Height = Defaults.StoryCard.Height;
            this.Opacity = Defaults.Opacity;

            this.Background = Constants.Colors.Yellow;

            this.decorators = new BindableCollection<object>();
        }

        #region Properties

        public IObservableCollection<object> Decorators
        {
            get
            {
                return this.decorators;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title == value)
                {
                    return;
                }

                this.title = value;
                this.NotifyOfPropertyChange(() => this.Title);
            }
        }

        public double MostLikelyEstimate
        {
            get
            {
                return this.mostLikelyEstimate;
            }

            set
            {
                if (EpsilonComparer.AreEqual(this.mostLikelyEstimate, value))
                {
                    return;
                }

                this.mostLikelyEstimate = value;
                this.NotifyOfPropertyChange(() => this.MostLikelyEstimate);
            }
        }

        public double ActualEffort
        {
            get
            {
                return this.actualEffort;
            }

            set
            {
                if (EpsilonComparer.AreEqual(this.actualEffort, value))
                {
                    return;
                }

                this.actualEffort = value;
                this.NotifyOfPropertyChange(() => this.ActualEffort);
            }
        }

        public string TaskOwner
        {
            get
            {
                return this.taskOwner;
            }

            set
            {
                if (this.taskOwner == value)
                {
                    return;
                }

                this.taskOwner = value;
                this.NotifyOfPropertyChange(() => this.TaskOwner);
            }
        }

        public bool IsHandWritingEnabled
        {
            get
            {
                return this.isHandWritingEnabled;
            }

            set
            {
                if (this.isHandWritingEnabled == value)
                {
                    return;
                }

                this.isHandWritingEnabled = value;
                this.NotifyOfPropertyChange(() => this.IsHandWritingEnabled);
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }

            set
            {
                if (this.description == value)
                {
                    return;
                }

                this.description = value;
                this.NotifyOfPropertyChange(() => this.Description);
            }
        }

        #endregion Properties
    }
}
