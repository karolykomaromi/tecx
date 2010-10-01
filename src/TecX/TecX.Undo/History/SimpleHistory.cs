using System;
using System.Collections.Generic;
using System.Collections;
using TecX.Common;

namespace TecX.Undo.History
{
    /// <summary>
    /// IActionHistory represents a recorded list of actions undertaken by user.

    /// This class implements a usual, linear action sequence. You can move back and forth
    /// changing the state of the respective document. When you move forward, you execute
    /// a respective action, when you move backward, you Undo it (UnExecute).

    /// Implemented through a double linked-list of SimpleHistoryNode objects.
    /// ====================================================================
    /// </summary>
    internal class SimpleHistory : IActionHistory
    {
        #region Fields

        private HistoryNode _currentState = new HistoryNode();

        #endregion Fields

        #region Properties

        /// <summary>
        /// "Iterator" to navigate through the sequence, "Cursor"
        /// </summary>
        public HistoryNode CurrentState
        {
            get
            {
                return _currentState;
            }
            private set
            {
                Guard.AssertNotNull(value, "CurrentState");

                _currentState = value;
            }
        }

        private HistoryNode Head { get; set; }

        public bool CanMoveForward
        {
            get
            {
                return CurrentState.NextAction != null &&
                       CurrentState.NextNode != null;
            }
        }

        public bool CanMoveBack
        {
            get
            {
                return CurrentState.PreviousAction != null &&
                       CurrentState.PreviousNode != null;
            }
        }

        /// <summary>
        /// The length of Undo buffer (total number of undoable actions)
        /// </summary>
        public int Length { get; private set; }

        #endregion Properties

        #region Events

        public event EventHandler UndoBufferChanged = delegate { };

        private void RaiseUndoBufferChanged()
        {
            if (UndoBufferChanged != null)
            {
                UndoBufferChanged(this, new EventArgs());
            }
        }

        #endregion Events

        #region c'tor

        public SimpleHistory()
        {
            Init();
        }

        #endregion c'tor

        #region Methods

        /// <summary>
        /// Adds a new action to the tail after current state. If 
        /// there exist more actions after this, they're lost (Garbage Collected).
        /// This is the only method of this class that actually modifies the linked-list.
        /// </summary>
        /// <param name="newAction">Action to be added.</param>
        /// <returns>true if action was appended, false if it was merged with the previous one</returns>
        public bool AppendAction(IAction newAction)
        {
            if (CurrentState.PreviousAction != null &&
                CurrentState.PreviousAction.TryToMerge(newAction))
            {
                RaiseUndoBufferChanged();
                return false;
            }

            CurrentState.NextAction = newAction;
            CurrentState.NextNode = new HistoryNode(newAction, CurrentState);
            return true;
        }

        /// <summary>
        /// All existing Nodes and Actions are garbage collected.
        /// </summary>
        public void Clear()
        {
            Init();
            RaiseUndoBufferChanged();
        }

        public IEnumerable<IAction> EnumUndoableActions()
        {
            HistoryNode current = Head;

            while (current != null &&
                   current != CurrentState &&
                   current.NextAction != null)
            {
                yield return current.NextAction;
                current = current.NextNode;
            }
        }

        public void MoveForward()
        {
            if (!CanMoveForward)
            {
                throw new InvalidOperationException(
                    "History.MoveForward() cannot execute because"
                    + " CanMoveForward returned false (the current state"
                    + " is the last state in the undo buffer.");
            }

            CurrentState.NextAction.Execute();
            CurrentState = CurrentState.NextNode;
            Length += 1;
            RaiseUndoBufferChanged();
        }

        public void MoveBack()
        {
            if (!CanMoveBack)
            {
                throw new InvalidOperationException(
                    "History.MoveBack() cannot execute because"
                    + " CanMoveBack returned false (the current state"
                    + " is the last state in the undo buffer.");
            }

            CurrentState.PreviousAction.UnExecute();
            CurrentState = CurrentState.PreviousNode;
            Length -= 1;
            RaiseUndoBufferChanged();
        }

        private void Init()
        {
            CurrentState = new HistoryNode();
            Head = CurrentState;
        }

        #endregion Methods

        #region Implementation of IEnumerable<IAction>

        public IEnumerator<IAction> GetEnumerator()
        {
            return EnumUndoableActions().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Implementation of IEnumerable<IAction>
    }
}
