namespace TecX.Undo.Test.TestObjects
{
    public class FlagAction : AbstractAction
    {
        public bool Executed { get; set; }

        protected override void ExecuteCore()
        {
            Executed = true;
        }

        protected override void UnExecuteCore()
        {
            Executed = false;
        }
    }
}