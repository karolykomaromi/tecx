namespace Hydra.Import.Test.Messages
{
    using Hydra.Import.Messages;
    using Xunit;

    public class ImportMessagesTests
    {
        [Fact]
        public void Should_Not_Add_Empty_Messages()
        {
            ImportMessages sut = new ImportMessages();

            sut.Add(ImportMessage.Empty);

            Assert.Equal(0, sut.Count);
        }
    }
}
