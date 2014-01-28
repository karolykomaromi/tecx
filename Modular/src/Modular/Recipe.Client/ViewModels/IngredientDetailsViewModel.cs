namespace Recipe.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;

    public class IngredientDetailsViewModel : TitledViewModel
    {
        public IngredientDetailsViewModel(ResxKey titleKey)
            : base(titleKey)
        {
        }
    }
}
