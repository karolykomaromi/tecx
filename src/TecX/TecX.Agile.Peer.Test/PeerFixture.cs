using System;
using System.Text;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Remote;
using TecX.Agile.ViewModel;

namespace TecX.Agile.Peer.Test
{
    [TestClass]
    public class PeerFixture
    {
        [TestMethod]
        public void WhenMovingStoryCard_PeersReceiveNotification()
        {
            try
            {
                bool messageReceived = false;

                using (new PeerServiceHost())
                {
                    PeerClient peer1 = new PeerClient();

                    PeerClient peer2 = new PeerClient();

                    peer1.StoryCardMoved += (s, e) => Assert.Fail("message must not bounce back");

                    peer2.StoryCardMoved += (s, e) =>
                                                {
                                                    messageReceived = true;
                                                };

                    peer1.MoveStoryCard(peer1.Id, Guid.NewGuid(), 1.2, 2.3, 3.4);

                }

                Assert.IsTrue(messageReceived);
            }
            catch (PnrpNotAvailableException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }

        [TestMethod]
        public void WhenHighlightingField_PeersReceiveNotification()
        {
            try
            {
                bool messageReceived = false;

                using (new PeerServiceHost())
                {
                    PeerClient peer1 = new PeerClient();

                    PeerClient peer2 = new PeerClient();

                    peer1.IncomingRequestToHighlightField += (s, e) => Assert.Fail("message must not bounce back");

                    peer2.IncomingRequestToHighlightField += (s, e) =>
                    {
                        messageReceived = true;
                    };

                    peer1.Highlight(peer1.Id, Guid.NewGuid(), "Id");

                }

                Assert.IsTrue(messageReceived);
            }
            catch (PnrpNotAvailableException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }

        [TestMethod]
        public void GivenAnInboundMessageThatWillTriggerAnOutBoundMessage_WhenCheckingWetherToSendOutboundMessage_SaysNo()
        {
            Guid artefactId = Guid.NewGuid();
            const string fieldName = "Description";

            HighlightMessageFilter filter = new HighlightMessageFilter();

            filter.Enqueue(artefactId, fieldName);

            FieldHighlighted outboundMessage = new FieldHighlighted(artefactId, fieldName);

            bool letPass = filter.ShouldLetPass(outboundMessage);

            Assert.IsFalse(letPass);
        }

        [TestMethod] 
        public void GivenAnInboundPropertyChangeMessage_WhenCheckingWetherToSendReboundMessage_SaysNo()
        {
            Guid storyCardId = Guid.NewGuid();
            StoryCard card = new StoryCard {Id = storyCardId};
            const string propertyName = "Name";
            string oldValue = null;
            string newValue = "Some name";

            PropertyChangedMessageFilter filter = new PropertyChangedMessageFilter();

            filter.Enqueue(storyCardId, propertyName, oldValue, newValue);

            PropertyUpdated outboundMessage = new PropertyUpdated(card.Id, propertyName, oldValue, newValue);

            bool letPass = filter.ShouldLetPass(outboundMessage);

            Assert.IsFalse(letPass);
        }

        [TestMethod]
        public void GivenAnInboundStoryCardMovedMessage_WhenCheckingWetherToSendReboundMessage_SaysNo()
        {
            Guid storyCardId = Guid.NewGuid();
            StoryCard card = new StoryCard { Id = storyCardId };
            double x = 125.0;

            StoryCardMovedMessageFilter filter = new StoryCardMovedMessageFilter();

            filter.Enqueue(storyCardId, x, 0.0, 0.0);

            StoryCardMoved outboundMessage = new StoryCardMoved(storyCardId, x, 0.0, 0.0);

            bool letPass = filter.ShouldLetPass(outboundMessage);

            Assert.IsFalse(letPass);
        }
    }
}
