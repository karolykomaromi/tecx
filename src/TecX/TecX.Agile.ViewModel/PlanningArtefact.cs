using System;
using System.ComponentModel;
using System.Linq.Expressions;
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
                Set(() => _id, value);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                Set(() => _name, value);
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                Set(() => _description, value);
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

        protected void Set<T>(Expression<Func<T>> backingFieldSelector, object value)
        {
            Guard.AssertNotNull(backingFieldSelector, "backingFieldSelector");

            FieldInfo field = GetBackingField(backingFieldSelector);

            object currentValue = field.GetValue(this);

            if (Equals(currentValue, value))
            {
                return;
            }

            string propertyName = GetPropertyName(field.Name);

            OnPropertyChanging(propertyName);

            field.SetValue(this, value);

            OnPropertyChanged(propertyName);

            EventAggregator.Publish(new PropertyUpdated(Id, propertyName, currentValue, value));
        }

        private static string GetPropertyName(string backingFieldName)
        {
            string propertyName = backingFieldName.Substring(1, 1).ToUpper() + backingFieldName.Substring(2);

            return propertyName;
        }

        private static FieldInfo GetBackingField<T>(Expression<Func<T>> fieldSelector)
        {
            var expr = (MemberExpression)fieldSelector.Body;

            var field = (FieldInfo)expr.Member;

            return field;
        }
    }
}