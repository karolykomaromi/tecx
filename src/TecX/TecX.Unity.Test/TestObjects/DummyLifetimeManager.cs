using System;

using Microsoft.Practices.Unity;

namespace TecX.Unity.Test.TestObjects
{
    class DummyLifetimeManager : LifetimeManager
    {
        public override object GetValue()
        {
            throw new NotImplementedException();
        }

        public override void RemoveValue()
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object newValue)
        {
            throw new NotImplementedException();
        }
    }
}
