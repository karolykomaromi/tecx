namespace TecX.Agile.ViewModel.ChangeTracking
{
    public interface IChangeTracker
    {
        void Subscribe(PlanningArtefact item);

        void Unsubscribe(PlanningArtefact item);

        void Subscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection)
            where TArtefact : PlanningArtefact;
        
        void Unsubscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection)
            where TArtefact : PlanningArtefact;
        
        void Subscribe(StoryCardCollection collection);

        void Unsubscribe(StoryCardCollection collection);
    }
}
