namespace TecX.Undo.Test.TestObjects
{
    using System;

    public class ThrowsOnUndo : Command
    {
        protected override void ExecuteCore()
        {
        }

        protected override void UnexecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}