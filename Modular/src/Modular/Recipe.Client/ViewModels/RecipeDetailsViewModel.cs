namespace Recipe.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Recipe.Assets.Resources;

    public class RecipeDetailsViewModel : TitledViewModel
    {
        private readonly LocalizedString title;

        public RecipeDetailsViewModel()
        {
            this.title = new LocalizedString(() => this.Title, () => Labels.Recipes, this.OnPropertyChanged);
        }

        public override string Title
        {
            get { return this.title.Value; }
        }
    }
}
