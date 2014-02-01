namespace Main.ViewModels
{
    using System.Diagnostics.Contracts;
    using Infrastructure.Dynamic;
    using Infrastructure.I18n;
    using Infrastructure.ViewModels;

    public class OptionsViewModel : TitledViewModel
    {
        private readonly IViewRegistry registry;
        private readonly LocalizedString labelShowSomething;
        private bool showSomething;

        public OptionsViewModel(IViewRegistry registry, ResxKey titleKey)
            : base(titleKey)
        {
            Contract.Requires(registry != null);

            this.registry = registry;
            this.labelShowSomething = LocalizedString.Create(this, vm => vm.LabelShowSomething, this.OnPropertyChanged);
        }

        public string LabelShowSomething
        {
            get
            {
                return this.labelShowSomething.Value;
            }
        }

        public bool ShowSomething
        {
            get
            {
                return this.showSomething;
            }

            set
            {
                if (this.showSomething != value)
                {
                    this.OnPropertyChanging(() => this.ShowSomething);
                    this.showSomething = value;
                    this.OnPropertyChanged(() => this.ShowSomething);
                }
            }
        }
    }
}
