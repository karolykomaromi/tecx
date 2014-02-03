namespace Main.ViewModels
{
    using Infrastructure.I18n;
    using Infrastructure.Options;

    public class OptionsViewModel : Options
    {
        public OptionsViewModel(ResxKey titleKey)
            : base(titleKey)
        {
        }
    }
}