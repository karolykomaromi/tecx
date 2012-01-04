using System;

namespace TecX.Undo
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using TecX.Common;

    public class CommandExecutor
    {
        private readonly List<CommandConfiguration> executedCommands;

        private readonly List<CommandConfiguration> undoneCommands;

        public bool CanUndo
        {
            get
            {
                return this.executedCommands.Count > 0;
            }
        }

        public bool CanRedo
        {
            get
            {
                return this.undoneCommands.Count > 0;
            }
        }

        public CommandExecutor()
        {
            this.executedCommands = new List<CommandConfiguration>();
            this.undoneCommands = new List<CommandConfiguration>();
        }

        public event EventHandler CanUndoRedoChanged = delegate { };

        public void Execute<TCommand>(Action<CommandConfiguration<TCommand>> action)
            where TCommand : Command
        {
            Guard.AssertNotNull(action, "action");

            var config = new CommandConfiguration<TCommand>();

            action(config);

            config.Execute();

            this.executedCommands.Add(config);

            this.undoneCommands.Clear();

            this.CanUndoRedoChanged(this, EventArgs.Empty);
        }

        public CancellationTokenSource ExecuteInBackground<TCommand>(Action<CommandConfiguration<TCommand>> action)
            where TCommand : Command
        {
            Guard.AssertNotNull(action, "action");

            var config = new CommandConfiguration<TCommand>();

            action(config);

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task.Factory.StartNew(config.Execute, token);

            return tokenSource;
        }

        public void Abort()
        {
        }

        public void Undo()
        {
            if (this.executedCommands.Count == 0)
            {
                return;
            }

            int index = this.executedCommands.Count - 1;
            var cmd = this.executedCommands[index];
            this.executedCommands.RemoveAt(index);
            this.undoneCommands.Add(cmd);
            cmd.Unexecute();

            this.CanUndoRedoChanged(this, EventArgs.Empty);
        }

        public void Redo()
        {
            if (this.undoneCommands.Count == 0)
            {
                return;
            }

            int index = this.undoneCommands.Count - 1;
            var cmd = this.undoneCommands[index];
            this.undoneCommands.RemoveAt(index);
            cmd.Execute();
            this.executedCommands.Add(cmd);
        }
    }
}
