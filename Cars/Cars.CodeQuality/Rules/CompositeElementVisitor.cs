namespace Cars.CodeQuality.Rules
{
    using System.Collections.Generic;
    using StyleCop.CSharp;

    public class CompositeElementVisitor : IElementVisitor
    {
        private readonly List<IElementVisitor> visitors;

        public CompositeElementVisitor(params IElementVisitor[] visitors)
        {
            this.visitors = new List<IElementVisitor>(visitors ?? new IElementVisitor[0]);
        }

        public bool Visit(CsElement element, CsElement parentelement, object context)
        {
            bool b = true;
            
            this.visitors.ForEach(visitor => b &= visitor.Visit(element, parentelement, context));

            return b;
        }
    }
}