﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel
{
    public class Project : PlanningArtefactCollection<Iteration>
    {
        private readonly IActionManager _actionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="Project"/> class
        /// </summary>
        public Project(IActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actionManager = actionManager;
        }

        #region Overrides of PlanningArtefactCollection<Iteration>

        protected override void AddCore(Iteration item)
        {
            item.Project = this;
        }

        protected override void RemoveCore(Iteration item)
        {
            item.Project = null;
        }

        #endregion
    }
}