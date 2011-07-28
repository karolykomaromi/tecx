using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TecX.Agile.Infrastructure;
using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile.ViewModel
{
    public abstract class PlanningArtefactCollection<TArtefact> : PlanningArtefact, ICollection<TArtefact>, INotifyCollectionChanged
        where TArtefact : PlanningArtefact
    {
        #region Fields

        private readonly Dictionary<Guid, TArtefact> _artefacts;

        #endregion Fields

        #region Properties

        public Project Project { get; internal set; }

        internal IDictionary<Guid, TArtefact> Artefacts
        {
            get { return _artefacts; }
        }

        #endregion Properties

        #region Indexer

        public TArtefact this[Guid id]
        {
            get
            {
                TArtefact existing;
                if (_artefacts.TryGetValue(id, out existing))
                {
                    return existing;
                }
                return null;
            }
            set
            {
                TArtefact existing;
                if (_artefacts.TryGetValue(id, out existing))
                {
                    _artefacts[id] = value;
#if SILVERLIGHT
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, existing, -1);
#else
                    var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, existing);
#endif
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
            : base(new NullEventAggregator())
        {
            _artefacts = new Dictionary<Guid, TArtefact>();
        }

        #endregion c'tor

        #region Methods

        public void Add(TArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            TArtefact existing;
            if (_artefacts.TryGetValue(item.Id, out existing))
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
#if SILVERLIGHT
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, -1);
#else
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item);
#endif
            OnCollectionChanged(args);
        }

        public bool Remove(Guid id)
        {
            TArtefact existing;
            if (_artefacts.TryGetValue(id, out existing))
            {
                RemoveCore(existing);
                bool removed = _artefacts.Remove(id);

#if SILVERLIGHT
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, existing, -1);
#else
                var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, existing);
#endif
                OnCollectionChanged(args);

                return removed;
            }

            return false;
        }

        public bool Contains(TArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            TArtefact existing;
            if (_artefacts.TryGetValue(item.Id, out existing))
            {
                return existing.Equals(item);
            }

            return false;
        }

        public int Count
        {
            get { return _artefacts.Count; }
        }

        protected internal abstract void AddCore(TArtefact item);
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

            if (CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }

        #endregion

        #region Explicit Implementation of ICollection<TArtefact>

        void ICollection<TArtefact>.CopyTo(TArtefact[] array, int arrayIndex)
        {
            Guard.AssertNotNull(arrayIndex, "arrayIndex");
            Guard.AssertIsInRange(arrayIndex, "arrayIndex", 0, Count - 1);

            ((ICollection<TArtefact>)_artefacts).CopyTo(array, arrayIndex);
        }

        bool ICollection<TArtefact>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<TArtefact>.Remove(TArtefact item)
        {
            Guard.AssertNotNull(item, "item");

            return Remove(item.Id);
        }

        void ICollection<TArtefact>.Clear()
        {
            throw new NotImplementedException();
        }

        #endregion Explicit Implementation of ICollection<TArtefact>
    }
}
