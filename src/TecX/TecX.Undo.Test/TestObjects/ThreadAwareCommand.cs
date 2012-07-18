namespace TecX.Undo.Test.TestObjects
{
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class ThreadAwareCommand : Command
    {
        private readonly int testThreadId;

        public ThreadAwareCommand(int testThreadId)
        {
            this.testThreadId = testThreadId;
        }

        protected override void ExecuteCore()
        {
            int backgroundThreadId = Thread.CurrentThread.ManagedThreadId;

            Assert.AreNotEqual(this.testThreadId, backgroundThreadId);
        }

        protected override void UnexecuteCore()
        {
            throw new System.NotImplementedException();
        }
    }
}