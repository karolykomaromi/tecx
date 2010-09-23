using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TecX.Common;

namespace TecX.TestTools
{
    public static class NotifyOnEvent
    {
        public static NotifyExpectation<T> AfterCalling<T>(this T owner, Action<T> action)
        {
            Guard.AssertNotNull(owner, "owner");
            Guard.AssertNotNull(action, "action");

            return new NotifyExpectation<T>(owner, action);
        }
    }
}
