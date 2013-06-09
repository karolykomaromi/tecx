namespace TecX.Katas.BankOcr.Test
{
    using TecX.Katas.BankOcr;

    using Xunit;
    using Xunit.Extensions;

    public class BankOcrReaderFixture
    {
        [Theory]
        [InlineData(" _ \r\n" +
                    "| |\r\n" +
                    "|_|", 0)]
        [InlineData("   \r\n" +
                    "  |\r\n" +
                    "  |", 1)]
        [InlineData(" _ \r\n" +
                    " _|\r\n" +
                    "|_ ", 2)]
        [InlineData(" _ \r\n" +
                    " _|\r\n" +
                    " _|", 3)]
        [InlineData("   \r\n" +
                    "|_|\r\n" +
                    "  |", 4)]
        [InlineData(" _ \r\n" +
                    "|_ \r\n" +
                    " _|", 5)]
        [InlineData(" _ \r\n" +
                    "|_ \r\n" +
                    "|_|", 6)]
        [InlineData(" _ \r\n" +
                    "  |\r\n" +
                    "  |", 7)]
        [InlineData(" _ \r\n" +
                    "|_|\r\n" +
                    "|_|", 8)]
        [InlineData(" _ \r\n" +
                    "|_|\r\n" +
                    " _|", 9)]
        [InlineData("    _  _     _  _  _  _  _ \r\n" +
                    "  | _| _||_||_ |_   ||_||_|\r\n" +
                    "  ||_  _|  | _||_|  ||_| _|", 123456789)]
        [InlineData("    _  _     _  _  _  _  _ \r\n" +
                    "  | _| _||_||_ |_   ||_||_|\r\n" +
                    "  ||_  _|  | _||_|  ||_||_|", 123456788)]
        [InlineData("    _  _     _  _  _  _  _ \r\n" +
                    "  | _| _||_||_ |_   ||_||_|\r\n" +
                    "  | _| _|  | _||_|  ||_||_|", 133456788)]
        [InlineData("    _  _     _  _  _  _  _ \r\n" +
                    "  | _| _||_||_ |_   ||_|| |\r\n" +
                    "  | _| _|  | _||_|  ||_||_|", 133456780)]
        public void ParsesCorrectNumber(string input, int expected)
        {
            BankOcrReader sut = new BankOcrReader();

            Assert.Equal(expected, sut.Parse(input));
        }
    }
}
