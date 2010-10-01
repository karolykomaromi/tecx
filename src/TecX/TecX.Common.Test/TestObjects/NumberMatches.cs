using TecX.Common.Specifications;

namespace TecX.Common.Test.TestObjects
{
    internal class NumberMatches : CompareToValueSpecification<SearchTestEntity, int>
    {
        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberMatches"/> class.
        /// </summary>
        public NumberMatches()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberMatches"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public NumberMatches(int number)
        {
            this.Value = number;
        }

        #endregion c'tor

        #region Specification Members

        /// <summary>
        /// Determines whether a candidate object satifies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        /// 	<c>true</c> if the specification is satisfied by the
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSatisfiedBy(SearchTestEntity candidate)
        {
            return candidate.Number == this.Value;
        }

        #endregion Specification Members
    }
}