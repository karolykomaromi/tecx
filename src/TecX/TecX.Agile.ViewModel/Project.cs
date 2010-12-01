using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Practices.Prism.Commands;

namespace TecX.Agile.ViewModel
{
    public class Project : IterationCollection, IEnumerable<PlanningArtefact>
    {
        #region Fields

        private readonly Backlog _backlog;
        private readonly DelegateCommand<Tuple<Guid, string, object, object>> _updatePropertyCommand;

        #endregion Fields

        #region Properties

        public Backlog Backlog
        {
            get { return _backlog; }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class
        /// </summary>
        public Project()
        {
            _backlog = new Backlog();
            _updatePropertyCommand = new DelegateCommand<Tuple<Guid, string, object, object>>(OnPropertyUpdated);
        }

        private void OnPropertyUpdated(Tuple<Guid, string, object, object> args)
        {
            Guid artefactId = args.Item1;
            string propertyName = args.Item2;
            object oldValue = args.Item3;
            object newValue = args.Item4;

            PlanningArtefact existing = Find<PlanningArtefact>(artefactId);

            PropertyInfo property = existing.GetType().GetProperty(propertyName);

            property.SetValue(existing, newValue, null);
        }

        #endregion c'tor

        #region Overrides of PlanningArtefactCollection<Iteration>

        protected internal override void AddCore(Iteration item)
        {
            item.Project = this;
        }

        protected override void RemoveCore(Iteration item)
        {
            item.Project = null;
        }

        #endregion Overrides of PlanningArtefactCollection<Iteration>

        public TArtefact Find<TArtefact>(Guid id)
            where TArtefact : PlanningArtefact
        {
            if (Id == id)
                return this as TArtefact;

            if (Backlog.Id == id)
                return Backlog as TArtefact;

            foreach (Iteration iteration in this)
            {
                if (iteration.Id == id)
                    return iteration as TArtefact;

                foreach (StoryCard storyCard in iteration)
                {
                    if (storyCard.Id == id)
                        return storyCard as TArtefact;
                }
            }

            return null;
        }

        #region Implementation of IEnumerable<out PlanningArtefact>

        IEnumerator<PlanningArtefact> IEnumerable<PlanningArtefact>.GetEnumerator()
        {
            yield return this;

            yield return _backlog;

            foreach (StoryCard storyCard in Backlog)
            {
                yield return storyCard;
            }

            foreach (Iteration iteration in Project)
            {
                yield return iteration;

                foreach (StoryCard storyCard in iteration)
                {
                    yield return storyCard;
                }
            }
        }

        #endregion Implementation of IEnumerable<out PlanningArtefact>
    }
}
