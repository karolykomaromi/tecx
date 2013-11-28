namespace TecX.Build.Rules
{
    using System.Collections.Generic;
    using System.Linq;
    using StyleCop.CSharp;

    using TecX.Common;

    public class CompositeVisitor : CodeVisitor
    {
        private readonly IEnumerable<CodeVisitor> visitors;

        public CompositeVisitor(IEnumerable<CodeVisitor> visitors)
        {
            Guard.AssertNotNull(visitors, "visitors");

            this.visitors = new List<CodeVisitor>(visitors);
        }

        public override bool Visit(CsElement element, CsElement parentelement, object context)
        {
            return this.visitors.Select(visitor => visitor.Visit(element, parentelement, context)).All(b => b);
        }
    }
}