namespace TecX.Agile.DesignerSupport
{
    using TecX.Agile.ViewModels;

    internal class DesignStoryCardViewModel : StoryCardViewModel
    {
        public DesignStoryCardViewModel()
            : base(new NullEventAggregator())
        {
        }
    }
}
