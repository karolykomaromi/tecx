namespace TecX.Agile.ViewModel
{
    public class StoryCard : PlanningArtefact
    {
        public PlanningArtefactCollection<StoryCard> Parent { get; internal set; }
    }
}