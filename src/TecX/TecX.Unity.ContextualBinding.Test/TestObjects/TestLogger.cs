using System;

namespace TecX.Unity.ContextualBinding.Test.TestObjects
{
    class TestLogger : ILogger, IDisposable
    {
        #region IDisposable Member

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
