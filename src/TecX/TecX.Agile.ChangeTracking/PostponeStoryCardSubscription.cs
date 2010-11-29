using System;
using System.Linq;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.ViewModel;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ChangeTracking
{
    public class PostponeStoryCardSubscription : IChangeSubscription
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Iteration _iteration;
        private IDisposable _subscription;

        public PostponeStoryCardSubscription(IEventAggregator eventAggregator, Iteration iteration)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");
            Guard.AssertNotNull(iteration, "iteration");

            _eventAggregator = eventAggregator;
            _iteration = iteration;

            var postponed =
                from evt in Observable.FromEvent<StoryCardPostponedEventArgs>(iteration, "StoryCardPostponed")
                select new {evt.EventArgs.From, evt.EventArgs.StoryCard};

            _subscription =
                postponed.Subscribe(x => _eventAggregator.Publish(new StoryCardPostponed(x.StoryCard, x.From)));
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _subscription.Dispose();
                _subscription = null;
            }
        }

        #endregion
    }
}