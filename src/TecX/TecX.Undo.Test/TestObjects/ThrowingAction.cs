using System;

namespace TecX.Undo.Test.TestObjects
{
    public class ThrowingAction : AbstractAction
    {
        protected override void ExecuteCore()
        {
            throw new NotImplementedException();
        }

        protected override void UnExecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}