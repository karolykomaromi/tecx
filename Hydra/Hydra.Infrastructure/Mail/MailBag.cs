namespace Hydra.Infrastructure.Mail
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public class MailBag : IComparable<MailBag>, IEquatable<MailBag>, ICloneable<MailBag>
    {
        public static readonly MailBag Empty;

        private static readonly DateTime Zero;

        private readonly DateTime timestamp;

        private readonly string id;

        static MailBag()
        {
            DateTime zero = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            Zero = zero;

            Empty = new MailBag(zero);
        }

        private MailBag(DateTime timestamp)
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

        public static MailBag NewBag()
        {
            DateTime timestamp = TimeProvider.UtcNow;

            return new MailBag(timestamp);
        }

        public static bool TryParse(string s, out MailBag bag)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                bag = MailBag.Empty;
                return false;
            }

            long t;
            if (long.TryParse(s.Replace("-", string.Empty), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out t))
            {
                DateTime dt = new DateTime(Zero.Ticks + t, DateTimeKind.Utc);

                bag = new MailBag(dt);
                return true;
            }

            bag = MailBag.Empty;
            return false;
        }

        public MailBag Clone()
        {
            return new MailBag(this.Timestamp);
        }

        public int CompareTo(MailBag other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.Timestamp.CompareTo(other.Timestamp);
        }

        public bool Equals(MailBag other)
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
            MailBag other = obj as MailBag;

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