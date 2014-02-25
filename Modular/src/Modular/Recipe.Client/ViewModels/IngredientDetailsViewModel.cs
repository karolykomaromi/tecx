namespace Recipe.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Recipe.Assets.Resources;

    public class IngredientDetailsViewModel : TitledViewModel
    {
        private readonly LocalizedString title;

        public IngredientDetailsViewModel()
        {
            this.title = new LocalizedString(() => this.Title, () => Labels.Ingredients, this.OnPropertyChanged);
        }

        public override string Title
        {
            get { return this.title.Value; }
        }
    }
}
