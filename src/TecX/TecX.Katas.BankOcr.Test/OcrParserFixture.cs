namespace TecX.Katas.BankOcr.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using TecX.Common;

    using Xunit;

    public class OcrParserFixture
    {
        [Fact]
        public void FactMethodName()
        {
            string s = "";

            MemoryStream stream = new MemoryStream();

            StreamWriter writer = new StreamWriter(stream);

            writer.Write(s);

            writer.Flush();

            stream.Position = 0;

            using (TextReader reader = new StreamReader(stream))
            {
                OcrParser parser = new OcrParser();

                AccountNumberParseResult result = parser.Parse(reader).Single();

                Assert.Equal(345882865, result.AccountNumber);
                Assert.Equal(AccountNumberStatus.Valid, result.Status);
            }
        }
    }

    public abstract class AccountNumberStatus
    {
        public static readonly AccountNumberStatus Valid = new ValidStatus();

        public static readonly AccountNumberStatus Illicit = new IllicitStatus();

        public static readonly AccountNumberStatus Error = new ErrorStatus();

        private class ErrorStatus : AccountNumberStatus
        {
            public override string ToString()
            {
                return "ERR";
            }
        }

        private class IllicitStatus : AccountNumberStatus
        {
            public override string ToString()
            {
                return "ILL";
            }
        }

        private class ValidStatus : AccountNumberStatus
        {
            public override string ToString()
            {
                return string.Empty;
            }
        }
    }

    public class AccountNumberParseResult
    {
        private readonly int accountNumber;

        private readonly AccountNumberStatus status;

        public AccountNumberParseResult(int accountNumber, AccountNumberStatus status)
        {
            this.accountNumber = accountNumber;
            this.status = status;
        }

        public int AccountNumber
        {
            get
            {
                return accountNumber;
            }
        }

        public AccountNumberStatus Status
        {
            get
            {
                return status;
            }
        }
    }

    public class OcrParser
    {
        public IEnumerable<AccountNumberParseResult> Parse(TextReader reader)
        {
            Guard.AssertNotNull(reader, "reader");

            NumeralStringTokenizer tokenizer = new NumeralStringTokenizer(reader);

            do
            {
                string[] tokens = tokenizer.Take(9).ToArray();

                Digit[] digits = new Digit[9];

                for (int i = 0; i < 9; i++)
                {
                    try
                    {
                        digits[i] = new Digit(tokens[i]);
                    }
                    catch (ArgumentException)
                    {
                    }
                }
            }
            while (tokenizer.GetNext());

            throw new NotImplementedException();
        }
    }
}
