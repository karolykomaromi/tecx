namespace TecX.Common.Specifications
{
    /// <summary>
    /// Base class for specifications that links two other specifications using logical operators
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public abstract class CompositeSpecification<TCandidate> : Specification<TCandidate>
    {
        /// <summary>
        /// Gets or sets the left side of a <see cref="CompositeSpecification{T}"/>. Which is the
        /// left side of a logical operator
        /// </summary>
        /// <value>The left side of a logical operation (first specification in the composition)</value>
        public ISpecification<TCandidate> LeftSide { get; private set; }

        /// <summary>
        /// Gets or sets the right side of a <see cref="CompositeSpecification{T}"/>. Which is the
        /// right side of a logical operator
        /// </summary>
        /// <value>The right side of a logical operation (second specification in the composition)</value>
        public ISpecification<TCandidate> RightSide { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="leftSide">The left side.</param>
        /// <param name="rightSide">The right side.</param>
        protected CompositeSpecification(ISpecification<TCandidate> leftSide, ISpecification<TCandidate> rightSide)
        {
            Guard.AssertNotNull(leftSide, "leftSide");
            Guard.AssertNotNull(rightSide, "rightSide");

            LeftSide = leftSide;
            RightSide = rightSide;
        }
    }
}