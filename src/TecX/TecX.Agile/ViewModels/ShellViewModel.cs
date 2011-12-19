namespace TecX.Agile.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Media;

    using Caliburn.Micro;

    using TecX.Agile.Infrastructure;
    using TecX.Agile.Infrastructure.Commands;
    using TecX.Agile.Infrastructure.Events;
    using TecX.Common;
    using TecX.Event;

    using IEventAggregator = TecX.Event.IEventAggregator;

    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell, ISubscribeTo<AddStoryCard>
    {
        private readonly IEventAggregator eventAggregator;

        private readonly Conductor<IScreen>.Collection.AllActive overlays;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            this.eventAggregator = eventAggregator;
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

        public void AddOverlay(Screen overlay)
        {
            Guard.AssertNotNull(overlay, "overlay");

            this.Overlays.Add(overlay);
        }

        public void AddStoryCard(Guid id, double x, double y, double angle)
        {
            StoryCardViewModel storyCard = new StoryCardViewModel(this.EventAggregator) { Id = id, X = x, Y = y, Angle = angle };

            Items.Add(storyCard);

            this.EventAggregator.Publish(new AddedStoryCard { Id = storyCard.Id, X = x, Y = y, Angle = angle });

            ActivateItem(storyCard);
        }

        public Point PointFromScreen(Point point)
        {
            Point p = ((Visual)this.GetView()).PointFromScreen(point);

            return p;
        }

        public Point PointToScreen(Point point)
        {
            Point p = ((Visual)this.GetView()).PointToScreen(point);

            return p;
        }

        public void Handle(AddStoryCard message)
        {
            Guard.AssertNotNull(message, "message");

            this.AddStoryCard(message.Id, message.X, message.Y, message.Angle);
        }
    }
}
