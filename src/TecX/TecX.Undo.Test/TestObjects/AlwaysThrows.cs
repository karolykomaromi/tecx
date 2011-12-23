namespace TecX.Undo.Test.TestObjects
{
    using System;

    public class AlwaysThrows : Command
    {
        protected override void ExecuteCore()
        {
            throw new NotImplementedException();
        }

        protected override void UnexecuteCore()
        {
            throw new NotImplementedException();
        }
    }
}