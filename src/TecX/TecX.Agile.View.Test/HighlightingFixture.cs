using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.View.Test.TestObjects;
using TecX.Agile.ViewModel;

namespace TecX.Agile.View.Test
{
    [TestClass]
    public class HighlightingFixture
    {
        [TestMethod]
        public void WhenTellingViewModelToHighlightTextBox_TextBoxIsFocused()
        {
            Guid id = Guid.NewGuid();

            ViewModel.StoryCard card = new ViewModel.StoryCard { Id = id };

            TestUserControl ctrl = new TestUserControl(card);

            EventSource.RaiseHighlightField(this, new HighlightEventArgs(id, "Txt"));

            Assert.IsTrue(ctrl.Txt.IsFocused);
        }

        [TestMethod]
        public void WhenFocussingTextBox_ViewModelIsNotified()
        {
            Guid id = Guid.NewGuid();

            ViewModel.StoryCard card = new ViewModel.StoryCard { Id = id };

            TestUserControl ctrl = new TestUserControl(card);

            bool notified = false;

            Action<object, HighlightEventArgs> action = (s, e) =>
                                                            {
                                                                Assert.AreEqual(id, e.Id);
                                                                Assert.AreEqual("Txt", e.FieldName);
                                                                notified = true;
                                                            };

            EventHandler<HighlightEventArgs> handler = new EventHandler<HighlightEventArgs>(action);

            EventSource.FieldHighlighted += handler;

            ctrl.Txt.Focus();

            Assert.IsTrue(notified);

            EventSource.FieldHighlighted -= handler;
        }
    }
}
