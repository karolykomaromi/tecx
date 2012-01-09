namespace TecX.Common.Specifications
{
    using System.Collections.Generic;

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
        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification&lt;TCandidate&gt;"/> class.
        /// </summary>
        /// <param name="leftSide">The first specification</param>
        /// <param name="rightSide">The second specification</param>
        public AndSpecification(ISpecification<TCandidate> leftSide, ISpecification<TCandidate> rightSide)
            : base(leftSide, rightSide)
        {
        }

        /// <inheritdoc/>
        public override string Description
        {
            get { return "AND"; }
        }

        /// <summary>
        /// Determines whether a candidate object satifies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <param name="matchedSpecifications"></param>
        /// <returns>
        ///     <c>true</c> if the specification is satisfied by the
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        protected override bool IsMatchCore(TCandidate candidate, ICollection<ISpecification<TCandidate>> matchedSpecifications)
        {
            bool isMatch = LeftSide.IsMatch(candidate, matchedSpecifications) && 
                           RightSide.IsMatch(candidate, matchedSpecifications);

            return isMatch;
        }
    }
}