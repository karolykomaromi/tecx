using System.Collections.Generic;

namespace TecX.Common.Specifications
{
    /// <summary>
    /// Always returns false no matter what the candidate is
    /// </summary>
    /// <typeparam name="TCandidate">Type of the candidate to evaluate</typeparam>
    public class AlwaysFalseSpecification<TCandidate> : Specification<TCandidate>
    {
        /// <inheritdoc/>
        public override string Description
        {
            get { return "FALSE"; }
        }

        /// <inheritdoc/>
        protected override bool IsMatchCore(TCandidate candidate, ICollection<ISpecification<TCandidate>> matchedSpecifications)
        {
            return false;
        }
    }
}
