using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Undo.Actions;

namespace TecX.Undo.Test
{
    [TestClass]
    public class ActionsFixture
    {
        [TestMethod]
        public void AddItemActionWorks()
        {
            List<string> list = new List<string>();
            AddItemAction<string> action = new AddItemAction<string>(list.Add, s => list.Remove(s), "foo");
            IActionManager am = new ActionManager();
            am.RecordAction(action);
            Assert.AreEqual("foo", list[0]);
            am.Undo();
            Assert.AreEqual(0, list.Count);
            am.Redo();
            Assert.AreEqual("foo", list[0]);
        }

        [TestMethod]
        public void CallMethodActionWorks()
        {
            bool capturedFlag = false;
            IActionManager am = new ActionManager();
            CallMethodAction action = new CallMethodAction(
                () => capturedFlag = true,
                () => capturedFlag = false);
            am.RecordAction(action);
            Assert.IsTrue(capturedFlag);
            am.Undo();
            Assert.IsFalse(capturedFlag);
            am.Redo();
            Assert.IsTrue(capturedFlag);
        }
    }
}
