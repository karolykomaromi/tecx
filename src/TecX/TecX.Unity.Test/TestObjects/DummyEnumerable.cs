using System;
using System.Collections.Generic;

namespace TecX.Unity.Test.TestObjects
{
    class DummyEnumerable : IEnumerable<string>
    {
        #region IEnumerable<string> Member

        public IEnumerator<string> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
