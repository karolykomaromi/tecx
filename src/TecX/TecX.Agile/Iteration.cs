using System;

using TecX.Common;
using TecX.Common.Time;

namespace TecX.Agile
{
    /// <summary>
    /// Data-object for an iteration
    /// </summary>
    [Serializable]
    public class Iteration : StoryCardCollection, IEquatable<Iteration>
    {
        #region Fields

        private Trackable _tracking;
        private Visualizable _view;

        #endregion Fields

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
        /// Gets and sets the work-time that is available for the iteration
        /// </summary>
        public decimal AvailableEffort { get; set; }

        /// <summary>
        /// Gets and sets the end-date of the iteration
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets and sets the start-date of the iteration
        /// </summary>
        public DateTime StartDate { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Iteration"/> class.
        /// </summary>
        public Iteration()
        {
            AvailableEffort = 0;
            StartDate = TimeProvider.Current.Today;
            EndDate = TimeProvider.Current.Today.AddDays(14);

            _tracking = new Trackable();
            _view = new Visualizable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Iteration"/> class.
        /// </summary>
        /// <param name="original">The original.</param>
        private Iteration(Iteration original)
            : this()
        {
            Guard.AssertNotNull(original, "original");

            CopyValuesFrom(original);
        }

        #endregion c'tor

        #region Methods

        /// <summary>
        /// Copies the values of all properties from another iteration. This does not include copying the story-cards
        /// inside the iteration or changing the Parent-field of story-cards inside this iteration.
        /// In case you want a deep copy of this iteration use <see cref="Clone"/> instead.
        /// </summary>
        /// <param name="other">The iteration with the new values</param>
        private void CopyValuesFrom(Iteration other)
        {
            Guard.AssertNotNull(other, "other");

            base.CopyValuesFrom(other);

            AvailableEffort = other.AvailableEffort;

            EndDate = other.EndDate;

            StartDate = other.StartDate;

            Tracking.CopyValuesFrom(other.Tracking);

            View.CopyValuesFrom(other.View);
        }

        #endregion Methods

        #region Overrides of Object

        public bool Equals(Iteration other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = base.Equals(other);
            equal &= AvailableEffort == other.AvailableEffort;
            equal &= DateTime.Equals(EndDate, other.EndDate);
            equal &= DateTime.Equals(StartDate, other.StartDate);
            equal &= View.Equals(other.View);
            equal &= Tracking.Equals(other.Tracking);

            return equal;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            Iteration other = obj as Iteration;

            if (other == null)
                return false;

            return Equals(other);
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

        #endregion Overrides of Object

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = new Iteration(this);
            return clone;
        }

        #endregion ICloneable Members
    }
}