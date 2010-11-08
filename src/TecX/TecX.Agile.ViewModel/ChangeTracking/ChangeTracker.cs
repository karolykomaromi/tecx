using System;
using System.Collections.Generic;

using TecX.Common;
using TecX.Common.Extensions.Error;
using TecX.Undo;

namespace TecX.Agile.ViewModel.ChangeTracking
{
    public class ChangeTracker : IChangeTracker
    {
        #region Fields

        private readonly IActionManager _actionManager;
        private readonly Dictionary<Guid, IChangeSubscription> _propertyChangeSubscriptions;
        private readonly Dictionary<Guid, IChangeSubscription> _collectionChangeSubscriptions;
        private Dictionary<Guid, IChangeSubscription> _rescheduleStoryCardSubscriptions;

        #endregion Fields

        #region c'tor

        public ChangeTracker(IActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actionManager = actionManager;
            _propertyChangeSubscriptions = new Dictionary<Guid, IChangeSubscription>();
            _collectionChangeSubscriptions = new Dictionary<Guid, IChangeSubscription>();
            _rescheduleStoryCardSubscriptions = new Dictionary<Guid, IChangeSubscription>();
        }

        #endregion c'tor

        #region Implementation of IChangeTracker

        public void Subscribe(PlanningArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            if (_propertyChangeSubscriptions.ContainsKey(item.Id))
            {
                throw new InvalidOperationException("Item already subscribed to change tracking")
                    .WithAdditionalInfo("item", item);
            }

            PropertyChangeSubscription subscription = new PropertyChangeSubscription(_actionManager, item);

            _propertyChangeSubscriptions.Add(item.Id, subscription);
        }

        public void Unsubscribe(PlanningArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            IChangeSubscription existing;
            if (_propertyChangeSubscriptions.TryGetValue(item.Id, out existing))
            {
                _propertyChangeSubscriptions.Remove(item.Id);
                existing.Dispose();
            }
        }

        public void Subscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection) where TArtefact : PlanningArtefact
        {
            Guard.AssertNotNull(collection, "collection");
            
            //will throw if item is already subscribed
            Subscribe((PlanningArtefact)collection);

            CollectionChangeSubscription<TArtefact> subscription = new CollectionChangeSubscription<TArtefact>(_actionManager, collection);

            _collectionChangeSubscriptions.Add(collection.Id, subscription);

        }

        public void Unsubscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection) where TArtefact : PlanningArtefact
        {
            Guard.AssertNotNull(collection, "collection");

            Unsubscribe((PlanningArtefact)collection);

            IChangeSubscription existing;
            if(_collectionChangeSubscriptions.TryGetValue(collection.Id, out existing))
            {
                _collectionChangeSubscriptions.Remove(collection.Id);
                existing.Dispose();
            }
        }

        public void Subscribe(StoryCardCollection collection)
        {
            Guard.AssertNotNull(collection, "collection");

            Subscribe(((PlanningArtefactCollection<StoryCard>)collection));

            RescheduleStoryCardSubscription subscription = new RescheduleStoryCardSubscription(_actionManager, collection);

            _rescheduleStoryCardSubscriptions.Add(collection.Id, subscription);
        }

        public void Unsubscribe(StoryCardCollection collection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}