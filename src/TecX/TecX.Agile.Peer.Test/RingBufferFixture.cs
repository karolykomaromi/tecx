using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TecX.Agile.Peer.Test
{
    [TestClass]
    public class RingBufferFixture
    {
        [TestMethod]
        public void GivenARingBuffer_WhenAddingMoreItemsThanAllowedByCapacity_RemovesFirstItem()
        {
            var buffer = new RingMessageHistory<int>(1, EqualityComparer<int>.Default);

            buffer.Add(1);

            buffer.Add(2);

            Assert.AreEqual(1, buffer.Count);
            Assert.IsFalse(buffer.Contains(1));
            Assert.IsTrue(buffer.Contains(2));
        }
    }
}
