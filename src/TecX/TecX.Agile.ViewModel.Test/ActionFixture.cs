using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.ViewModel.Undo;
using TecX.Undo;

namespace TecX.Agile.ViewModel.Test
{
    [TestClass]
    public class ActionFixture
    {
        [TestMethod]
        public void Transactions()
        {
            var instance = new Exception();
            //var instance = new Exception { Source = "green" };
            IActionManager am = new ActionManager();

            SetPropertyAction action = new SetPropertyAction(instance, "Source", "green", "blue");

            am.RecordAction(action);

            Assert.AreEqual("blue", instance.Source);
            am.Undo();
            Assert.AreEqual("green", instance.Source);

            using (Transaction.Create(am))
            {
                action = new SetPropertyAction(instance, "Source", "green", "red");
                am.RecordAction(action);
                Assert.AreEqual("green", instance.Source);
            }
            Assert.AreEqual(instance.Source, "red");
            am.Undo();
            Assert.AreEqual("green", instance.Source);
            am.Redo();
            Assert.AreEqual(instance.Source, "red");
        }
    }
}
