namespace TecX.Undo
{
    using System;
    using System.Windows.Threading;

    using TecX.Common;

    public class CommandConfiguration<TCommand> : CommandConfiguration
        where TCommand : Command
    {
        private Action<Action> executor;

        private TCommand command;

        private Func<TCommand> commandFactory;

        private Action<TCommand> handleSuccess;

        private Action<TCommand, Exception> handleFailure;

        private Action<ProgressInfo> handleProgress;

        public CommandConfiguration()
        {
            this.handleSuccess = cmd => { };
            this.handleFailure = (cmd, ex) => { };
            this.handleProgress = progressInfo => { };
            this.commandFactory = () => (TCommand)Activator.CreateInstance(typeof(TCommand));

            this.executor = action => action();
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
                this.executor(() => this.handleProgress(new ProgressInfo()));

                this.Command.Execute();

                this.executor(() => this.handleSuccess(this.Command));
            }
            catch (Exception ex)
            {
                this.executor(() => this.handleFailure(this.Command, ex));
            }
        }

        public void ConstructUsing(Func<TCommand> commandFactory)
        {
            Guard.AssertNotNull(commandFactory, "commandFactory");

            this.commandFactory = commandFactory;
        }

        public void CallbackToUI()
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;

            this.executor = delegate(Action action)
            {
                if (dispatcher.CheckAccess())
                {
                    action();
                }
                else
                {
                    dispatcher.Invoke(action, new object[0]);
                }
            };
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

                this.executor(() => this.handleSuccess(this.Command));
            }
            catch (Exception ex)
            {
                this.executor(() => this.handleFailure(this.Command, ex));
            }
        }
    }

    public abstract class CommandConfiguration
    {
        public abstract void Execute();

        public abstract void Unexecute();
    }
}