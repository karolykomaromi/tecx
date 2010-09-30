namespace TecX.Common.Specifications
{
    /// <summary>
    /// Specification that links two other specifications using logical OR
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type the specification should work on</typeparam>
    public class AndSpecification<TCandidate> : CompositeSpecification<TCandidate>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification&lt;TCandidate&gt;"/> class.
        /// </summary>
        /// <param name="leftSide">The first specification</param>
        /// <param name="rightSide">The second specification</param>
        public AndSpecification(ISpecification<TCandidate> leftSide, ISpecification<TCandidate> rightSide)
            : base(leftSide, rightSide)
        {
        }

        #endregion c'tor

        #region ISpecification Members

        /// <summary>
        /// Determines whether a candidate object satifies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        /// 	<c>true</c> if the specification is satisfied by the
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSatisfiedBy(TCandidate candidate)
        {
            Guard.AssertNotNull(candidate, "candidate");

            return LeftSide.IsSatisfiedBy(candidate) && RightSide.IsSatisfiedBy(candidate);
        }

        #endregion ISpecification Members
    }
}