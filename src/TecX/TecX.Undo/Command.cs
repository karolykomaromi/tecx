namespace TecX.Undo
{
    public abstract class Command
    {
        private int executeCount;

        protected Command()
        {
            this.executeCount = 0;
        }

        public virtual void Execute()
        {
            if(this.CanExecute())
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
}