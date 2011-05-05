using System;
using System.Text;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Infrastructure.Events.Builder;
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

                    PositionAndOrientation from = new PositionAndOrientation(0.0, 0.0, 0.0);
                    PositionAndOrientation to = new PositionAndOrientation(1.2, 2.3, 3.4);

                    peer1.MoveStoryCard(peer1.Id, Guid.NewGuid(), from, to);

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

            FieldHighlighted outboundMessage = new FieldHighlighted(artefactId, fieldName);

            filter.Enqueue(outboundMessage);
            
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
            const string newValue = "Some name";

            PropertyChangedMessageFilter filter = new PropertyChangedMessageFilter();

            PropertyUpdated outboundMessage = new PropertyUpdated(card.Id, propertyName, oldValue, newValue);

            filter.Enqueue(outboundMessage);

            bool letPass = filter.ShouldLetPass(outboundMessage);

            Assert.IsFalse(letPass);
        }

        [TestMethod]
        public void GivenAnInboundStoryCardMovedMessage_WhenCheckingWetherToSendReboundMessage_SaysNo()
        {
            Guid storyCardId = Guid.NewGuid();
            StoryCard card = new StoryCard { Id = storyCardId };
            const double x = 125.0;

            StoryCardMovedMessageFilter filter = new StoryCardMovedMessageFilter();

            StoryCardMoved outboundMessage = new StoryCardMovedMessageBuilder()
                                                    .MoveStoryCard(storyCardId)
                                                    .From(0.0, 0.0, 0.0)
                                                    .To(x, 0.0, 0.0);

            filter.Enqueue(outboundMessage);

            bool letPass = filter.ShouldLetPass(outboundMessage);

            Assert.IsFalse(letPass);
        }
    }
}
