namespace TecX.TestTools.Data
{
    using System;

    using TecX.Common;

    public class DatabaseBuildResult : IDisposable
    {
        private readonly Action onDispose;

        public DatabaseBuildResult(Action onDispose)
        {
            Guard.AssertNotNull(onDispose, "onDispose");

            this.onDispose = onDispose;
        }

        public Exception Error { get; set; }

        public void Dispose()
        {
            this.onDispose();
        }
    }
}