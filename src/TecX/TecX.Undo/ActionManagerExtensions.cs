using System.Collections.Generic;

using TecX.Common;
using TecX.Undo.Actions;

namespace TecX.Undo
{
    public static class Extensions
    {
        public static Transaction CreateTransaction(this ActionManager actionManager)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            return Transaction.Create(actionManager);
        }

        public static Transaction CreateTransaction(this ActionManager actionManager, bool delayed)
        {
            Guard.AssertNotNull(actionManager, "actionManager");

            return Transaction.Create(actionManager, delayed);
        }
    }
}
