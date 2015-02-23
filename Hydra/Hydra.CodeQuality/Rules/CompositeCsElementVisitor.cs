namespace Hydra.CodeQuality.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using StyleCop.CSharp;

    public class CompositeCsElementVisitor : ICsElementVisitor
    {
        private readonly List<ICsElementVisitor> visitors;

        public CompositeCsElementVisitor(params ICsElementVisitor[] visitors)
        {
            this.visitors = new List<ICsElementVisitor>(visitors ?? new ICsElementVisitor[0]);
        }

        public bool Visit(CsElement element, CsElement parentelement, object context)
        {
            return this.visitors.All(visitor => visitor.Visit(element, parentelement, context));
        }
    }
}