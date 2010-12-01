using System.Reflection;

using TecX.Common;
using TecX.Undo;

namespace TecX.Agile.ViewModel.Undo
{
    /// <summary>
    /// This is a sample action that can change any property on any object
    /// It can also undo what it did
    /// </summary>
    public class SetPropertyAction : AbstractAction
    {
        #region Fields

        private readonly PlanningArtefact _parentObject;
        private readonly PropertyInfo _property;
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

        public PropertyInfo Property
        {
            get { return _property; }
        }

        public PlanningArtefact ParentObject
        {
            get { return _parentObject; }
        }

        #endregion Properties

        #region c'tor

        public SetPropertyAction(PlanningArtefact parentObject, string propertyName, object oldValue, object newValue)
        {
            Guard.AssertNotNull(parentObject, "parentObject");
            Guard.AssertNotEmpty(propertyName, "propertyName");

            _parentObject = parentObject;
            _property = parentObject.GetType().GetProperty(propertyName);
            _oldValue = oldValue;
            _newValue = newValue;
        }

        #endregion c'tor

        protected override void ExecuteCore()
        {
            _property.SetValue(_parentObject, _newValue, null);
        }

        protected override void UnExecuteCore()
        {
            _property.SetValue(_parentObject, _oldValue, null);
        }

        ///// <summary>
        ///// Subsequent changes of the same property on the same object are consolidated into one action
        ///// </summary>
        ///// <param name="followingAction">Subsequent action that is being recorded</param>
        ///// <returns>true if it agreed to merge with the next action, 
        ///// false if the next action should be recorded separately</returns>
        //public override bool TryToMerge(IAction followingAction)
        //{
        //    SetPropertyAction next = followingAction as SetPropertyAction;
        //    if (next != null && next.ParentObject == this.ParentObject && next.Property == this.Property)
        //    {
        //        Value = next.Value;
        //        Property.SetValue(ParentObject, Value, null);
        //        return true;
        //    }
        //    return false;
        //}
    }
}
