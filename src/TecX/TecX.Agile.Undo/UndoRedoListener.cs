using TecX.Agile.Infrastructure.Events;
using TecX.Common;
using TecX.Common.Event;
using TecX.Undo;

namespace TecX.Agile.ViewModel.Undo
{
    public class UndoRedoListener :
        ISubscribeTo<PropertyUpdated>
    {
        #region Fields

        private readonly IActionManager _actionManager;

        #endregion Fields

        #region c'tor

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedoListener"/> class
        /// </summary>
        public UndoRedoListener(IActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actionManager = actionManager;
        }

        #endregion c'tor

        #region Event Subscriptions

        public void Handle(PropertyUpdated message)
        {
            Guard.AssertNotNull(message, "message");
            Guard.AssertNotEmpty(message.PropertyName, "message.PropertyName");

            SetPropertyAction running = _actionManager.CurrentAction as SetPropertyAction;

            if (running != null &&
                //TODO weberse use equals with null checks -> helper?
                running.ParentObject.Id == message.ArtefactId &&
                running.Property.Name == message.PropertyName)
            {
                if (AreEqual(running.NewValue, message.NewValue) &&
                    AreEqual(running.OldValue, message.OldValue))
                {
                    //this is either a rebound of an already executed action
                    //or a redo -> dont create another action
                    return;
                }

                if (AreEqual(running.NewValue, message.OldValue) &&
                    AreEqual(running.OldValue, message.NewValue))
                {
                    //this is a redo -> dont create another action
                    return;
                }
            }

            //trying to record an action while another action is running would result in an exception
            //so we dont have to die trying
            if (!_actionManager.IsActionExecuting)
            {
                //TODO weberse implement set action

                //SetPropertyAction action = new SetPropertyAction(message.ParentObject,
                //   message.PropertyName,
                //   message.OldValue,
                //   message.NewValue);

                //_actionManager.RecordAction(action);
            }

        }

        #endregion Event Subscriptions

        private static bool AreEqual(object first, object second)
        {
            if (first == null && second == null)
                return true;

            if (first != null && second != null)
                return first.Equals(second);

            return false;
        }
    }
}
