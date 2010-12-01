using TecX.Agile.ViewModel;

namespace TecX.Agile.ChangeTracking
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
        void Subscribe(Iteration iteration);
        void Unsubscribe(Iteration iteration);
    }
}
