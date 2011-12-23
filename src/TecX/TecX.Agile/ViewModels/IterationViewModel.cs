namespace TecX.Agile.ViewModels
{
    using TecX.Event;

    public class IterationViewModel : CardViewModel
    {
        public IterationViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }
    }
}
