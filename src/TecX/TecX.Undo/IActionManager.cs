namespace TecX.Undo
{
    public interface IActionManager
    {
        /// <summary>
        /// Currently running action (during an Undo or Redo process)
        /// </summary>
        /// <remarks>null if no Undo or Redo is taking place</remarks>
        IAction CurrentAction { get; }

        bool CanUndo { get; }
        bool CanRedo { get; }

        void BeginTransaction(Transaction t);
        void CommitTransaction();
        void RollBackTransaction();

        void Clear();

        /// <summary>
        /// Central method to add and execute a new action.
        /// </summary>
        /// <param name="action">An action to be recorded in the buffer and executed</param>
        void RecordAction(IAction action);

        void Undo();
        void Redo();
    }
}