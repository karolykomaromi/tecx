using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TecX.Common;

namespace TecX.Agile.Peer.Test
{
    [TestClass]
    public class PeerFixture
    {
        [TestMethod]
        public void CanExchangeMessageBetweenPeers()
        {
            PeerClient peer1 = new PeerClient();
            PeerClient peer2 = new PeerClient();

            Guid id = Guid.NewGuid();

            bool messageReceived = false;

            peer2.MovedStoryCard += (s, e) =>
                                        {
                                            Assert.AreEqual(id, e.Id);
                                            Assert.AreEqual(1d, e.X);
                                            Assert.AreEqual(2d, e.Y);
                                            Assert.AreEqual(3d, e.Angle);

                                            messageReceived = true;
                                        };

            peer1.MovedStoryCard += (s, e) => Assert.Fail("sender must not receive own message");
       
            peer1.MoveStoryCard(id, 1d, 2d, 3d);

            Assert.IsTrue(messageReceived);

        }
    }

    public interface IMessageBroker
    {
        void PublishMoveStoryCard(Guid senderId, Guid id, double dx, double dy, double angle);
    }

    public class PeerClient
    {
        private readonly IMessageBroker _messageBroker;
        private readonly Guid _senderId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PeerClient"/> class
        /// </summary>
        public PeerClient(IMessageBroker messageBroker)
        {
            Guard.AssertNotNull(messageBroker, "messageBroker");

            _messageBroker = messageBroker;
            _senderId = Guid.NewGuid();
        }

        public void MoveStoryCard(Guid id, double dx, double dy, double angle)
        {
            _messageBroker.PublishMoveStoryCard(_senderId, id, dx, dy, angle);
        }

        public event EventHandler<MovedStoryCardEventArgs> MovedStoryCard = delegate  {};
    }

    public class MovedStoryCardEventArgs : EventArgs
    {
        public double X { get; set; }
        public Guid Id { get; set; }

        public double Y { get; set; }

        public double Angle { get; set; }
    }
}
