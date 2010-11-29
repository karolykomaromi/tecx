﻿using System;
using System.Text;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Agile.Infrastructure.Events;
using TecX.Agile.Remote;

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
    }
}
