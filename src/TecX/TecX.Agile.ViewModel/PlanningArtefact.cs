using System;
using System.Reflection;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public abstract class PlanningArtefact : ViewModelBase
    {
        #region Fields

        private Guid _id;
        private string _name;
        private string _description;

        private readonly DelegateCommand<Tuple<Guid, string, object, object>> _updatePropertyCommand;

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
        protected PlanningArtefact()
        {
            _id = Guid.Empty;
            _name = string.Empty;
            _description = string.Empty;

            _updatePropertyCommand = new DelegateCommand<Tuple<Guid, string, object, object>>(OnPropertyUpdated);

            Commands.UpdateProperty.RegisterCommand(_updatePropertyCommand);
        }

        #endregion c'tor

        private void OnPropertyUpdated(Tuple<Guid, string, object, object> args)
        {
            Guid artefactId = args.Item1;
            string propertyName = args.Item2;
            object oldValue = args.Item3;
            object newValue = args.Item4;

            if (Id != artefactId)
                return;

            PropertyInfo property = GetType().GetProperty(propertyName);

            property.SetValue(this, newValue, null);
        }
    }
}