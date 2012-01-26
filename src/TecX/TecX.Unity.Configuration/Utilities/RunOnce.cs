namespace TecX.Unity.Configuration.Utilities
{
    using System;

    using TecX.Common;

    public class RunOnce<T>
    {
        private readonly Action<T> action;

        private bool run;

        public RunOnce(Action<T> action)
        {
            Guard.AssertNotNull(action, "action");

            this.action = action;
            this.run = false;
        }

        public static implicit operator Action<T>(RunOnce<T> runOnce)
        {
            Guard.AssertNotNull(runOnce, "runOnce");

            return runOnce.Run;
        }

        public void Run(T item)
        {
            if (this.run)
            {
                return;
            }

            this.action(item);

            this.run = true;
        }
    }
}