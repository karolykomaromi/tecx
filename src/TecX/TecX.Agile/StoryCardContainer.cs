using System;
using System.Collections;
using System.Collections.Generic;

using TecX.Common;
using TecX.Common.Extensions.Error;

namespace TecX.Agile
{
    /// <summary>
    /// Base for planning artefacts that can contain story-cards (e.g. <see cref="Backlog"/> and
    /// <see cref="Iteration"/>)
    /// </summary>
    [Serializable]
    public abstract class StoryCardContainer : PlanningArtefact, IEnumerable<StoryCard>
    {
        #region Constants

        private static class Constants
        {
            public static class PropertyNames
            {
                /// <summary>StoryCards</summary>
                public const string StoryCardsPropertyName = "StoryCards";
            }
        }

        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region Fields

        /// <summary>
        /// Backing field for <see cref="StoryCards"/>
        /// </summary>
        private readonly Dictionary<Guid, StoryCard> _storyCards;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        /// <summary>
        /// Gets and sets the list of <see cref="StoryCard"/>s in the <see cref="Backlog"/>
        /// </summary>
        public IEnumerable<StoryCard> StoryCards
        {
            get { return _storyCards.Values; }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCardContainer"/> class.
        /// </summary>
        protected StoryCardContainer()
        {
            _storyCards = new Dictionary<Guid, StoryCard>();
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Methods

        public void Clear()
        {
            _storyCards.Clear();
        }

        public void Add(StoryCard storyCard)
        {
            Guard.AssertNotNull(storyCard, "storyCard");

            if (_storyCards.ContainsKey(storyCard.Id))
                throw new ArgumentException("A StoryCard with the same Id already exists.", "storyCard")
                    .WithAdditionalInfo("existing", _storyCards[storyCard.Id]);

            _storyCards.Add(storyCard.Id, storyCard);
        }

        public bool Contains(StoryCard storyCard)
        {
            Guard.AssertNotNull(storyCard, "storyCard");

            StoryCard existing;
            if (_storyCards.TryGetValue(storyCard.Id, out existing))
            {
                if (storyCard.Equals(existing))
                {
                    return true;
                }
                else
                {
                    //TODO weberse is it an error if the cards share the same id but have different values?
                }
            }

            return false;
        }

        public bool Remove(Guid id)
        {
            return _storyCards.Remove(id);
        }

        protected internal void CopyValuesFrom(StoryCardContainer other)
        {
            Guard.AssertNotNull(other, "other");

            base.CopyValuesFrom(other);

            _storyCards.Clear();

            foreach (StoryCard storyCard in other)
            {
                Add((StoryCard)storyCard.Clone());
            }
        }

        #endregion Methods

        ////////////////////////////////////////////////////////////

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<StoryCard> GetEnumerator()
        {
            return _storyCards.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}