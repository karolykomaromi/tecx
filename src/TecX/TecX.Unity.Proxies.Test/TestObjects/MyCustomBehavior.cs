namespace TecX.Unity.Proxies.Test.TestObjects
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity.InterceptionExtension;

    public class MyCustomBehavior : IInterceptionBehavior
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            return input.CreateMethodReturn("Foo()");
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public bool WillExecute
        {
            get
            {
                return true;
            }
        }
    }
}