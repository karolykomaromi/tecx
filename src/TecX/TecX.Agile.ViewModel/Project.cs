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
    }
}
