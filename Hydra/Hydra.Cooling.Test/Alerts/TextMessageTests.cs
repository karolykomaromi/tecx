namespace Hydra.Cooling.Test.Alerts
{
    using System.Linq;
    using Hydra.Cooling.Alerts;
    using Xunit;

    public class TextMessageTests
    {
        [Fact]
        public void Should_Split_Overly_Large_Messages_In_Chunks_Of_160_Letters()
        {
            string overlyLargeMessage = string.Concat(Enumerable.Range(0, 330).Select(i => (int)(i / 160)));

            TextMessage originalMessage = new TextMessage(PhoneNumber.Empty, overlyLargeMessage);

            TextMessage[] chunkifiedMessages = originalMessage.Chunkify().ToArray();

            Assert.Equal(3, chunkifiedMessages.Length);
            Assert.True(chunkifiedMessages[0].Message.All(c => c == '0'));
            Assert.Equal(160, chunkifiedMessages[0].Message.Length);
            Assert.Equal(1u, chunkifiedMessages[0].PartNumber);
            Assert.Equal(3u, chunkifiedMessages[0].PartsTotal);
            Assert.True(chunkifiedMessages[1].Message.All(c => c == '1'));
            Assert.Equal(160, chunkifiedMessages[1].Message.Length);
            Assert.Equal(2u, chunkifiedMessages[1].PartNumber);
            Assert.Equal(3u, chunkifiedMessages[1].PartsTotal);
            Assert.True(chunkifiedMessages[2].Message.All(c => c == '2'));
            Assert.Equal(10, chunkifiedMessages[2].Message.Length);
            Assert.Equal(3u, chunkifiedMessages[2].PartNumber);
            Assert.Equal(3u, chunkifiedMessages[2].PartsTotal);
        }
    }
}