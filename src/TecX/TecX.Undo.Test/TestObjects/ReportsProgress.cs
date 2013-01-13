namespace TecX.Undo.Test.TestObjects
{
    public class ReportsProgress : Command
    {
        protected override void ExecuteCore()
        {
            this.Progress(new Progress());
        }

        protected override void UnexecuteCore()
        {
            throw new System.NotImplementedException();
        }
    }
}