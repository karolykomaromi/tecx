using System;

using TecX.Common;

namespace TecX.Unity.Configuration.Conventions
{
    public class RunOnce<T>
    {
        private bool _run;

        private readonly Action<T> _action;

        public RunOnce(Action<T> action)
        {
            Guard.AssertNotNull(action, "action");

            _action = action;
            _run = false;
        }

        public void Run(T item)
        {
            if (_run)
            {
                return;
            }

            _action(item);

            _run = true;
        }

        public static implicit operator Action<T>(RunOnce<T> runOnce)
        {
            Guard.AssertNotNull(runOnce, "runOnce");

            return runOnce.Run;
        }
    }
}