using System;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Defines an interface for a specification that verifies wether a property of the candidate object
    /// falls within a certain range
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate.</typeparam>
    public abstract class InRangeSpecification<TCandidate> : Specification<TCandidate>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the lower bound of the range
        /// </summary>
        /// <value>The lower bound.</value>
        public IComparable LowerBound { get; set; }

        /// <summary>
        /// Gets or sets the upper bound of the range.
        /// </summary>
        /// <value>The upper bound.</value>
        public IComparable UpperBound { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="InRangeSpecification&lt;TCandidate&gt;"/> class.
        /// </summary>
        protected InRangeSpecification()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InRangeSpecification&lt;TCandidate&gt;"/> class.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        protected InRangeSpecification(IComparable lowerBound, IComparable upperBound)
        {
            Guard.AssertNotNull(lowerBound, "lowerBound");
            Guard.AssertNotNull(upperBound, "upperBound");

            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        #endregion c'tor
    }
}