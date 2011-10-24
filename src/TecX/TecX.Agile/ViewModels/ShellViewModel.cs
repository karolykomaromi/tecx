using System;
using System.Windows;
using System.Windows.Media;

using Caliburn.Micro;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Common;

using IEventAggregator = TecX.Common.Event.IEventAggregator;

namespace TecX.Agile.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.AllActive, IShell
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly Conductor<IScreen>.Collection.AllActive _overlays;

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
            _overlays = new Conductor<IScreen>.Collection.AllActive();
        }

        public IEventAggregator EventAggregator
        {
            get
            {
                return _eventAggregator;
            }
        }

        public IObservableCollection<IScreen> Overlays
        {
            get
            {
                return _overlays.Items;
            }
        }

        private int counter = 0;

        public void AddItem()
        {
            if (counter % 2 == 0)
            {
                AddStoryCard(0, 0, 0);
            }
            else
            {
                Items.Add(new IterationViewModel());
            }

            counter++;
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

            Overlays.Add(overlay);
        }

        public void AddStoryCard(double x, double y, double angle)
        {
            StoryCardViewModel storyCard = new StoryCardViewModel { Id = Guid.NewGuid(), X = x, Y = y, Angle = angle };

            Items.Add(storyCard);

            EventAggregator.Publish(new AddedStoryCard { Id = storyCard.Id, X = x, Y = y, Angle = angle });

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
    }
}
