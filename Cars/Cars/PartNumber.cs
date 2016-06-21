using System;
using System.Diagnostics.Contracts;

namespace Cars
{
    public struct PartNumber : IEquatable<PartNumber>, IComparable<PartNumber>
    {
        public static readonly PartNumber Empty = new PartNumber("EMPTY");

        public static readonly PartNumber[] None = new PartNumber[0];

        private readonly string partNumber;

        public PartNumber(string partNumber)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(partNumber));

            this.partNumber = partNumber.ToUpperInvariant();
        }

        public static implicit operator PartNumber(string partNumber)
        {
            return new PartNumber(partNumber);
        }

        public static bool operator ==(PartNumber pn1, PartNumber pn2)
        {
            return pn1.Equals(pn2);
        }

        public static bool operator !=(PartNumber pn1, PartNumber pn2)
        {
            return !pn1.Equals(pn2);
        }

        public int CompareTo(PartNumber other)
        {
            return string.Compare(this.partNumber, other.partNumber, StringComparison.Ordinal);
        }

        public bool Equals(PartNumber other)
        {
            return string.Equals(this.partNumber, other.partNumber, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (obj is PartNumber)
            {
                return this.Equals((PartNumber)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.partNumber.GetHashCode();
        }

        public override string ToString()
        {
            return this.partNumber;
        }
    }
}