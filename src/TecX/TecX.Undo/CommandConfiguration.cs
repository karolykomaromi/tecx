namespace TecX.Undo
{
    using System;
    using System.Windows.Threading;

    using TecX.Common;

    public class CommandConfiguration
    {
        private readonly Dispatcher dispatcher;

        private Command command;

        private Action<Command> handleSuccess;

        private Action<Command, Exception> handleFailure;

        private bool callbackToUI;

        public CommandConfiguration()
        {
            this.handleSuccess = cmd => { };
            this.handleFailure = (cmd, ex) => { };
        }

        public void Execute()
        {
            try
            {
                this.command.Execute();

                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(DispatcherPriority.Normal, 
                        new Action(() => this.handleSuccess(this.command)));
                }
                else
                {
                    this.handleSuccess(this.command);
                }
            }
            catch (Exception ex)
            {
                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(DispatcherPriority.Normal, 
                        new Action(() => this.handleFailure(this.command, ex)));
                }
                else
                {
                    this.handleFailure(this.command, ex);
                }
            }
        }

        public void Command(Command command)
        {
            Guard.AssertNotNull(command, "command");

            this.command = command;
        }

        public void CallbackToUI()
        {
            this.callbackToUI = true;
        }

        public void OnSuccess(Action<Command> handleSuccess)
        {
            Guard.AssertNotNull(handleSuccess, "handleSuccess");

            this.handleSuccess = handleSuccess;
        }

        public void OnFailure(Action<Command, Exception> handleFailure)
        {
            Guard.AssertNotNull(handleFailure, "handleFailure");

            this.handleFailure = handleFailure;
        }

        public void Unexecute()
        {
            try
            {
                this.command.Unexecute();

                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => this.handleSuccess(this.command)));
                }
                else
                {
                    this.handleSuccess(this.command);
                }
            }
            catch (Exception ex)
            {
                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(DispatcherPriority.Normal,
                        new Action(() => this.handleFailure(this.command, ex)));
                }
                else
                {
                    this.handleFailure(this.command, ex);
                }
            }
        }
    }
}