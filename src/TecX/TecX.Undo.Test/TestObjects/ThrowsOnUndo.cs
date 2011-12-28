namespace TecX.Undo.Test.TestObjects
{
    using System;

    public class ThrowsOnUndo : CountsUndoRedo
    {
        protected override void UnexecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}