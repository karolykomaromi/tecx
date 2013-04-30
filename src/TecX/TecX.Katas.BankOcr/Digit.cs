namespace TecX.Katas.BankOcr
{
    using System;
    using System.Linq;

    using TecX.Common;

    public struct Digit : IEquatable<Digit>
    {
        #region Constants

        public static readonly Digit Zero = new Digit(" _ \r\n" +
                                                      "| |\r\n" +
                                                      "|_|");

        public static readonly Digit One = new Digit("   \r\n" +
                                                     "  |\r\n" +
                                                     "  |");

        public static readonly Digit Two = new Digit(" _ \r\n" +
                                                     " _|\r\n" +
                                                     "|_ ");

        public static readonly Digit Three = new Digit(" _ \r\n" +
                                                       " _|\r\n" +
                                                       " _|");

        public static readonly Digit Four = new Digit("   \r\n" +
                                                      "|_|\r\n" +
                                                      "  |");

        public static readonly Digit Five = new Digit(" _ \r\n" +
                                                      "|_ \r\n" +
                                                      " _|");

        public static readonly Digit Six = new Digit(" _ \r\n" +
                                                     "|_ \r\n" +
                                                     "|_|");

        public static readonly Digit Seven = new Digit(" _ \r\n" +
                                                       "  |\r\n" +
                                                       "  |");

        public static readonly Digit Eight = new Digit(" _ \r\n" +
                                                       "|_|\r\n" +
                                                       "|_|");

        public static readonly Digit Nine = new Digit(" _ \r\n" +
                                                      "|_|\r\n" +
                                                      " _|");

        #endregion Constants

        private readonly string digit;

        public Digit(string digit)
        {
            Guard.AssertNotEmpty(digit, "digit");
            AssertValidCharacters(digit);
            Assert3By3(digit);
            this.digit = digit;
        }

        public static implicit operator Digit(int digit)
        {
            if (digit < 0 || digit > 9)
            {
                string msg = string.Format("Digit must be between 0 and 9 but is {0}.", digit);

                throw new InvalidCastException(msg);
            }

            switch (digit)
            {
                case 0:
                    return Zero;
                case 1:
                    return One;
                case 2:
                    return Two;
                case 3:
                    return Three;
                case 4:
                    return Four;
                case 5:
                    return Five;
                case 6:
                    return Six;
                case 7:
                    return Seven;
                case 8:
                    return Eight;
                case 9:
                    return Nine;
                default:
                    return Zero;
            }
        }

        public bool Equals(Digit other)
        {
            bool equals = string.Equals(this.digit, other.digit, StringComparison.Ordinal);

            return equals;
        }

        public override int GetHashCode()
        {
            return this.digit.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Digit))
            {
                return false;
            }

            return this.Equals((Digit)obj);
        }

        public override string ToString()
        {
            return this.digit;
        }

        private static void Assert3By3(string token)
        {
        }

        private static void AssertValidCharacters(string token)
        {
            if (!token.All(c => c == '|' || c == ' ' || c == '_' || c == '\r' || c == '\n'))
            {
                throw new ArgumentOutOfRangeException(token, "Token must only contain '|', '_', ' ' or newline characters.");
            }
        }
    }
}