namespace TecX.Undo.Test.TestObjects
{
    public class CountsUndoRedo : Command
    {
        public int UndoCount { get; set; }

        public int ExecuteCount { get; set; }

        protected override void ExecuteCore()
        {
            this.ExecuteCount++;
        }

        protected override void UnexecuteCore()
        {
            this.UndoCount++;
        }
    }
}
