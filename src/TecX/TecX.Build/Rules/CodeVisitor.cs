namespace TecX.Build.Rules
{
	using StyleCop.CSharp;

	public abstract class CodeVisitor
	{
		public abstract bool Visit(CsElement element, CsElement parentelement, object context);
	}
}