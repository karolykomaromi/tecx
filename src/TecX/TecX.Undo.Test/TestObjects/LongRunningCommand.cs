namespace TecX.Undo.Test.TestObjects
{
    using System.Threading;

    public class LongRunningCommand : Command
    {
        protected override void ExecuteCore()
        {
            Thread.Sleep(50);
            Thread.Sleep(50);
        }

        protected override void UnexecuteCore()
        {
            throw new System.NotImplementedException();
        }
    }
}