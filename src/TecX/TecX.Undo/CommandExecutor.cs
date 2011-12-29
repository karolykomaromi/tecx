using System;

namespace TecX.Undo
{
    using System.Collections.Generic;
    using System.Linq;

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

        public void Execute(Action<CommandConfiguration> action)
        {
            Guard.AssertNotNull(action, "action");

            var config = new CommandConfiguration();

            action(config);

            this.executedCommands.Add(config);

            config.Execute();

            this.undoneCommands.Clear();

            this.CanUndoRedoChanged(this, EventArgs.Empty);
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
