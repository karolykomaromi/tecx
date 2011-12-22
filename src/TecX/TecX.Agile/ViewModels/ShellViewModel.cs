namespace TecX.Agile.ViewModels
{
    using System;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Agile.Messaging;
    using TecX.Common;
    using TecX.Event;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell, ISubscribeTo<AddStoryCard>
    {
        private readonly IEventAggregator eventAggregator;

        private readonly MessageHub messageHub;

        private readonly Conductor<IScreen>.Collection.AllActive overlays;

        public ShellViewModel(IEventAggregator eventAggregator, MessageHub messageHub)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");
            Guard.AssertNotNull(messageHub, "messageHub");

            this.eventAggregator = eventAggregator;
            this.messageHub = messageHub;
            this.overlays = new Conductor<IScreen>.Collection.AllActive();
        }

        public IEventAggregator EventAggregator
        {
            get
            {
                return this.eventAggregator;
            }
        }

        public IObservableCollection<IScreen> Overlays
        {
            get
            {
                return this.overlays.Items;
            }
        }

        public bool CanAddItem
        {
            get
            {
                return true;
            }
        }

        public void AddOverlay(IScreen overlay)
        {
            Guard.AssertNotNull(overlay, "overlay");

            this.Overlays.Add(overlay);
        }

        public void AddStoryCard(Guid id, double x, double y, double angle)
        {
            //StoryCardViewModel storyCard = new StoryCardViewModel(this.EventAggregator) { Id = id, X = x, Y = y, Angle = angle };
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
    }
}
