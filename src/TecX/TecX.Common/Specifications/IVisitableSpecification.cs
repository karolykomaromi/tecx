namespace TecX.Common.Specifications
{
    /// <summary>
    /// Defines callback for double dispatch call back to visitor
    /// </summary>
    /// <typeparam name="TCandidate">Type of object the specification works on. </typeparam>
    public interface IVisitableSpecification<TCandidate>
    {
        /// <summary>
        /// Double dispatch for call back to the visitor
        /// </summary>
        /// <param name="visitor">The visitor to call back</param>
        void Accept(ISpecificationVisitor<TCandidate> visitor);
    }
}