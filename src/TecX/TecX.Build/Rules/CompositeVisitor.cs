namespace TecX.Build.Rules
{
	using System.Collections.Generic;
	using StyleCop.CSharp;

	using TecX.Common;

	public class CompositeVisitor : CodeVisitor
	{
		private readonly IEnumerable<CodeVisitor> visitors;

		public CompositeVisitor(IEnumerable<CodeVisitor> visitors)
		{
			Guard.AssertNotNull(visitors, "visitors");

			this.visitors = visitors;
		}

		public override bool Visit(CsElement element, CsElement parentelement, object context)
		{
			foreach (var visitor in visitors)
			{
				bool b = visitor.Visit(element, parentelement, context);

				if (!b)
				{
					return false;
				}
			}

			return true;
		}
	}
}