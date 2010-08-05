using System;
using System.Linq;

using TecX.Common;
using TecX.Common.Comparison;

namespace TecX.Agile
{
    /// <summary>
    /// Story-card data-object
    /// </summary>
    [Serializable]
    public class StoryCard : PlanningArtefact, IEquatable<StoryCard>
    {
        #region Constants

        public static class Constants
        {
            /// <summary>StoryCard</summary>
            public const string StoryCardElementName = "StoryCard";

            public static class PropertyNames
            {
                /// <summary>View</summary>
                public const string ViewPropertyName = "View";

                /// <summary>Tracking</summary>
                public const string TrackingPropertyName = "Tracking";

                /// <summary>TaskOwner</summary>
                public const string TaskOwnerPropertyName = "TaskOwner";

                /// <summary>CurrentSideUp</summary>
                public const string CurrentSideUpPropertyName = "CurrentSideUp";

                /// <summary>DescriptionHandwritingImage</summary>
                public const string DescriptionHandwritingImagePropertyName = "DescriptionHandwritingImage";
            }
        }

        #endregion Constants

        ////////////////////////////////////////////////////////////

        #region Fields

        private Trackable _tracking;
        private Visualizable _view;
        private byte[] _descriptionHandwritingImage;

        #endregion Fields

        ////////////////////////////////////////////////////////////

        #region Properties

        /// <summary>
        /// Gets or sets information related to tracking progress and effort
        /// </summary>
        public Trackable Tracking
        {
            get { return _tracking; }
            set
            {
                Guard.AssertNotNull(value, "value");

                _tracking = value;
            }
        }

        /// <summary>
        /// Gets or sets information related to the visual representation
        /// </summary>
        public Visualizable View
        {
            get { return _view; }
            set
            {
                Guard.AssertNotNull(value, "value");

                _view = value;
            }
        }

        /// <summary>
        /// Gets and sets the owner of the task denoted on the story-card
        /// </summary>
        public string TaskOwner { get; set; }

        /// <summary>
        /// Gets and sets the side of the story-card that is currently display
        /// </summary>
        public StoryCardSides CurrentSideUp { get; set; }

        /// <summary>
        /// Gets and sets the data for the handwriten description
        /// </summary>
        public byte[] DescriptionHandwritingImage
        {
            get { return _descriptionHandwritingImage; }
            set
            {
                if (value == null)
                    value = new byte[0];

                _descriptionHandwritingImage = value;
            }
        }

        #endregion Properties

        ////////////////////////////////////////////////////////////

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCard"/> class.
        /// </summary>
        public StoryCard()
        {
            CurrentSideUp = StoryCardSides.FrontSide;
            DescriptionHandwritingImage = new byte[0];
            TaskOwner = string.Empty;

            _tracking = new Trackable();
            _view = new Visualizable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryCard"/> class.
        /// Copy constructor
        /// </summary>
        /// <param name="original">The original story-card.</param>
        private StoryCard(StoryCard original)
            : this()
        {
            Guard.AssertNotNull(original, "original");

            CopyValuesFrom(original);
        }

        #endregion c'tor

        ////////////////////////////////////////////////////////////

        #region Public Methods

        /// <summary>
        /// Copies the values of all properties from another story-card.
        /// </summary>
        /// <param name="anotherStoryCard">The story-card with the new values</param>
        private void CopyValuesFrom(StoryCard anotherStoryCard)
        {
            Guard.AssertNotNull(anotherStoryCard, "anotherStoryCard");

            base.CopyValuesFrom(anotherStoryCard);

            CurrentSideUp = anotherStoryCard.CurrentSideUp;
            DescriptionHandwritingImage = (byte[])anotherStoryCard.DescriptionHandwritingImage.Clone();

            TaskOwner = anotherStoryCard.TaskOwner;

            Tracking.CopyValuesFrom(anotherStoryCard.Tracking);
            View.CopyValuesFrom(anotherStoryCard.View);
        }

        #endregion Public Methods

        ////////////////////////////////////////////////////////////

        #region Object Members

        public bool Equals(StoryCard other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = base.Equals(other);
            equal &= CurrentSideUp == other.CurrentSideUp;
            equal &= DescriptionHandwritingImage.SequenceEqual(other.DescriptionHandwritingImage);
            equal &= Compare.AreEqual(other.TaskOwner, TaskOwner);
            equal &= Tracking.Equals(other.Tracking);
            equal &= View.Equals(other.View);

            return equal;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            var other = obj as StoryCard;

            if (other != null)
                return Equals(other);

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            //method solely exists to get rid of
            // 'overrides Equals() but does not override GetHashCode()' warning 
            return base.GetHashCode();
        }

        #endregion Object Members

        ////////////////////////////////////////////////////////////

        #region Overrides of PlanningArtefact

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = new StoryCard(this);

            return clone;
        }

        #endregion Overrides of PlanningArtefact
    }
}