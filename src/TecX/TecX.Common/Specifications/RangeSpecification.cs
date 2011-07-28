using System;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Defines an interface for a specification that verifies wether a property of the candidate object
    /// falls within a certain range
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate.</typeparam>
    public abstract class RangeSpecification<TCandidate> : Specification<TCandidate>
    {
        private readonly IComparable _lowerBound;

        private readonly IComparable _upperBound;

        /// <summary>
        /// Gets or sets the lower bound of the range
        /// </summary>
        /// <value>The lower bound.</value>
        public IComparable LowerBound
        {
            get { return _lowerBound; }
        }

        /// <summary>
        /// Gets or sets the upper bound of the range.
        /// </summary>
        /// <value>The upper bound.</value>
        public IComparable UpperBound
        {
            get { return _upperBound; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSpecification{TCandidate}"/> class.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        protected RangeSpecification(IComparable lowerBound, IComparable upperBound)
        {
            Guard.AssertNotNull(lowerBound, "lowerBound");
            Guard.AssertNotNull(upperBound, "upperBound");

            _lowerBound = lowerBound;
            _upperBound = upperBound;
        }
    }
}