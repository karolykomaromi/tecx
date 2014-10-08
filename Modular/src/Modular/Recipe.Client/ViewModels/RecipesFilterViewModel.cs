namespace Recipe.ViewModels
{
    using Infrastructure.ViewModels;

    public class RecipesFilterViewModel : FilterViewModel
    {
        public override Filter GetFilter()
        {
            return new NullFilter();
        }
    }
}
