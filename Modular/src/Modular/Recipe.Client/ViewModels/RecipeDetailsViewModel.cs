namespace Recipe.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;

    public class RecipeDetailsViewModel : TitledViewModel
    {
        public RecipeDetailsViewModel(ResxKey titleKey)
            : base(titleKey)
        {
        }
    }
}
