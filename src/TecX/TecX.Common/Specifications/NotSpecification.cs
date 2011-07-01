namespace TecX.Common.Specifications
{
    /// <summary>
    /// Negates the result of a specification
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public class NotSpecification<TCandidate> : Specification<TCandidate>
    {
        private readonly ISpecification<TCandidate> _wrapped;

        /// <summary>
        /// Gets the original specification.
        /// </summary>
        public ISpecification<TCandidate> Wrapped
        {
            get { return _wrapped; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="specificationToNegate">The specification to negate.</param>
        public NotSpecification(ISpecification<TCandidate> specificationToNegate)
        {
            Guard.AssertNotNull(specificationToNegate, "specificationToNegate");

            _wrapped = specificationToNegate;
        }

        /// <inheritdoc/>
        protected override bool IsMatchCore(TCandidate candidate)
        {
            return !Wrapped.IsMatch(candidate);
        }
    }
}