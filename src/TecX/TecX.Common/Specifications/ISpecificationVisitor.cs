namespace TecX.Common.Specifications
{
    /// <summary>
    /// Defines a visitor for specifications
    /// </summary>
    /// <typeparam name="TCandidate">The candidate that is evaluated by the specification</typeparam>
    public interface ISpecificationVisitor<TCandidate>
    {
        /// <summary>
        /// Tells the visitor to visit the <paramref name="specification"/>
        /// </summary>
        /// <param name="specification">The specification to visit</param>
        void Visit(ISpecification<TCandidate> specification);
        
        /// <summary>
        /// Tells the visitor to visit the <paramref name="specification"/>
        /// </summary>
        /// <param name="specification">The specification to visit</param>
        void Visit(CompositeSpecification<TCandidate> specification);
    }
}
