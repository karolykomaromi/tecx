namespace TecX.Undo.Test.TestObjects
{
    public class LogAction : AbstractAction
    {
        public int ExecutesCount { get; set; }
        public int UnexecutesCount { get; set; }

        protected override void ExecuteCore()
        {
            ExecuteCount++;
        }

        protected override void UnExecuteCore()
        {
            UnexecutesCount++;
        }
    }
}