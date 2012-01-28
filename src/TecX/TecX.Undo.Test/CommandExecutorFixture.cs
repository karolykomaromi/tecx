namespace TecX.Undo.Test
{
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using TecX.Undo.Test.TestObjects;

    [TestClass]
    public class CommandExecutorFixture
    {
        [TestMethod]
        public void ExecuteInBackgroundUsesDifferentThread()
        {
            var executor = new CommandExecutor();

            int testThreadId = Thread.CurrentThread.ManagedThreadId;

            executor.ExecuteInBackground<ThreadAwareCommand>(
                x => x.ConstructUsing(() => new ThreadAwareCommand(testThreadId)));
        }

        [TestMethod]
        public void CanCancelExecutionInBackground()
        {
            var executor = new CommandExecutor();

            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            var tokenSource = executor.ExecuteInBackground<LongRunningCommand>(
                x => x.OnSuccess(c => Assert.Fail("Command must not succeed.")));

            manualResetEvent.WaitOne(25);

            tokenSource.Cancel();
        }

        [TestMethod]
        public void CanExecute()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd));

            Assert.AreEqual("1", cmd.Message);
        }

        [TestMethod]
        public void CanCallBackOnSuccess()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            bool calledBack = false;

            executor.Execute<SubmitMessage>(
                x =>
                {
                    x.ConstructUsing(() => cmd);
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

            executor.Execute<AlwaysThrows>(
                x =>
                {
                    x.ConstructUsing(() => cmd);
                    x.OnFailure(info =>
                        {
                            Assert.AreSame(cmd, info.Command);
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

            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd));

            Assert.AreEqual("1", cmd.Message);

            executor.Undo();

            Assert.AreEqual("0", cmd.Message);
        }

        [TestMethod]
        public void CanRedo()
        {
            var executor = new CommandExecutor();

            var cmd = new SubmitMessage();

            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd));

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

            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd1));
            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd2));
            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd3));

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

            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd1));
            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd2));
            executor.Execute<SubmitMessage>(x => x.ConstructUsing(() => cmd3));

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
        public void CanReportProgress()
        {
            var executor = new CommandExecutor();

            bool reportedProgress = false;

            executor.Execute<ReportsProgress>(
                x => x.OnProgress(p => reportedProgress = true));

            Assert.IsTrue(reportedProgress);
        }

        [TestMethod]
        public void FailureDuringUndoDoesNotPutCmdInUndostack()
        {
            var executor = new CommandExecutor();

            try
            {
                executor.Execute<ThrowsOnUndo>(x => { });
            }
            catch
            {
            }

            Assert.IsFalse(executor.CanRedo);
        }

        [TestMethod]
        public void FailureDuringRedoDoesNotPutCmdInRedostack()
        {
            var executor = new CommandExecutor();
            executor.Execute<ThrowsOnRedo>(x => { });
            executor.Undo();
            try
            {
                executor.Redo();
            }
            catch
            {
            }

            Assert.IsFalse(executor.CanUndo);
        }

        [TestMethod]
        public void ComplexUndoRedoSequence()
        {
            var complex1 = new CountsUndoRedo();
            var complex2 = new CountsUndoRedo();
            var complex3 = new CountsUndoRedo();

            var executor = new CommandExecutor();

            executor.Execute<CountsUndoRedo>(x => x.ConstructUsing(() => complex1));
            executor.Execute<CountsUndoRedo>(x => x.ConstructUsing(() => complex2));
            executor.Undo();
            executor.Execute<CountsUndoRedo>(x => x.ConstructUsing(() => complex3));
            executor.Redo();

            Assert.AreEqual(1, complex1.ExecuteCount);
            Assert.AreEqual(1, complex2.ExecuteCount);
            Assert.AreEqual(1, complex2.UndoCount);
            Assert.AreEqual(1, complex3.ExecuteCount);
        }

        [TestMethod]
        public void NotifiesOnCanUndoChanged()
        {
            var executor = new CommandExecutor();

            executor.CanUndoRedoChanged += (s, e) => Assert.IsTrue(executor.CanUndo);

            executor.Execute<CountsUndoRedo>(x => { });
        }

        [TestMethod]
        public void NotifiesOnCanRedoChanged()
        {
            var executor = new CommandExecutor();

            executor.Execute<CountsUndoRedo>(x => { });

            executor.CanUndoRedoChanged += (s, e) => Assert.IsTrue(executor.CanRedo);

            executor.Undo();
        }
    }
}
