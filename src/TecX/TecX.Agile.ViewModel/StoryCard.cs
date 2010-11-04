using System;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public class StoryCard : PlanningArtefact
    {
        public PlanningArtefactCollection<StoryCard> Parent { get; internal set; }
    }
}