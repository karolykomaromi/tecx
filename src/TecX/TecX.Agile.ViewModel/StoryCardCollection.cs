namespace TecX.Agile.ViewModel
{
    public class StoryCardCollection : PlanningArtefactCollection<StoryCard>
    {
        public Project Project { get; internal set; }

        #region Overrides of PlanningArtefactCollection<StoryCard>

        protected override void AddCore(StoryCard item)
        {
            item.Parent = this;
        }

        protected override void RemoveCore(StoryCard item)
        {
            item.Parent = null;
        }

        #endregion
    }
}