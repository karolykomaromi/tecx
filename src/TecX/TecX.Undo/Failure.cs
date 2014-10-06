namespace TecX.Undo
{
    using System;

    using TecX.Common;

    public class Failure
    {
        private readonly Command command;

        private readonly Exception error;

        public Failure(Command command, Exception error)
        {
            Guard.AssertNotNull(command, "command");
            Guard.AssertNotNull(error, "error");

            this.command = command;
            this.error = error;
        }

        public Exception Error
        {
            get
            {
                return this.error;
            }
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