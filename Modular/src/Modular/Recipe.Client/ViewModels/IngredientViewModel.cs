namespace Recipe.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;
    using Recipe.Assets.Resources;

    public class IngredientViewModel : ViewModel
    {
        private readonly LocalizedString labelName;

        private LocalizedString name;

        public IngredientViewModel()
        {
            this.labelName = new LocalizedString(() => this.LabelName, () => Labels.Name, this.OnPropertyChanged);
            this.name = new LocalizedString(() => this.Name, () => "INGREDIENTVIEWMODEL.NAME", this.OnPropertyChanged);
        }

        public string LabelName
        {
            get { return this.labelName.Value; }
        }

        public string Name
        {
            get
            {
                return this.name.Value;
            }

            set
            {
                ResourceAccessor resourceAccessor = ResourceAccessor.Create(value);

                this.name = new LocalizedString(() => this.Name, resourceAccessor.GetResource, this.OnPropertyChanged);

                this.OnPropertyChanged(() => this.Name);
            }
        }
    }
}
