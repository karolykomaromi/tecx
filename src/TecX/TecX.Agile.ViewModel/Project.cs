using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel
{
    public class Project : PlanningArtefactCollection<Iteration>
    {
        #region Fields

        private Backlog _backlog;
        
        #endregion Fields

        #region Properties

        public Backlog Backlog
        {
            get { return _backlog; }
            set
            {
                Guard.AssertNotNull(value, "value");

                if (_backlog == value)
                    return;

                OnPropertyChanging(() => Backlog);
                _backlog = value;
                OnPropertyChanged(() => Backlog);
            }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class
        /// </summary>
        public Project()
        {
            _backlog = new Backlog();
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

            foreach(Iteration iteration in this)
            {
                if (iteration.Id == id)
                    return iteration as TArtefact;

                foreach(StoryCard storyCard in iteration)
                {
                    if (storyCard.Id == id)
                        return storyCard as TArtefact;
                }
            }

            return null;
        }
    }
}
