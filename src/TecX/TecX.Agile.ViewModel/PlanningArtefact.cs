using System;
using System.Reflection;

using Microsoft.Practices.Prism.Commands;

using TecX.Agile.Infrastructure;
using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Event;

namespace TecX.Agile.ViewModel
{
    [Serializable]
    public abstract class PlanningArtefact : ViewModelBase
    {

        #region Fields

        private Guid _id;
        private string _name;
        private string _description;
        private IEventAggregator _eventAggregator;
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

                Guid pre = _id;
                Guid post = value;

                string propertyName = GetPropertyName(() => Id);

                OnPropertyChanging(propertyName);

                _id = value;

                OnPropertyChanged(propertyName);

                EventAggregator.Publish(new PropertyUpdated(Id, propertyName, pre, post));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;

                string pre = _name;
                string post = value;

                string propertyName = GetPropertyName(() => Name);

                OnPropertyChanging(propertyName);

                _name = value;

                OnPropertyChanged(propertyName);

                EventAggregator.Publish(new PropertyUpdated(Id, propertyName, pre, post));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                    return;

                string pre = _description;
                string post = value;

                string propertyName = GetPropertyName(() => Description);

                OnPropertyChanging(propertyName);

                _description = value;

                OnPropertyChanged(propertyName);

                EventAggregator.Publish(new PropertyUpdated(Id, propertyName, pre, post));
            }
        }

        public IEventAggregator EventAggregator
        {
            get { return _eventAggregator; }
            set
            {
                Guard.AssertNotNull(value, "EventAggregator");

                _eventAggregator = value;
            }
        }

        #endregion Properties

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="PlanningArtefact"/> class
        /// </summary>
        protected PlanningArtefact()
            : this(new NullEventAggregator())
        {
        }

        protected PlanningArtefact(IEventAggregator eventAggregator)
        {
            Guard.AssertNotNull(eventAggregator, "eventAggregator");

            _eventAggregator = eventAggregator;
            _id = Guid.Empty;
            _name = string.Empty;
            _description = string.Empty;

            _updatePropertyCommand = new DelegateCommand<PropertyUpdated>(OnPropertyUpdated);

            Commands.UpdateProperty.RegisterCommand(_updatePropertyCommand);
        }

        #endregion c'tor

        private void OnPropertyUpdated(PropertyUpdated args)
        {
            Guard.AssertNotNull(args, "args");

            if (Id != args.ArtefactId)
                return;

            PropertyInfo property = GetType().GetProperty(args.PropertyName);

            property.SetValue(this, args.NewValue, null);
        }
    }
}