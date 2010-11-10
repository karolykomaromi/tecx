using System;

using TecX.Common;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public class StoryCard : PlanningArtefact
    {
        public PlanningArtefactCollection<StoryCard> Parent { get; internal set; }
    }
}