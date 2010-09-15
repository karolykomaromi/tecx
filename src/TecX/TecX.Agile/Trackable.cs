using System;

using TecX.Common;

namespace TecX.Agile
{
    /// <summary>
    /// Information related to tracking efforts and progress
    /// </summary>
    [Serializable]
    public class Trackable : Features<Trackable>
    {
        #region Fields

        /// <summary>
        /// Backing field for <see cref="ActualEffort"/>
        /// </summary>
        private decimal _actualEffort;

        /// <summary>
        /// Backing field for <see cref="BestCaseEstimate"/>
        /// </summary>
        private decimal _bestCaseEstimate;

        /// <summary>
        /// Backing field for <see cref="MostLikelyEstimate"/>
        /// </summary>
        private decimal _mostLikelyEstimate;

        /// <summary>
        /// Backing field for <see cref="WorstCaseEstimate"/>
        /// </summary>
        private decimal _worstCaseEstimate;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets and sets the actual effort for completing the task denoted on the index-card
        /// </summary>
        public decimal ActualEffort
        {
            get { return _actualEffort; }
            set
            {
                Guard.AssertCondition(value >= 0, value, "value", "ActualEffort must be >= 0");
                _actualEffort = value;
            }
        }

        /// <summary>
        /// Gets and sets the best-case estimate for completing the task denoted on the index-card
        /// </summary>
        public decimal BestCaseEstimate
        {
            get { return _bestCaseEstimate; }
            set
            {
                Guard.AssertCondition(value >= 0, value, "value", "BestCaseEstimate must be >= 0");
                _bestCaseEstimate = value;
            }
        }

        /// <summary>
        /// Gets and sets the most-likely estimate for completing the task denoted on the index-card
        /// </summary>
        public decimal MostLikelyEstimate
        {
            get { return _mostLikelyEstimate; }
            set
            {
                Guard.AssertCondition(value >= 0, value, "value", "MostLikelyEstimate must be >= 0");
                _mostLikelyEstimate = value;
            }
        }

        /// <summary>
        /// Gets or sets the priority for this story-card
        /// </summary>
        /// <value>The priority.</value>
        public Priority Priority { get; set; }

        /// <summary>
        /// Gets and sets the worst-case estimate for completing the task denoted on the index-card
        /// </summary>
        public decimal WorstCaseEstimate
        {
            get { return _worstCaseEstimate; }
            set
            {
                Guard.AssertCondition(value >= 0, value, "value", "WorstCaseEstimate must be >= 0");
                _worstCaseEstimate = value;
            }
        }

        /// <summary>
        /// Gets and sets the current status of the task denoted on the index-card
        /// </summary>
        public Status Status { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Trackable"/> class.
        /// </summary>
        public Trackable()
        {
            ActualEffort = 0;
            BestCaseEstimate = 0;
            MostLikelyEstimate = 0;
            Priority = Priority.NotRanked;
            WorstCaseEstimate = 0;
            Status = Status.Defined;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trackable"/> class.
        /// </summary>
        /// <param name="original">The original.</param>
        private Trackable(Trackable original)
        {
            Guard.AssertNotNull(original, "original");

            ActualEffort = original.ActualEffort;
            BestCaseEstimate = original.BestCaseEstimate;
            MostLikelyEstimate = original.MostLikelyEstimate;
            Priority = original.Priority;
            WorstCaseEstimate = original.WorstCaseEstimate;
            Status = original.Status;
        }

        #endregion c'tor

        #region Methods

        /// <summary>
        /// Copies the values of all properties from another trackable.
        /// </summary>
        /// <param name="other">This trackable with the new values</param>
        internal void CopyValuesFrom(Trackable other)
        {
            Guard.AssertNotNull(other, "other");

            ActualEffort = other.ActualEffort;
            BestCaseEstimate = other.BestCaseEstimate;
            MostLikelyEstimate = other.MostLikelyEstimate;
            Priority = other.Priority;
            Status = other.Status;
            WorstCaseEstimate = other.WorstCaseEstimate;
        }

        #endregion Methods

        #region Overrides of Features

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object Clone()
        {
            var clone = new Trackable(this);

            return clone;
        }

        public override bool Equals(object obj)
        {
            Guard.AssertNotNull(obj, "obj");

            Trackable other = obj as Trackable;

            if (other != null)
                return Equals(other);

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(Trackable other)
        {
            Guard.AssertNotNull(other, "other");

            bool equal = ActualEffort == other.ActualEffort;
            equal &= BestCaseEstimate == other.BestCaseEstimate;
            equal &= MostLikelyEstimate == other.MostLikelyEstimate;
            equal &= Priority == other.Priority;
            equal &= Status == other.Status;
            equal &= WorstCaseEstimate == other.WorstCaseEstimate;

            return equal;
        }

        #endregion Overrides of Features
    }
}