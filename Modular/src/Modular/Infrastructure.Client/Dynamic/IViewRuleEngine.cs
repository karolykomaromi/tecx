namespace Infrastructure.Dynamic
{
    using System.Diagnostics.Contracts;

    public interface IViewRuleEngine
    {
        void Add(IViewRule rule);

        void EvaluateAllRules();
    }

    internal abstract class ViewRuleEngineContract : IViewRuleEngine
    {
        public void Add(IViewRule rule)
        {
            Contract.Requires(rule != null);
        }

        public void EvaluateAllRules()
        {
        }
    }
}
