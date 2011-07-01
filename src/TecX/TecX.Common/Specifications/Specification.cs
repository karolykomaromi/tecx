using System;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Abstract base class for all specifications.
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public abstract class Specification<TCandidate> : ISpecification<TCandidate>
    {
        /// <inheritdoc/>
        public bool IsMatch(TCandidate candidate)
        {
            Guard.AssertNotNull(candidate, "candidate");

            bool isMatch = IsMatchCore(candidate);

            return isMatch;
        }

        /// <summary>
        /// Implementers must put the core validation logic here
        /// </summary>
        /// <param name="candidate">The candidate object to validate</param>
        /// <returns><c>true</c> if the candidate matches this specification; <c>false</c> otherwise</returns>
        protected abstract bool IsMatchCore(TCandidate candidate);

        /// <inheritdoc/>
        public ISpecification<TCandidate> And(ISpecification<TCandidate> other)
        {
            Guard.AssertNotNull(other, "other");

            return new AndSpecification<TCandidate>(this, other);
        }
        
        /// <inheritdoc/>
        public ISpecification<TCandidate> Or(ISpecification<TCandidate> other)
        {
            Guard.AssertNotNull(other, "other");

            return new OrSpecification<TCandidate>(this, other);
        }

        /// <inheritdoc/>
        public ISpecification<TCandidate> Not()
        {
            return new NotSpecification<TCandidate>(this);
        }

        /// <inheritdoc/>
        public ISpecification<TCandidate> AndNot(ISpecification<TCandidate> other)
        {
            return new AndSpecification<TCandidate>(this, other.Not());
        }
    }
}