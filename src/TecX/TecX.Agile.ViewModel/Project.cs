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

        private readonly IActionManager _actionManager;

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

        public IRemoteUI RemoteUI { get; set; }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class
        /// </summary>
        public Project(IActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actionManager = actionManager;

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

        public void NotifyFieldHighlighted(Guid artefactId, string fieldName)
        {
            Guard.AssertNotEmpty(fieldName, "fieldName");

            if(RemoteUI != null)
            {
                RemoteUI.HighlightField(artefactId, fieldName);
            }
        }

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

    public interface IRemoteUI
    {
        void HighlightField(Guid artefactId, string fieldName);
    }
}
