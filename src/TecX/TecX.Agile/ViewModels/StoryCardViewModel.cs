namespace TecX.Agile.ViewModels
{
    using System;

    using Caliburn.Micro;

    using TecX.Common;

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
            this.title = string.Empty;
            this.mostLikelyEstimate = 0.0;
            this.actualEffort = 0.0;
            this.taskOwner = string.Empty;
            this.isHandWritingEnabled = false;
            this.description = string.Empty;
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
                this.Set(() => this.title, value);
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
                this.Set(() => this.mostLikelyEstimate, value);
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
                this.Set(() => this.actualEffort, value);
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
                this.Set(() => this.taskOwner, value);
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
                this.Set(() => this.isHandWritingEnabled, value);
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
                this.Set(() => this.description, value);
            }
        }

        #endregion Properties

        public static StoryCardViewModel Create(IEventAggregator eventAggregator, Guid id, double x, double y, double angle)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            StoryCardViewModel storyCard = new StoryCardViewModel(eventAggregator);

            storyCard.Initialize(id, x, y, angle);

            return storyCard;
        }
    }
}
