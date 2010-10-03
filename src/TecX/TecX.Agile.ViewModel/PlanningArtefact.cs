using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel
{
    public abstract class PlanningArtefact : ViewModelBase
    {
        #region Fields

        private readonly IActionManager _actionManager;
        private Guid _id;
        private string _name;
        private string _description;

        #endregion Fields

        #region Properties

        public Guid Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;

                OnPropertyChanging(() => Id);
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                OnPropertyChanging(() => Name);
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                OnPropertyChanging(() => Description);
                _description = value;
                OnPropertyChanged(() => Description);
            }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefact"/> class
        /// </summary>
        protected PlanningArtefact(IActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actionManager = actionManager;
        }

        #endregion c'tor
    }
}
