namespace TecX.Common.Specifications
{
    using System.Collections.Generic;

    /// <summary>
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public interface ISpecification<TCandidate> : IFluentInterface, IVisitableSpecification<TCandidate>
    {
        /// <summary>
        /// Gets a brief description of this specification
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Determines whether a candidate object satisfies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <param name="matchedSpecifications">List of specifications that were matched by the candidate</param>
        /// <returns>
        ///     <c>true</c> if the specification is satisfied by the 
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        bool IsMatch(TCandidate candidate, ICollection<ISpecification<TCandidate>> matchedSpecifications);
 
        /// <summary>
        /// Links two specifications using logical AND
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> And(ISpecification<TCandidate> other);

        /// <summary>
        /// Links two specifications using logical OR
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> Or(ISpecification<TCandidate> other);

        /// <summary>
        /// Negates the result of the current specification
        /// </summary>
        /// <returns>The negated specification</returns>
        ISpecification<TCandidate> Not();

        /// <summary>
        /// Links two specifications using logical NOT AND (NAND)
        /// </summary>
        /// <param name="other">The other specification</param>
        /// <returns>The linked specifications</returns>
        ISpecification<TCandidate> AndNot(ISpecification<TCandidate> other);
    }
}