namespace Search.ViewModels
{
    using Infrastructure.Dynamic;
    using Infrastructure.Options;

    public class DisableSearchRule : IViewRule
    {
        public void Apply(IRuleContext context)
        {
            Option option = Option.Create((SearchOptionsViewModel o) => o.IsSearchEnabled);

            if (context.Options.KnowsAbout(option))
            {
                bool isSearchEnabled = (bool)context.Options[option];

                IControl searchView;
                if (context.Registry.TryFindById(new ControlId("SearchView"), out searchView))
                {
                    if (isSearchEnabled)
                    {
                        searchView.Enable();
                    }
                    else
                    {
                        searchView.Disable();
                    }
                }
            }
        }
    }
}