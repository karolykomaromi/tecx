using System;

namespace TecX.Unity.Test.TestObjects
{
    class Logger : IFormattable, ILogger
    {
        #region IFormattable Member

        public string ToString(string format, IFormatProvider formatProvider)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
