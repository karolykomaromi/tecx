using System;
using System.Reflection;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Common;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public abstract class PlanningArtefact : ViewModelBase
    {
        #region Fields

        private Guid _id;
        private string _name;
        private string _description;

        private readonly DelegateCommand<PropertyUpdated> _updatePropertyCommand;

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

            _updatePropertyCommand = new DelegateCommand<PropertyUpdated>(OnPropertyUpdated);

            Commands.UpdateProperty.RegisterCommand(_updatePropertyCommand);
        }

        #endregion c'tor

        private readonly object _locker = new object();

        private void OnPropertyUpdated(PropertyUpdated args)
        {
            lock (_locker)
            {
                Guard.AssertNotNull(args, "args");

                if (Id != args.ArtefactId)
                    return;

                PropertyInfo property = GetType().GetProperty(args.PropertyName);

                property.SetValue(this, args.NewValue, null);
            }
        }
    }
}