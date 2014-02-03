namespace Main.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class OptionsViewModel : Options
    {
        private readonly LanguageSelectionViewModel languageSelection;
        private readonly ThemeSelectionViewModel themeSelection;
        private readonly LocalizedString labelLanguageSelection;
        private readonly LocalizedString labelThemeSelection;

        public OptionsViewModel(ResxKey titleKey, LanguageSelectionViewModel languageSelection, ThemeSelectionViewModel themeSelection)
            : base(titleKey)
        {
            this.languageSelection = languageSelection;
            this.themeSelection = themeSelection;

            this.labelLanguageSelection = new LocalizedString(this, "LabelLanguageSelection", new ResxKey("MAIN.LABEL_LANGUAGESELECTION"), this.OnPropertyChanged);
            this.labelThemeSelection = new LocalizedString(this, "LabelThemeSelection", new ResxKey("MAIN.LABEL_THEMESELECTION"), this.OnPropertyChanged);
        }

        public string LabelLanguageSelection
        {
            get { return this.labelLanguageSelection.Value; }
        }

        public LanguageSelectionViewModel LanguageSelection
        {
            get { return this.languageSelection; }
        }

        public string LabelThemeSelection
        {
            get { return this.labelThemeSelection.Value; }
        }

        public ThemeSelectionViewModel ThemeSelection
        {
            get { return this.themeSelection; }
        }
    }
}