using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.ViewModel
{
    public abstract class PlanningArtefactCollection<TArtefact> : PlanningArtefact, 
        IEnumerable<TArtefact>, INotifyCollectionChanged
        where TArtefact : PlanningArtefact
    {
        #region Fields

        private readonly Dictionary<Guid, TArtefact> _artefacts;

        #endregion Fields

        #region Properties

        public Project Project { get; internal set; }

        #endregion Properties

        #region Indexer

        public TArtefact this[Guid id]
        {
            get 
            { 
                TArtefact existing;
                if(_artefacts.TryGetValue(id, out existing))
                {
                    return existing;
                }
                return null;
            }
            set
            {
                TArtefact existing;
                if(_artefacts.TryGetValue(id, out existing))
                {
                    _artefacts[id] = value;

                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value,
                                                                    existing);

                    OnCollectionChanged(args);
                }
                else
                {
                    Add(value);
                }
            }
        }

        #endregion Indexer

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefactCollection{TArtefact}"/> class
        /// </summary>
        protected PlanningArtefactCollection()
        {
            _artefacts = new Dictionary<Guid, TArtefact>();
        }

        #endregion c'tor

        #region Methods

        public void Add(TArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            TArtefact existing;
            if(_artefacts.TryGetValue(item.Id, out existing))
            {
                throw new InvalidOperationException("Artefact with identical Id already in collection")
                    .WithAdditionalInfos(new Dictionary<object, object>
                                             {
                                                 {"existing", existing}, 
                                                 {"new", item}
                                             });
            }

            _artefacts.Add(item.Id, item);

            AddCore(item);

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
            OnCollectionChanged(args);
        }
        
        public bool Remove(Guid id)
        {
            TArtefact existing;
            if(_artefacts.TryGetValue(id, out existing))
            {
                RemoveCore(existing);
                bool removed = _artefacts.Remove(id);

                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, existing);
                OnCollectionChanged(args);

                return removed;
            }

            return false;
        }

        protected abstract void AddCore(TArtefact item);
        protected abstract void RemoveCore(TArtefact item);

        #endregion Methods

        #region Implementation of IEnumerable

        public IEnumerator<TArtefact> GetEnumerator()
        {
            return _artefacts.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            Guard.AssertNotNull(args, "args");

            if(CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }

        #endregion
    }
}
