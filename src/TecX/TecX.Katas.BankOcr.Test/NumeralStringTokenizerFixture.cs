namespace TecX.Katas.BankOcr.Test
{
    using System.IO;
    using System.Linq;
    using System.Text;

    using TecX.Katas.BankOcr;

    using Xunit;
    using Xunit.Extensions;

    public class NumeralStringTokenizerFixture
    {
        [Theory]
        [InlineData("    _  _     _  _  _  _  _ \r\n" +
                    "  | _| _||_||_ |_   ||_||_|\r\n" +
                    "  ||_  _|  | _||_|  ||_| _|\r\n" +
                    "\r\n")]
        public void GetsCorrect3By3Tokens(string s)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);

                writer.Write(s);

                writer.Flush();

                stream.Position = 0;

                using (StreamReader reader = new StreamReader(stream))
                {
                    var tokenizer = new NumeralStringTokenizer(reader);

                    Assert.Equal(new Digit[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, tokenizer.Select(d => new Digit(d)).ToArray());
                }
            }
        }
    }
}
