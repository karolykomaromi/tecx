namespace TecX.Undo.Test.TestObjects
{
    using System;

    public class ThrowsOnRedo : CountsUndoRedo
    {
        protected override void ExecuteCore()
        {
            base.ExecuteCore();

            if (this.ExecuteCount >= 1)
            {
                throw new Exception();
            }
        }
    }
}