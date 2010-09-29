namespace TecX.Common.Specifications
{
    /// <summary>
    /// Negates the result of a specification
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public class NotSpecification<TCandidate> : Specification<TCandidate>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the original specification.
        /// </summary>
        protected ISpecification<TCandidate> Wrapped { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="specificationToNegate">The specification to negate.</param>
        public NotSpecification(ISpecification<TCandidate> specificationToNegate)
        {
            Guard.AssertNotNull(specificationToNegate, "specificationToNegate");

            Wrapped = specificationToNegate;
        }

        #endregion c'tor

        #region ISpecification Members

        /// <summary>
        /// Determines whether [is satisfied by] [the specified candidate].
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        /// 	<c>true</c> if [is satisfied by] [the specified candidate]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsSatisfiedBy(TCandidate candidate)
        {
            return !Wrapped.IsSatisfiedBy(candidate);
        }

        #endregion ISpecification Members
    }
}