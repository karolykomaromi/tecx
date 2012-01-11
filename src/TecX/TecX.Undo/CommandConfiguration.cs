namespace TecX.Undo
{
    using System;
    using System.Windows.Threading;

    using TecX.Common;

    public class CommandConfiguration<TCommand> : CommandConfiguration
        where TCommand : Command
    {
        private readonly Dispatcher dispatcher;

        private TCommand command;

        private Func<TCommand> commandFactory;

        private Action<TCommand> handleSuccess;

        private Action<TCommand, Exception> handleFailure;

        private bool callbackToUI;

        private Action<ProgressInfo> handleProgress;

        public CommandConfiguration()
        {
            this.handleSuccess = cmd => { };
            this.handleFailure = (cmd, ex) => { };
            this.commandFactory = () => (TCommand)Activator.CreateInstance(typeof(TCommand));
        }

        public TCommand Command
        {
            get
            {
                if (this.command == null)
                {
                    this.command = this.commandFactory();
                }

                return this.command;
            }
        }

        public override void Execute()
        {
            try
            {
                if (this.callbackToUI)
                {
                    this.command.Progress = info => this.dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => this.handleProgress(info)));
                }
                else
                {
                    this.command.Progress = this.handleProgress;
                }

                this.Command.Progress = this.handleProgress;
                this.Command.Execute();

                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(() => this.handleSuccess(this.Command)));
                }
                else
                {
                    this.handleSuccess(this.Command);
                }
            }
            catch (Exception ex)
            {
                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(() => this.handleFailure(this.Command, ex)));
                }
                else
                {
                    this.handleFailure(this.Command, ex);
                }
            }
        }

        public void ConstructUsing(Func<TCommand> commandFactory)
        {
            Guard.AssertNotNull(commandFactory, "commandFactory");

            this.commandFactory = commandFactory;
        }

        public void CallbackToUI()
        {
            this.callbackToUI = true;
        }

        public void OnSuccess(Action<TCommand> handleSuccess)
        {
            Guard.AssertNotNull(handleSuccess, "handleSuccess");

            this.handleSuccess = handleSuccess;
        }

        public void OnFailure(Action<TCommand, Exception> handleFailure)
        {
            Guard.AssertNotNull(handleFailure, "handleFailure");

            this.handleFailure = handleFailure;
        }

        public void OnProgress(Action<ProgressInfo> handleProgress)
        {
            Guard.AssertNotNull(handleProgress, "handleProgress");

            this.handleProgress = handleProgress;
        }

        public override void Unexecute()
        {
            try
            {
                //TODO weberse 2012-01-03 should progress of undo also be reported?
                this.Command.Unexecute();

                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(() => this.handleSuccess(this.Command)));
                }
                else
                {
                    this.handleSuccess(this.Command);
                }
            }
            catch (Exception ex)
            {
                if (this.callbackToUI)
                {
                    this.dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Action(() => this.handleFailure(this.Command, ex)));
                }
                else
                {
                    this.handleFailure(this.Command, ex);
                }
            }
        }
    }

    public abstract class CommandConfiguration
    {
        public abstract void Execute();

        public abstract void Unexecute();
    }
}