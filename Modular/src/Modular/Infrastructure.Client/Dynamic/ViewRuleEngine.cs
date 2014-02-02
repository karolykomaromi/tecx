namespace Infrastructure.Dynamic
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Infrastructure.Events;
    using Infrastructure.Options;

    public class ViewRuleEngine : IViewRuleEngine, ISubscribeTo<IOptionsChanged<IOptions>>
    {
        private readonly IOptions appWideOptions;

        private readonly HashSet<IViewRule> rules;

        public ViewRuleEngine(IOptions appWideOptions)
        {
            Contract.Requires(appWideOptions != null);

            this.appWideOptions = appWideOptions;
            this.rules = new HashSet<IViewRule>();
        }

        public void Add(IViewRule rule)
        {
            this.rules.Add(rule);
        }

        public void EvaluateAllRules()
        {
            var context = new RuleContext(ViewRegistry.Current, this.appWideOptions);

            foreach (IViewRule rule in this.rules)
            {
                rule.Apply(context);
            }
        }

        public void Handle(IOptionsChanged<IOptions> message)
        {
            this.EvaluateAllRules();
        }
    }
}