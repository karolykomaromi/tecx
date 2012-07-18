namespace TecX.Common.Specifications
{
    using System.Collections.Generic;

    /// <summary>
    /// Negates the result of a specification
    /// </summary>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public class NotSpecification<TCandidate> : Specification<TCandidate>
    {
        private readonly ISpecification<TCandidate> wrapped;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotSpecification{TCandidate}"/> class. 
        /// </summary>
        /// <param name="specificationToNegate">
        /// The specification to negate.
        /// </param>
        public NotSpecification(ISpecification<TCandidate> specificationToNegate)
        {
            Guard.AssertNotNull(specificationToNegate, "specificationToNegate");

            this.wrapped = specificationToNegate;
        }

        /// <inheritdoc/>
        public override string Description
        {
            get { return "NOT"; }
        }

        /// <summary>
        /// Gets the original specification.
        /// </summary>
        public ISpecification<TCandidate> Wrapped
        {
            get { return this.wrapped; }
        }

        /// <inheritdoc/>
        protected override bool IsMatchCore(TCandidate candidate, ICollection<ISpecification<TCandidate>> matchedSpecifications)
        {
            return !this.Wrapped.IsMatch(candidate, matchedSpecifications);
        }
    }
}