namespace Recipe.ViewModels
{
    using Infrastructure.ViewModels;

    public class IngredientsFilterViewModel : FilterViewModel
    {
        public override Filter GetFilter()
        {
            return new NullFilter();
        }
    }
}
