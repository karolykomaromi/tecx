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

        public CommandExecutor()
        {
            this.executedCommands = new List<CommandConfiguration>();
            this.undoneCommands = new List<CommandConfiguration>();
        }

        public void Execute(Action<CommandConfiguration> action)
        {
            Guard.AssertNotNull(action, "action");

            var config = new CommandConfiguration();

            action(config);

            this.executedCommands.Add(config);

            config.Execute();
        }

        public void Abort()
        {
        }

        public void Undo()
        {
            int index = this.executedCommands.Count - 1;
            var cmd = this.executedCommands[index];
            this.executedCommands.RemoveAt(index);
            this.undoneCommands.Add(cmd);
            cmd.Unexecute();
        }

        public void Redo()
        {
            int index = this.undoneCommands.Count - 1;
            var cmd = this.undoneCommands[index];
            this.undoneCommands.RemoveAt(index);
            this.executedCommands.Add(cmd);
            cmd.Execute();
        }
    }
}
