namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class MailCharge : IComparable<MailCharge>, IEquatable<MailCharge>, ICloneable<MailCharge>
    {
        public static readonly MailCharge Empty;

        private static readonly DateTime Zero;

        private readonly DateTime timestamp;

        private readonly string id;

        static MailCharge()
        {
            DateTime zero = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            Zero = zero;

            Empty = new MailCharge(zero);
        }

        private MailCharge(DateTime timestamp)
        {
            this.timestamp = timestamp;

            long ticks = this.timestamp.Ticks - Zero.Ticks;

            string hex = ticks.ToString(FormatStrings.Numeric.Hex);

            this.id = Hyphenate(hex);
        }

        public DateTime Timestamp
        {
            get { return this.timestamp; }
        }

        public string Id
        {
            get { return this.id; }
        }

        public static MailCharge NewCharge()
        {
            DateTime timestamp = TimeProvider.UtcNow;

            return new MailCharge(timestamp);
        }

        public static bool TryParse(string s, out MailCharge charge)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                charge = MailCharge.Empty;
                return false;
            }

            long t;
            if (long.TryParse(s.Replace("-", string.Empty), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out t))
            {
                DateTime dt = new DateTime(Zero.Ticks + t, DateTimeKind.Utc);

                charge = new MailCharge(dt);
                return true;
            }

            charge = MailCharge.Empty;
            return false;
        }

        public MailCharge Clone()
        {
            return new MailCharge(this.Timestamp);
        }

        public int CompareTo(MailCharge other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.Timestamp.CompareTo(other.Timestamp);
        }

        public bool Equals(MailCharge other)
        {
            if (other == null)
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            return this.Timestamp.Equals(other.Timestamp);
        }

        public override bool Equals(object obj)
        {
            MailCharge other = obj as MailCharge;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.Timestamp.GetHashCode();
        }

        public override string ToString()
        {
            return this.Id;
        }

        private static string Hyphenate(string s)
        {
            var chunks = Chunk(s, 4);

            return string.Join("-", chunks);
        }

        private static IEnumerable<string> Chunk(string s, int chunkSize)
        {
            for (var i = 0; i < s.Length; i += chunkSize)
            {
                yield return s.Substring(i, Math.Min(chunkSize, s.Length - i));
            }
        }
    }
}