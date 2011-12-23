namespace TecX.Undo.Test.TestObjects
{
    using System;

    public class ThrowsOnRedo : Command
    {
        private int executeCount = 0;

        protected override void ExecuteCore()
        {
            if (this.executeCount < 1)
            {
                this.executeCount++;
            }
            else
            {
                throw new Exception();
            }
        }

        protected override void UnexecuteCore()
        {
        }
    }
}