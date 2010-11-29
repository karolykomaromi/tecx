using TecX.Agile.ViewModel;
using TecX.Common;

namespace TecX.Agile.Infrastructure.Events
{
    public class PropertyChanged : IDomainEvent
    {
        #region Fields

        private readonly PlanningArtefact _parentObject;
        private readonly string _propertyName;
        private readonly object _oldValue;
        private readonly object _newValue;

        #endregion Fields

        #region Properties

        public object NewValue
        {
            get { return _newValue; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
        }

        public PlanningArtefact ParentObject
        {
            get { return _parentObject; }
        }

        #endregion Properties

        #region c'tor

        public PropertyChanged(PlanningArtefact parentObject, string propertyName, object oldValue, object newValue)
        {
            Guard.AssertNotNull(parentObject, "parentObject");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            _parentObject = parentObject;
            _propertyName = propertyName;
            _oldValue = oldValue;
            _newValue = newValue;
        }

        #endregion c'tor
    }
}