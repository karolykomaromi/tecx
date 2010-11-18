using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.View.Test.TestObjects;
using TecX.Agile.ViewModel;
using TecX.Agile.ViewModel.Remote;

namespace TecX.Agile.View.Test
{
    [TestClass]
    public class HighlightingFixture
    {
        [TestMethod]
        public void WhenRaisingHighlightRequestedEventOnEventHub_TextBoxIsFocued()
        {
            Guid id = Guid.NewGuid();

            ViewModel.StoryCard card = new ViewModel.StoryCard { Id = id };

            TestUserControl ctrl = new TestUserControl(card);

            RemoteHighlight.RaiseIncomingRequestToHighlightField(this, new RemoteHighlightEventArgs(id, "Txt"));

            Assert.IsTrue(ctrl.Txt.IsFocused);
        }

        [TestMethod]
        public void WhenTextBoxGetsFocus_EventHubRaisesFieldHighlightedEvent()
        {
            Guid id = Guid.NewGuid();

            ViewModel.StoryCard card = new ViewModel.StoryCard { Id = id };

            TestUserControl ctrl = new TestUserControl(card);

            bool notified = false;

            Action<object, RemoteHighlightEventArgs> action = (s, e) =>
                                                            {
                                                                Assert.AreEqual(id, e.ArtefactId);
                                                                Assert.AreEqual("Txt", e.FieldName);
                                                                notified = true;
                                                            };

            EventHandler<RemoteHighlightEventArgs> handler = new EventHandler<RemoteHighlightEventArgs>(action);

            RemoteHighlight.OutgoingNotificationThatFieldWasHighlighted += handler;

            ctrl.Txt.Focus();

            Assert.IsTrue(notified);

            RemoteHighlight.OutgoingNotificationThatFieldWasHighlighted -= handler;
        }
    }
}
