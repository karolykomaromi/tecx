using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity.InterceptionExtension;

namespace TecX.Unity.Test.TestObjects
{
    public class IncrementIntegerByOneBehavior : IInterceptionBehavior
    {
        public int counter = 0;

        #region IInterceptionBehavior Member

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            for (int i = 0; i < input.Inputs.Count; i++)
            {
                object parameter = input.Inputs[i];

                if (parameter != null &&
                    parameter.GetType() == typeof(int))
                {
                    input.Inputs[i] = ((int)parameter) + 1;
                }
            }

            IMethodReturn methodReturn = getNext().Invoke(input, getNext);

            return methodReturn;
        }

        public bool WillExecute
        {
            get { return true; }
        }

        #endregion
    }
}