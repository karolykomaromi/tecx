using System;
using System.Collections.Generic;
using System.Linq;

using TecX.Common;

namespace TecX.Undo
{
    public sealed class Transaction : IAction, IDisposable
    {
        #region Fields

        readonly List<IAction> _actions;
        readonly IActionManager _actionManager;

        #endregion Fields

        #region Properties

        bool Aborted { get; set; }

        public bool AllowToMergeWithPrevious { get; set; }
        public bool IsDelayed { get; set; }

        #endregion Properties

        #region c'tor

        private Transaction(IActionManager actionManager, bool delayed)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            _actions = new List<IAction>();
            _actionManager = actionManager;

            actionManager.BeginTransaction(this);
            IsDelayed = delayed;
        }

        public static Transaction Create(IActionManager actionManager, bool delayed)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            return new Transaction(actionManager, delayed);
        }

        /// <summary>
        /// By default, the actions are delayed and executed only after
        /// the top-level transaction commits.
        /// </summary>
        /// <remarks>
        /// Make sure to dispose of the transaction once you're done - it will actually call Commit for you
        /// </remarks>
        /// <example>
        /// Recommended usage: using (Transaction.Create(actionManager)) { DoStuff(); }
        /// </example>
        public static Transaction Create(IActionManager actionManager)
        {
            return Create(actionManager, true);
        }

        #endregion c'tor

        #region Implementation of IAction

        public void Execute()
        {
            if (!IsDelayed)
            {
                IsDelayed = true;
                return;
            }
            foreach (var action in _actions)
            {
                action.Execute();
            }
        }

        public void UnExecute()
        {
            foreach (var action in Enumerable.Reverse(_actions))
            {
                action.UnExecute();
            }
        }

        public bool CanExecute()
        {
            foreach (var action in _actions)
            {
                if (!action.CanExecute())
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanUnExecute()
        {
            foreach (var action in Enumerable.Reverse(_actions))
            {
                if (!action.CanUnExecute())
                {
                    return false;
                }
            }
            return true;
        }

        public bool TryToMerge(IAction followingAction)
        {
            return false;
        }

        #endregion Implementation of IAction

        #region Methods

        public void Commit()
        {
            _actionManager.CommitTransaction();
        }

        public void Rollback()
        {
            _actionManager.RollBackTransaction();

            Aborted = true;
        }

        public void Dispose()
        {
            if (!Aborted)
            {
                Commit();
            }
        }

        public void Add(IAction actionToAppend)
        {
            Guard.AssertNotNull(actionToAppend, "actionToAppend");

            _actions.Add(actionToAppend);
        }

        public bool HasActions()
        {
            return _actions.Count != 0;
        }

        public void Remove(IAction actionToCancel)
        {
            Guard.AssertNotNull(actionToCancel, "actionToCancel");

            _actions.Remove(actionToCancel);
        }

        #endregion Methods
    }
}
