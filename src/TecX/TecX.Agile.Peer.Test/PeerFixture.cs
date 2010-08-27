using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

namespace TecX.Agile.Peer.Test
{
    [TestClass]
    public class PeerFixture
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void CanExchangeMessagesViaPeer()
        {
            bool messageReceived = false;

            using (new PeerServiceHost())
            {
                PeerClient peer1 = new PeerClient();

                PeerClient peer2 = new PeerClient();

                peer1.MovedStoryCard += (s, e) => Assert.Fail("message must not bounce back");

                peer2.MovedStoryCard += (s, e) =>
                                            {
                                                messageReceived = true;
                                            };

                peer1.MoveStoryCard(peer1.Id, Guid.NewGuid(), 1.2, 2.3, 3.4);

            }

            Assert.IsTrue(messageReceived);
        }
    }
}
