using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TecX.Agile.ViewModel
{
    public interface IChangeTracker
    {
        void Subscribe(PlanningArtefact item);

        void Unsubscribe(PlanningArtefact item);

        void Subscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection)
            where TArtefact : PlanningArtefact;

        void Unsubscribe<TArtefact>(PlanningArtefactCollection<TArtefact> collection)
            where TArtefact : PlanningArtefact;
    }
}
