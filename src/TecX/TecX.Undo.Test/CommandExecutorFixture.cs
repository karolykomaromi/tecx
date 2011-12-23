namespace TecX.Undo.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Undo.Test.TestObjects;

    [TestClass]
    public class CommandExecutorFixture
    {
        [TestMethod]
        public void CanExecute()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            executor.Execute(
                x =>
                    {
                        x.Command(cmd);
                        //x.CallbackToUI();
                        ////Uses the SynchronizationContext around the thread that owns the message pump for callbacks.
                        //x.OnSuccess(msg => { });
                        //x.OnFailure((msg, ex) => { });
                    });

            Assert.AreEqual("1", cmd.Message);
        }

        [TestMethod]
        public void CanCallBackOnSuccess()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            bool calledBack = false;

            executor.Execute(
                x =>
                {
                    x.Command(cmd);
                    x.OnSuccess(msg => { calledBack = true; });
                });
            
            Assert.AreEqual("1", cmd.Message);
            Assert.IsTrue(calledBack);
        }

        [TestMethod]
        public void CanCallBackOnFailure()
        {
            var executor = new CommandExecutor();

            var cmd = new AlwaysThrows();

            bool calledBack = false;

            executor.Execute(
                x =>
                {
                    x.Command(cmd);
                    x.OnFailure((c, ex) =>
                        {
                            Assert.AreSame(cmd, c);
                            calledBack = true;
                        });
                });
            Assert.IsTrue(calledBack);
        }

        [TestMethod]
        public void CanUndo()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            executor.Execute(x => x.Command(cmd));

            Assert.AreEqual("1", cmd.Message);
            
            executor.Undo();

            Assert.AreEqual("0", cmd.Message);
        }

        [TestMethod]
        public void CanRedo()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            executor.Execute(x => x.Command(cmd));

            Assert.AreEqual("1", cmd.Message);

            executor.Undo();

            Assert.AreEqual("0", cmd.Message);

            executor.Redo();

            Assert.AreEqual("1", cmd.Message);
        }

        [TestMethod]
        public void CanUndoMultipleCommands()
        {
            var executor = new CommandExecutor();

            var cmd1 = new SubmitMessage();
            var cmd2 = new SubmitMessage();
            var cmd3 = new SubmitMessage();

            executor.Execute(x => x.Command(cmd1));
            executor.Execute(x => x.Command(cmd2));
            executor.Execute(x => x.Command(cmd3));

            Assert.AreEqual("1", cmd1.Message);
            Assert.AreEqual("1", cmd2.Message);
            Assert.AreEqual("1", cmd3.Message);

            executor.Undo();
            executor.Undo();
            executor.Undo();

            Assert.AreEqual("0", cmd1.Message);
            Assert.AreEqual("0", cmd2.Message);
            Assert.AreEqual("0", cmd3.Message);
        }

        [TestMethod]
        public void CanRedoMultipleCommands()
        {
            var executor = new CommandExecutor();

            var cmd1 = new SubmitMessage();
            var cmd2 = new SubmitMessage();
            var cmd3 = new SubmitMessage();

            executor.Execute(x => x.Command(cmd1));
            executor.Execute(x => x.Command(cmd2));
            executor.Execute(x => x.Command(cmd3));

            Assert.AreEqual("1", cmd1.Message);
            Assert.AreEqual("1", cmd2.Message);
            Assert.AreEqual("1", cmd3.Message);

            executor.Undo();
            executor.Undo();
            executor.Undo();

            Assert.AreEqual("0", cmd1.Message);
            Assert.AreEqual("0", cmd2.Message);
            Assert.AreEqual("0", cmd3.Message);

            executor.Redo();
            executor.Redo();
            executor.Redo();

            Assert.AreEqual("1", cmd1.Message);
            Assert.AreEqual("1", cmd2.Message);
            Assert.AreEqual("1", cmd3.Message);
        }

        [TestMethod]
        public void FailureDuringUndoDoesNotPutCmdInUndostack()
        {
            Assert.Fail("implement");
        }

        [TestMethod]
        public void FailureDuringRedoDoesNotPutCmdInRedostack()
        {
            Assert.Fail("implement");
        }
    }
}
