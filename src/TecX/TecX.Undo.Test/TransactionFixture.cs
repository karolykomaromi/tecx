using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TecX.Undo.Actions;
using TecX.Undo.Test.TestObjects;

namespace TecX.Undo.Test
{
    [TestClass]
    public class TransactionFixture
    {
        public TestContext TestContext { get; set; }



        [TestMethod]
        public void ThrowingActionInsideTransactionWillRollback()
        {
            ActionManager am = new ActionManager();
            var log = new LogAction();
            var throwing = new ThrowingAction();
            try
            {
                using (Transaction.Create(am))
                {
                    am.RecordAction(log);
                    am.RecordAction(throwing);
                }
            }
            catch (NotImplementedException)
            {
            }
            Assert.AreEqual(0, log.ExecutesCount);
            Assert.AreEqual(0, log.UnexecutesCount);

            Assert.AreEqual(0, am.TransactionStack.Count);
            Assert.AreEqual(false, am.IsActionExecuting);
        }
    }
}
