namespace TecX.Undo
{
    using TecX.Common;

    public class Success
    {
        private readonly Command command;

        public Success(Command command)
        {
            Guard.AssertNotNull(command, "command");
            this.command = command;
        }

        public Command Command
        {
            get
            {
                return this.command;
            }
        }
    }
}