namespace TecX.Common.Specifications
{
    using System;

    /// <summary>
    /// Defines an interface for a specification that verifies wether a property of the candidate object
    /// falls within a certain range
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate.</typeparam>
    public abstract class RangeSpecification<TCandidate> : Specification<TCandidate>
    {
        private readonly IComparable lowerBound;

        private readonly IComparable upperBound;

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeSpecification{TCandidate}"/> class.
        /// </summary>
        /// <param name="lowerBound">The lower bound.</param>
        /// <param name="upperBound">The upper bound.</param>
        protected RangeSpecification(IComparable lowerBound, IComparable upperBound)
        {
            Guard.AssertNotNull(lowerBound, "lowerBound");
            Guard.AssertNotNull(upperBound, "upperBound");

            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }

        /// <summary>
        /// Gets the lower bound of the range
        /// </summary>
        /// <value>The lower bound.</value>
        public IComparable LowerBound
        {
            get { return this.lowerBound; }
        }

        /// <summary>
        /// Gets the upper bound of the range.
        /// </summary>
        /// <value>The upper bound.</value>
        public IComparable UpperBound
        {
            get { return this.upperBound; }
        }
    }
}