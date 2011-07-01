namespace TecX.Common.Specifications
{
    /// <summary>
    /// Part of the <c>Specification Pattern</c>
    /// </summary>
    /// <remarks>
    /// See http://en.wikipedia.org/wiki/Specification_pattern for further information on the pattern
    /// </remarks>
    /// <typeparam name="TCandidate">The type of the candidate objects the specification should
    /// work on</typeparam>
    public interface ISpecification<TCandidate> : IFluentInterface
    {
        /// <summary>
        /// Determines whether a candidate object satifies the specification
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>
        /// 	<c>true</c> if the specification is satisfied by the 
        /// candidate object; otherwise, <c>false</c>.
        /// </returns>
        bool IsMatch(TCandidate candidate);
 
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