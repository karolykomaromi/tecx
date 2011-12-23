namespace TecX.Agile.ViewModels
{
    using System;
    using System.Timers;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Messaging;
    using TecX.Common;
    using TecX.Common.Extensions.Primitives;
    using TecX.Event;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell,
        ISubscribeTo<AddStoryCard>,
        ISubscribeTo<DisplayInfoText>
    {
        private readonly IEventAggregator eventAggregator;

        private readonly MessageHub messageHub;

        private readonly Timer infoTextTimer;

        private readonly IObservableCollection<IOverlay> overlays;

        private string infoText;

        public ShellViewModel(IEventAggregator eventAggregator, MessageHub messageHub)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");
            Guard.AssertNotNull(messageHub, "messageHub");

            this.eventAggregator = eventAggregator;
            this.messageHub = messageHub;

            this.infoTextTimer = new Timer
                {
                    Interval = 10.Seconds().TotalMilliseconds
                };

            this.infoTextTimer.Elapsed += this.OnElapsed;

            this.overlays = new BindableCollection<IOverlay>();
        }

        public string InfoText
        {
            get
            {
                return this.infoText;
            }

            set
            {
                if (Equals(this.infoText, value))
                {
                    return;
                }

                this.infoText = value;
                NotifyOfPropertyChange(() => this.InfoText);
            }
        }

        public IEventAggregator EventAggregator
        {
            get
            {
                return this.eventAggregator;
            }
        }

        public IObservableCollection<IOverlay> Overlays
        {
            get
            {
                return this.overlays;
            }
        }

        public bool CanAddItem
        {
            get
            {
                return true;
            }
        }

        public void AddOverlay(IOverlay overlay)
        {
            Guard.AssertNotNull(overlay, "overlay");

            this.Overlays.Add(overlay);
        }

        public void AddStoryCard(Guid id, double x, double y, double angle)
        {
            StoryCardViewModel storyCard = StoryCardViewModel.Create(this.EventAggregator, id, x, y, angle);

            Items.Add(storyCard);

            this.EventAggregator.Publish(new StoryCardAdded(storyCard.Id, x, y, angle));

            ActivateItem(storyCard);
        }

        public void Handle(AddStoryCard message)
        {
            Guard.AssertNotNull(message, "message");

            this.AddStoryCard(message.Id, message.X, message.Y, message.Angle);
        }

        public void Handle(DisplayInfoText message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.Text, "message.Text");

            this.InfoText = message.Text;
            this.infoTextTimer.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            this.infoTextTimer.Stop();
            this.InfoText = string.Empty;
        }
    }
}
