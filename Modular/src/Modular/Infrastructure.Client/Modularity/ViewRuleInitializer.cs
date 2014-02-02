namespace Infrastructure.Modularity
{
    using Infrastructure.Dynamic;

    public class ViewRuleInitializer : IModuleInitializer
    {
        private readonly IViewRuleEngine ruleEngine;

        public ViewRuleInitializer(IViewRuleEngine ruleEngine)
        {
            this.ruleEngine = ruleEngine;
        }

        public void Initialize(UnityModule module)
        {
            module.ConfigureViewRules(this.ruleEngine);
        }
    }
}
