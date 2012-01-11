namespace TecX.Undo
{
    using System;

    using TecX.Common;

    public abstract class Command
    {
        private int executeCount;

        private Action<ProgressInfo> progress;

        protected Command()
        {
            this.executeCount = 0;
            this.Progress = info => { };
        }

        public Action<ProgressInfo> Progress
        {
            get
            {
                return this.progress;
            }
            set
            {
                Guard.AssertNotNull(value, "Progress");

                this.progress = value;
            }
        }

        public virtual void Execute()
        {
            if (this.CanExecute())
            {
                this.ExecuteCore();
                this.executeCount++;
            }
        }

        public virtual void Unexecute()
        {
            if (this.CanUnexecute())
            {
                this.UnexecuteCore();
                this.executeCount--;
            }
        }

        protected virtual bool CanExecute()
        {
            return this.executeCount == 0;
        }

        protected virtual bool CanUnexecute()
        {
            return this.executeCount > 0;
        }

        protected abstract void ExecuteCore();

        protected abstract void UnexecuteCore();
    }

    public class ProgressInfo
    {
    }
}