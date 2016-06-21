using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace Cars
{
    public class Part : IEquatable<Part>, IComparable<Part>
    {
        private readonly PartNumber partNumber;
        private readonly HashSet<PartNumber> replacesTheseParts;
        private readonly HashSet<PartNumber> cantBeCombinedWithTheseParts;

        private CurrencyAmount price;

        public Part(PartNumber partNumber)
            : this(partNumber, PartNumber.None, PartNumber.None)
        {
        }

        public Part(PartNumber partNumber, PartNumber[] replacesTheseParts, PartNumber[] cantBeCombinedWithTheseParts)
        {
            Contract.Requires(replacesTheseParts != null);
            Contract.Requires(cantBeCombinedWithTheseParts != null);

            this.partNumber = partNumber;
            this.replacesTheseParts = new HashSet<PartNumber>(replacesTheseParts);
            this.cantBeCombinedWithTheseParts = new HashSet<PartNumber>(cantBeCombinedWithTheseParts);

            this.price = new CurrencyAmount(0, Currency.Default);
        }

        public PartNumber PartNumber
        {
            get { return this.partNumber; }
        }

        public virtual IReadOnlyCollection<PartNumber> ReplacesTheseParts
        {
            get { return this.replacesTheseParts; }
        }

        public virtual IReadOnlyCollection<PartNumber> CantBeCombinedWithTheseParts
        {
            get { return this.cantBeCombinedWithTheseParts; }
        }

        public CurrencyAmount Price
        {
            get
            {
                return this.price;
            }

            set
            {
                Contract.Requires(value != null);

                this.price = value;
            }
        }

        public string ShortDescription { get; set; }

        public static bool operator ==(Part p1, Part p2)
        {
            if (object.ReferenceEquals(p1, p2))
            {
                return true;
            }

            if (object.ReferenceEquals(p1, null))
            {
                return false;
            }

            if (object.ReferenceEquals(p2, null))
            {
                return false;
            }

            return p1.Equals(p2);
        }

        public static bool operator !=(Part p1, Part p2)
        {
            return !(p1 == p2);
        }

        public int CompareTo(Part other)
        {
            if (other == null)
            {
                return 1;
            }

            return this.PartNumber.CompareTo(other.PartNumber);
        }

        public bool Equals(Part other)
        {
            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.PartNumber == other.PartNumber;
        }

        public override bool Equals(object obj)
        {
            Part other = obj as Part;

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            return this.PartNumber.GetHashCode();
        }

        public override string ToString()
        {
            return this.PartNumber.ToString();
        }
    }
}