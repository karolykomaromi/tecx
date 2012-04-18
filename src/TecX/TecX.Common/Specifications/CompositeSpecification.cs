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
        private readonly ISpecification<TCandidate> leftSide;

        private readonly ISpecification<TCandidate> rightSide;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSpecification{TCandidate}"/> class.
        /// </summary>
        /// <param name="leftSide">
        /// The left side.
        /// </param>
        /// <param name="rightSide">
        /// The right side.
        /// </param>
        protected CompositeSpecification(ISpecification<TCandidate> leftSide, ISpecification<TCandidate> rightSide)
        {
            Guard.AssertNotNull(leftSide, "leftSide");
            Guard.AssertNotNull(rightSide, "rightSide");

            this.leftSide = leftSide;
            this.rightSide = rightSide;
        }

        /// <summary>
        /// Gets the left side of a <see cref="CompositeSpecification{T}"/>. Which is the
        /// left side of a logical operator
        /// </summary>
        /// <value>The left side of a logical operation (first specification in the composition)</value>
        public ISpecification<TCandidate> LeftSide
        {
            get { return this.leftSide; }
        }

        /// <summary>
        /// Gets the right side of a <see cref="CompositeSpecification{T}"/>. Which is the
        /// right side of a logical operator
        /// </summary>
        /// <value>The right side of a logical operation (second specification in the composition)</value>
        public ISpecification<TCandidate> RightSide
        {
            get { return this.rightSide; }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The visitor shall be able to define the order in which it travels through the tree of specifications all by itself.
        /// Thus there is an overload of <see cref="ISpecificationVisitor{TCandidate}.Visit(CompositeSpecification{TCandidate})"/>
        /// that is called here.
        /// </remarks>
        public override void Accept(ISpecificationVisitor<TCandidate> visitor)
        {
            Guard.AssertNotNull(visitor, "visitor");

            visitor.Visit(this);
        }
    }
}