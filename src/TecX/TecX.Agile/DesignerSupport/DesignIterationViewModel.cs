namespace TecX.Agile.DesignerSupport
{
    using TecX.Agile.ViewModels;

    internal class DesignIterationViewModel : IterationViewModel
    {
        public DesignIterationViewModel()
            : base(new NullEventAggregator())
        {
        }
    }
}